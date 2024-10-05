using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MB_Linear : EnemyMoveBehaviour
{
    [SerializeField] private float slope;
    [SerializeField] private bool randomSlope;

    private Vector3 initialPosition;
    private void Start()
    {
        initialPosition = transform.position;
        if (randomSlope)
        {
            slope = Random.value * slope;
        }
    }
    public override void Move()
    {
        float intercept = initialPosition.y - slope * initialPosition.x;

        float targetX = transform.position.x + speed * Time.deltaTime;
        float targetY = slope * targetX + intercept;
        Vector3 targetPosition = new Vector3(targetX, targetY, transform.position.z);

        transform.position = targetPosition;
    }
}
