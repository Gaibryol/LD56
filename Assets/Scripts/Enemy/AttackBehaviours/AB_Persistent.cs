using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AB_Persistent : EnemyAttackBehaviour
{
    [SerializeField] private EnemyAttackObject attackObjectToSpawn;

    public override bool Attack()
    {
        if(!base.Attack()) return false;

        Instantiate(attackObjectToSpawn, transform.position, Quaternion.identity);

        return true;
    }
}
