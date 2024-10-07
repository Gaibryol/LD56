using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AB_FrogTongue : EnemyAttackBehaviour
{
    [SerializeField] private AO_FrogTongue frogTongue;

	private readonly EventBrokerComponent eventBroker = new EventBrokerComponent();

    public override IEnumerator Attack(TriggerAnimation triggerAnimation)
    {
        yield return base.Attack(triggerAnimation);
        frogTongue.Begin();
		eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.FrogTongue));
		yield return null;
    }

    protected override void Update()
    {
        base.Update();
        CurrentlyAttacking = frogTongue.started;
    }

    public override void StopAttack()
    {
        base.StopAttack();
        frogTongue.started = false;
        frogTongue.GetComponentInChildren<Collider2D>().enabled = false;
    }
}
