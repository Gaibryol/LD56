using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squid : EnemyAbstract
{
    [SerializeField] private float animationAttackDelay;
    [SerializeField] private Transform targetTransform;
    private Vector3 previousFramePosition;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        Vector3 movementDirection = transform.position - previousFramePosition;
        Vector3 movementDirectionNormalized = movementDirection.normalized;

        targetTransform.position = transform.position + movementDirectionNormalized;
        previousFramePosition = transform.position;
    }

    public override float OnAttack()
    {
        base.OnAttack();
        animator.SetTrigger("Attack");
        return animationAttackDelay;
    }
}
