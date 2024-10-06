using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AB_Projctile : EnemyAttackBehaviour
{
    
    [SerializeField] private EnemyAttackObject attackObjectToSpawn;
    [SerializeField] private Transform projectileSpawnTransform;
    [SerializeField] private bool forceUpright = false;
    public override IEnumerator Attack(TriggerAnimation triggerAnimation)
    {
        yield return base.Attack(triggerAnimation);

        // Get direction to player/camera
        Quaternion targetRotation = Quaternion.identity;
        if (!forceUpright)
        {
            Vector3 direction = Camera.main.transform.position - projectileSpawnTransform.position;
            direction.z = 0;
            Vector3 directionNormalized = Vector3.Normalize(direction);
            Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * directionNormalized;
            targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);
        }

        Instantiate(attackObjectToSpawn, projectileSpawnTransform.position, targetRotation);

        CurrentlyAttacking = false;

        yield return null;
    }
}
