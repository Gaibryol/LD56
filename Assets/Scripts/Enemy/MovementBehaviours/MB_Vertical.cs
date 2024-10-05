using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class MB_Vertical : EnemyMoveBehaviour
{

    public override void Move()
    {
        float y = transform.position.y + speed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
