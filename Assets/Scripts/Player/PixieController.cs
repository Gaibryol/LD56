using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixieController : MonoBehaviour
{
	private GameObject grabbedCreature;
	private bool isInvulnerable;

	private readonly EventBrokerComponent eventBroker = new EventBrokerComponent();

	private void Start()
	{
		grabbedCreature = null;
		isInvulnerable = false;
	}

	private void HandleUpdateClawState(BrokerEvent<PlayerEvents.UpdateClawState> inEvent)
	{
		if (inEvent.Payload.ClawState == Constants.Claw.States.Grabbed)
		{
			grabbedCreature = inEvent.Payload.GrabbedObj;
		}
	}

	private void HandleStartGame(BrokerEvent<GameEvents.StartGame> inEvent)
	{
		grabbedCreature = null;
		isInvulnerable = false;
	}

	private void HandleUpdateInvulnerability(BrokerEvent<PlayerEvents.UpdateInvulnerability> inEvent)
	{
		isInvulnerable = inEvent.Payload.NewState;
	}

	private void OnEnable()
	{
		eventBroker.Subscribe<GameEvents.StartGame>(HandleStartGame);
		eventBroker.Subscribe<PlayerEvents.UpdateClawState>(HandleUpdateClawState);
		eventBroker.Subscribe<PlayerEvents.UpdateInvulnerability>(HandleUpdateInvulnerability);
	}

	private void OnDisable()
	{
		eventBroker.Unsubscribe<GameEvents.StartGame>(HandleStartGame);
		eventBroker.Unsubscribe<PlayerEvents.UpdateClawState>(HandleUpdateClawState);
		eventBroker.Unsubscribe<PlayerEvents.UpdateInvulnerability>(HandleUpdateInvulnerability);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((collision.GetComponent<EnemyAbstract>() != null || collision.tag == "Enemy") && !isInvulnerable && collision.gameObject != grabbedCreature)
		{
			eventBroker.Publish(this, new PlayerEvents.Damage(1));
		}
	}
}
