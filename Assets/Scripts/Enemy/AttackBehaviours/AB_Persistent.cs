using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AB_Persistent : EnemyAttackBehaviour
{
    [SerializeField] private EnemyAttackObject attackObjectToSpawn;

    public override IEnumerator Attack(TriggerAnimation triggerAnimation)
    {
        yield return base.Attack(triggerAnimation);

        Instantiate(attackObjectToSpawn, transform.position, Quaternion.identity);

        CurrentlyAttacking = false;

        yield return null;
    }
}
