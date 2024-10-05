using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawController : MonoBehaviour
{
	[SerializeField, Header("Sprites")] private Sprite clawOpenSprite;
	[SerializeField] private Sprite clawClosedSprite;

	private CircleCollider2D coll;
	private SpriteRenderer sr;

	private bool isGrabbing;

	private readonly EventBrokerComponent eventBroker = new EventBrokerComponent();

	private void Awake()
	{
		coll = GetComponent<CircleCollider2D>();
		sr = GetComponent<SpriteRenderer>();
		isGrabbing = false;
	}

	private void HandleUpdateClawState(BrokerEvent<PlayerEvents.UpdateClawState> inEvent)
	{
		if (inEvent.Payload.ClawState == Constants.Claw.States.Extending)
		{
			isGrabbing = true;
			sr.sprite = clawOpenSprite;
		}
		else if (inEvent.Payload.ClawState == Constants.Claw.States.Retracting)
		{
			isGrabbing = false;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Enemy" && isGrabbing)
		{
			eventBroker.Publish(this, new PlayerEvents.UpdateClawState(Constants.Claw.States.Grabbed, collision.gameObject));
			sr.sprite = clawClosedSprite;
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.tag == "Enemy" && isGrabbing)
		{
			eventBroker.Publish(this, new PlayerEvents.UpdateClawState(Constants.Claw.States.Grabbed, collision.gameObject));
			sr.sprite = clawClosedSprite;
		}
	}

	private void OnEnable()
	{
		eventBroker.Subscribe<PlayerEvents.UpdateClawState>(HandleUpdateClawState);
	}

	private void OnDisable()
	{
		eventBroker.Unsubscribe<PlayerEvents.UpdateClawState>(HandleUpdateClawState);
	}
}
