using System.Collections;
using UnityEngine;

public class AB_RadialProjectile : EnemyAttackBehaviour
{
    [SerializeField] private EnemyAttackObject attackObjectToSpawn;
    [SerializeField] private int numberOfProjectiles = 10;
    [SerializeField] private float angle = 180;
    [SerializeField] private float radius = 1;
    [SerializeField] private float delayBetweenProjectiles = 0;
    [SerializeField] private Transform originTransform;
    [SerializeField] private Transform targetTransform;
    protected override void Start()
    {
        base.Start();
        if (originTransform == null)
        {
            originTransform = transform;
        }

        if (targetTransform == null)
        {
            targetTransform = Camera.main.transform;
        }
    }

    public override IEnumerator Attack(TriggerAnimation triggerAnimation)
    {
        yield return base.Attack(triggerAnimation);

        StartCoroutine(SpawnProjectile());

        yield return null;
    }


    private IEnumerator SpawnProjectile()
    {
        Vector3 direction = targetTransform.position - originTransform.position;
        direction.z = 0;
        Vector3 directionNormalized = Vector3.Normalize(direction);

        float centerSpawnAngle = Mathf.Atan2(directionNormalized.y, directionNormalized.x) * Mathf.Rad2Deg;
        float startAngle = centerSpawnAngle - (angle / 2);

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            yield return new WaitForSeconds(delayBetweenProjectiles);
            float spawnAngle = startAngle + (angle / (numberOfProjectiles - 1)) * i;
            float rad_angle = spawnAngle * Mathf.Deg2Rad;

            Vector3 spawnPosition = new Vector2(Mathf.Cos(rad_angle), Mathf.Sin(rad_angle)) * radius;

            Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * spawnPosition;
            Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);

            Instantiate(attackObjectToSpawn, originTransform.position + spawnPosition, targetRotation);
        }

        CurrentlyAttacking = false;

    }
}
