using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : EnemyAbstract
{
    private Animator animator;


    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }


    public override float OnAttack()
    {
        return .8f;
    }

    protected override void Update()
    {
        base.Update();
        animator.SetBool("Attack", attackBehaviour.CurrentlyAttacking);
    }
}
