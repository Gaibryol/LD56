using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MB_Lissajous : EnemyMoveBehaviour
{
    // https://www.desmos.com/calculator/tti5dasmc4

    [SerializeField] private bool randomScale = true;
    [SerializeField] private bool randomDirection = true;
    [SerializeField] private Vector2 scale = Vector2.one; // (A, B)
    [SerializeField] private float phase = 0.25f; // Will be multiplied by pi (f)
    [SerializeField] private Vector2 ratio = new Vector2(2, 3); // (a, b)
    private float t = 0;
    private Vector3 initialPostion;
    private float direction = 1;

    private void Start()
    {
        initialPostion = transform.position;
        if (randomScale)
        {
            scale.x *= Random.value < 0.5 ? 1 : -1;
            scale.y *= Random.value < 0.5 ? 1 : -1;
        }

        if (randomDirection)
        {
            direction = Random.value < 0.5 ? 1 : -1;
        }
    }

    public override void Move()
    {
        float x = scale.x * Mathf.Sin(ratio.x * t + phase * Mathf.PI);
        float y = scale.y * Mathf.Sin(ratio.y * t);

        transform.position = initialPostion + new Vector3(x, y, 0);
        t += speed * Time.deltaTime * direction;
    }
}
