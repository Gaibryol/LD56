using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AO_FrogTongue : EnemyAttackObject
{
    [SerializeField] private Transform tongue;
    [SerializeField] private float maxScale;
    [SerializeField] private float attackTime;
    private float t = 0;
    public bool started = false;

    private void Update()
    {
        float alpha = Mathf.Sin(Mathf.PI * t / attackTime);
        float xScale = Mathf.Lerp(0f, maxScale, alpha);
        tongue.localScale = new Vector3(xScale, tongue.localScale.y, tongue.localScale.z);
        if (started)
        {
            t = Mathf.Clamp(t + Time.deltaTime, 0f, attackTime);
        }
        if (t >= attackTime)
        {
            started = false;
        }
    }

    public void Begin()
    {
        // Get direction to player/camera
        Vector3 direction = Camera.main.transform.position - transform.position;
        direction.z = 0;
        Vector3 directionNormalized = Vector3.Normalize(direction);
        Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * directionNormalized;
        Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);
        transform.rotation = targetRotation;
        t = 0;
        started = true;
    }
}
