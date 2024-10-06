using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : EnemyAbstract
{


    protected override void Start()
    {
        base.Start();
    }


    public override float OnAttack()
    {
        base.OnAttack();
        return .8f;
    }

    protected override void Update()
    {
        base.Update();
        animator.SetBool("Attack", attackBehaviour.CurrentlyAttacking);
    }
}
