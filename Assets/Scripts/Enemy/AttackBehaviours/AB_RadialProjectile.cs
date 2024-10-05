using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AB_RadialProjectile : EnemyAttackBehaviour
{
    [SerializeField] private EnemyAttackObject attackObjectToSpawn;
    [SerializeField] private int numberOfProjectiles = 10;
    [SerializeField] private float angle = 180;
    [SerializeField] private float radius = 1;
    [SerializeField] private float delayBetweenProjectiles = 0;
    public override bool Attack()
    {
        if (!base.Attack()) return false;

        // Get direction to player/camera
        Vector3 direction = Camera.main.transform.position - transform.position;
        direction.z = 0;
        Vector3 directionNormalized = Vector3.Normalize(direction);

        float centerSpawnAngle = Mathf.Atan2(directionNormalized.y, directionNormalized.x) * Mathf.Rad2Deg;
        float startAngle = centerSpawnAngle - (angle / 2);

        StartCoroutine(SpawnProjectile(startAngle));

        return true;
    }

    private IEnumerator SpawnProjectile(float startAngle)
    {
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            float spawnAngle = startAngle + (angle / (numberOfProjectiles - 1)) * i;
            float rad_angle = spawnAngle * Mathf.Deg2Rad;

            Vector3 spawnPosition = new Vector2(Mathf.Cos(rad_angle), Mathf.Sin(rad_angle)) * radius;

            Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * spawnPosition;
            Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);

            Instantiate(attackObjectToSpawn, transform.position + spawnPosition, targetRotation);
            yield return new WaitForSeconds(delayBetweenProjectiles);
        }

    }
}
