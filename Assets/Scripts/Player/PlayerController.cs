using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[SerializeField, Header("Pixie")] private SpriteRenderer pixieSR;

	[SerializeField, Header("Claw")] private GameObject claw;
	[SerializeField] private GameObject clawHead;
	[SerializeField] private Sprite clawHeadOpenSprite;
	[SerializeField] private Sprite clawHeadClosedSprite;

	[SerializeField, Header("Inputs")] private InputAction move;
	[SerializeField] private InputAction shoot;

	private bool isShooting;

	private Rigidbody2D rbody;

	private int health;

	private readonly EventBrokerComponent eventBroker = new EventBrokerComponent();

	private void Awake()
	{
		rbody = GetComponent<Rigidbody2D>();
		health = Constants.Player.BaseHealth;
	}

	// Start is called before the first frame update
	void Start()
    {
		isShooting = false;
    }

    // Update is called once per frame
    void Update()
    {
		if (isShooting) return;

		// Claw Rotation
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
		mousePos.z = 0f;

		Vector3 direction = (mousePos - transform.position).normalized;
		claw.transform.position = transform.position + direction * Constants.Player.ClawOffset;

		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		claw.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90f));
    }

	private void FixedUpdate()
	{
		// Stop movement if shooting
		if (isShooting)
		{
			rbody.velocity = Vector2.zero;
			return;
		}

		// Player Movement
		rbody.velocity = move.ReadValue<Vector2>() * Constants.Player.Movespeed;
	}

	private IEnumerator ShootCoroutine()
	{
		isShooting = true;

		Vector3 baseScale = claw.transform.localScale;
		Vector3 baseHeadScale = clawHead.transform.localScale;

		float timer = 0f;
		while (timer < Constants.Player.ClawDuration * 0.5f)
		{
			timer += Time.deltaTime * Constants.Player.ClawSpeed;
			claw.transform.localScale = new Vector3(claw.transform.localScale.x, claw.transform.localScale.y + Time.deltaTime * Constants.Player.ClawSpeed, claw.transform.localScale.z);
			clawHead.transform.localScale = new Vector3(clawHead.transform.localScale.x, baseHeadScale.y / claw.transform.localScale.y, clawHead.transform.localScale.z);
			yield return null;
		}

		clawHead.GetComponent<SpriteRenderer>().sprite = clawHeadClosedSprite;

		while (timer > 0f)
		{
			timer -= Time.deltaTime * Constants.Player.ClawSpeed;

			claw.transform.localScale = new Vector3(claw.transform.localScale.x, claw.transform.localScale.y - Time.deltaTime * Constants.Player.ClawSpeed, claw.transform.localScale.z);
			clawHead.transform.localScale = new Vector3(clawHead.transform.localScale.x, baseHeadScale.y / claw.transform.localScale.y, clawHead.transform.localScale.z);
			yield return null;
		}

		claw.transform.localScale = baseScale;
		clawHead.transform.localScale = baseHeadScale;

		clawHead.GetComponent<SpriteRenderer>().sprite = clawHeadOpenSprite;
		isShooting = false;
	}

	private void OnMove(InputAction.CallbackContext context)
	{
		// Flip character sprite if moving left or right
		if (context.ReadValue<Vector2>().x != 0)
		{
			pixieSR.flipX = context.ReadValue<Vector2>().x < 0;
		}
	}

	private void OnShoot(InputAction.CallbackContext context)
	{
		if (isShooting) return;

		StartCoroutine(ShootCoroutine());
	}

	private void HandlePlayerDamage(BrokerEvent<PlayerEvents.Damage> inEvent)
	{
		health -= inEvent.Payload.Amount;

		if (health <= 0)
		{
			eventBroker.Publish(this, new PlayerEvents.Die());
		}
	}

	private void OnEnable()
	{
		move.performed += OnMove;
		shoot.performed += OnShoot;

		move.Enable();
		shoot.Enable();

		eventBroker.Subscribe<PlayerEvents.Damage>(HandlePlayerDamage);
	}

	private void OnDisable()
	{
		move.performed -= OnMove;
		shoot.performed -= OnShoot;

		move.Disable();
		shoot.Disable();

		eventBroker.Unsubscribe<PlayerEvents.Damage>(HandlePlayerDamage);
	}
}
