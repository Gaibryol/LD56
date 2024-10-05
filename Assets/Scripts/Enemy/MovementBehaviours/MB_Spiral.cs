using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MB_Spiral : EnemyMoveBehaviour
{
    [SerializeField] private float radius = 1;

    private float angle = 0;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
        radius *= Random.value < 0.5 ? 1 : -1;
    }

    public override void Move()
    {
        float x = radius * angle * Mathf.Cos(angle);
        float y = radius * angle * Mathf.Sin(angle);

        angle += speed * Time.deltaTime;

        transform.position = initialPosition + new Vector3(x, y, 0);
    }
}
