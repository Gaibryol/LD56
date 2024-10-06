using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemy : EnemyAbstract
{

    [SerializeField] private float animationAttackDelay;

    protected override void Start()
    {
        base.Start();
    }
    public override float OnAttack()
    {
        base.OnAttack();
        animator.SetTrigger("Attack");
        return animationAttackDelay;
    }
}
