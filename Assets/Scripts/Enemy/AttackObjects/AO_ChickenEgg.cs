using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AO_ChickenEgg : AO_Persistent
{
    [SerializeField] private EnemyAttackBehaviour EnemyAttackBehaviour;
    [SerializeField] private Animator animator;
    private bool exploded;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (exploded && !EnemyAttackBehaviour.CurrentlyAttacking)
        {
            Destroy(gameObject);
        }
    }
    protected override void OnLifeSpanEnded()
    {
        // Explode
        EnemyAttackBehaviour.QueueAttack(StartAnimation);
        exploded = true;
        //base.OnLifeSpanEnded();
    }

    private float StartAnimation()
    {
        animator.SetTrigger("Explode");
        return .5f;
    }
}
