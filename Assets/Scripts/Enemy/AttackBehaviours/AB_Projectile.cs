using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AB_Projctile : EnemyAttackBehaviour
{
    
    [SerializeField] private EnemyAttackObject attackObjectToSpawn;

    public override bool Attack()
    {
        if (!base.Attack()) return false;

        // Get direction to player/camera
        Vector3 direction = Camera.main.transform.position - transform.position;
        direction.z = 0;
        Vector3 directionNormalized = Vector3.Normalize(direction);
        Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * directionNormalized;
        Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);

        Instantiate(attackObjectToSpawn, transform.position, targetRotation);

        return true;
    }
}
