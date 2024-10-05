using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AB_DualProjectile : EnemyAttackBehaviour
{
    [SerializeField] private Transform[] spawnLocations;
    [SerializeField] private EnemyAttackObject attackObjectToSpawn;

    public override bool Attack()
    {
        if (!base.Attack()) return false;

        SpawnProjectileAtLocation(spawnLocations[0]);
        SpawnProjectileAtLocation(spawnLocations[1]);


        return true;
    }

    private void SpawnProjectileAtLocation(Transform position)
    {
        // Get direction to player/camera
        Vector3 direction = position.right;
        direction.z = 0;
        Vector3 directionNormalized = Vector3.Normalize(direction);
        Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * directionNormalized;
        Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);

        Instantiate(attackObjectToSpawn, position.position, targetRotation);
    }
}
