using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AB_FrogTongue : EnemyAttackBehaviour
{
    [SerializeField] private AO_FrogTongue frogTongue;



    public override IEnumerator Attack(TriggerAnimation triggerAnimation)
    {
        yield return base.Attack(triggerAnimation);
        frogTongue.Begin();


        yield return null;
    }

    protected override void Update()
    {
        base.Update();
        if (CurrentlyAttacking)
        {
            CurrentlyAttacking = frogTongue.started;
        }
    }
}
