using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MB_CircleAlongLine : EnemyMoveBehaviour
{
    [SerializeField] float circleRadius = 1f;
    [SerializeField] float loops = 3;
    [SerializeField] private float xDirection;
    [SerializeField] private float yDirection;

    private Vector2 initialPosition;
    private float angle = 0;

    private void Start()
    {
        initialPosition = transform.position;
    }

    public override void Move()
    {
        angle += speed * Time.deltaTime;
        float x = circleRadius * Mathf.Cos(loops * angle) + xDirection * angle - circleRadius;
        float y = circleRadius * Mathf.Sin(loops * angle) + yDirection * angle;

        transform.position = initialPosition + new Vector2(x, y);
    }
}
