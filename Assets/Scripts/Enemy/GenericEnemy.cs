using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class GenericEnemy : EnemyAbstract
{

    private Animator animator;
    [SerializeField] private float animationAttackDelay;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }
    public override float OnAttack()
    {
        animator.SetTrigger("Attack");
        return animationAttackDelay;
    }
}
