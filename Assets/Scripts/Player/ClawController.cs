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

	private int numBulletBlocks;
	private int bulletBlocksLeft;

	private readonly EventBrokerComponent eventBroker = new EventBrokerComponent();

	private void Awake()
	{
		coll = GetComponent<CircleCollider2D>();
		sr = GetComponent<SpriteRenderer>();
	}

	private void Start()
	{
		numBulletBlocks = Constants.Claw.BaseNumBulletBlocks;
		isGrabbing = false;
	}

	private void HandleUpdateClawState(BrokerEvent<PlayerEvents.UpdateClawState> inEvent)
	{
		if (inEvent.Payload.ClawState == Constants.Claw.States.Extending)
		{
			isGrabbing = true;
			sr.sprite = clawOpenSprite;
			bulletBlocksLeft = numBulletBlocks;
		}
		else if (inEvent.Payload.ClawState == Constants.Claw.States.Retracting)
		{
			isGrabbing = false;
		}
	}

	private void HandleStartGame(BrokerEvent<GameEvents.StartGame> inEvent)
	{
		numBulletBlocks = Constants.Claw.BaseNumBulletBlocks;
		isGrabbing = false;
	}

	private void HandleUpgradeBulletBlocks(BrokerEvent<PlayerEvents.UpgradeBulletBlocks> inEvent)
	{
		numBulletBlocks += inEvent.Payload.Amount;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<EnemyAbstract>() != null && isGrabbing)
		{
			// Collided with enemy
			eventBroker.Publish(this, new PlayerEvents.UpdateClawState(Constants.Claw.States.Grabbed, collision.gameObject));
			collision.GetComponent<EnemyAbstract>().Captured();
			sr.sprite = clawClosedSprite;
		}
		
		if (collision.GetComponentInParent<AO_Projectile>() != null && bulletBlocksLeft > 0)
		{
			// Collided with basic enemy projectile
			Destroy(collision.transform.parent.gameObject);
			bulletBlocksLeft -= 1;
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.GetComponent<GenericEnemy>() != null && isGrabbing)
		{
			// Collided with enemy
			eventBroker.Publish(this, new PlayerEvents.UpdateClawState(Constants.Claw.States.Grabbed, collision.gameObject));
			collision.GetComponent<EnemyAbstract>().Captured();
			sr.sprite = clawClosedSprite;
		}
		
		if (collision.GetComponentInParent<AO_Projectile>() != null && bulletBlocksLeft > 0)
		{
			// Collided with basic enemy projectile
			Destroy(collision.transform.parent.gameObject);
			bulletBlocksLeft -= 1;
		}
	}

	private void OnEnable()
	{
		eventBroker.Subscribe<PlayerEvents.UpdateClawState>(HandleUpdateClawState);
		eventBroker.Subscribe<PlayerEvents.UpgradeBulletBlocks>(HandleUpgradeBulletBlocks);
		eventBroker.Subscribe<GameEvents.StartGame>(HandleStartGame);
	}

	private void OnDisable()
	{
		eventBroker.Unsubscribe<PlayerEvents.UpdateClawState>(HandleUpdateClawState);
		eventBroker.Unsubscribe<PlayerEvents.UpgradeBulletBlocks>(HandleUpgradeBulletBlocks);
		eventBroker.Unsubscribe<GameEvents.StartGame>(HandleStartGame);
	}
}
