using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class MB_Cardioid : EnemyMoveBehaviour
{
    [SerializeField] private Direction cardioidDirection;
    [SerializeField] private float repetitions = 1; // leaves
    [SerializeField] private float spiral = 1;
    [SerializeField] private float size = 1;
    [SerializeField, Range(0f, 360f)] private float startingAngle = 0f;
    [SerializeField] private bool randomDirection = true;
    private int randomXDirection;
    private int randomYDirection;

    private float angle = 0;
    private Vector3 startingLocation;

    private void Start()
    {
        startingLocation = transform.position;
        if (randomDirection)
        {
            randomXDirection = UnityEngine.Random.value < 0.5 ? -1 : 1;
            randomYDirection = UnityEngine.Random.value < 0.5 ? -1 : 1;
            startingAngle = UnityEngine.Random.value * 360;
            size = UnityEngine.Random.value * 5;
        }
        angle = Mathf.Deg2Rad * startingAngle;
    }

    public override void Move()
    {
        angle = angle + speed * Time.deltaTime;

        float x = size * Mathf.Cos(angle * spiral) * (1 - Mathf.Cos(angle * repetitions)) * randomXDirection;
        float y = size * Mathf.Sin(angle * spiral) * (1 - Mathf.Cos(angle * repetitions)) * randomYDirection;
        
        transform.position = new Vector3(x + startingLocation.x, y + startingLocation.y, transform.position.z);
    }

    public enum Direction
    {
        Horizontal,
        Vertical
    }
}
