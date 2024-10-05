using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	[SerializeField, Header("Pixie")] private SpriteRenderer pixieSR;
	[SerializeField] private GameObject pixie;

	[SerializeField, Header("Claw")] private GameObject claw;
	[SerializeField] private GameObject clawHead;
	[SerializeField] private Sprite clawHeadOpenSprite;
	[SerializeField] private Sprite clawHeadClosedSprite;

	[SerializeField, Header("Passengers")] private List<SpriteRenderer> passengerSpots;
	[SerializeField] private Sprite bunnySprite;
	[SerializeField] private Sprite chickenSprite;
	[SerializeField] private Sprite crabSprite;
	[SerializeField] private Sprite frogSprite;
	[SerializeField] private Sprite sharkSprite;
	[SerializeField] private Sprite squidSprite;
	[SerializeField] private Sprite hippoSprite;

	[SerializeField, Header("Passenger Positions")] private List<Vector2> bunnyPositions;
	[SerializeField] private List<Vector2> chickenPositions;
	[SerializeField] private List<Vector2> crabPositions;
	[SerializeField] private List<Vector2> frogPositions;
	[SerializeField] private List<Vector2> sharkPositions;
	[SerializeField] private List<Vector2> squidPositions;
	[SerializeField] private List<Vector2> hippoPositions;

	[SerializeField, Header("Inputs")] private InputAction move;
	[SerializeField] private InputAction grab;

	private bool isGrabbing;
	private bool isInvulnerable;

	private Rigidbody2D rbody;

	private int health;
	private int numBunny;
	private int numChicken;
	private int numCrab;
	private int numFrog;
	private int numShark;
	private int numSquid;
	private int numHippo;

	Vector3 baseClawScale;
	Vector3 baseHeadScale;
	private Coroutine grabCoroutine;

	private readonly EventBrokerComponent eventBroker = new EventBrokerComponent();

	private void Awake()
	{
		rbody = GetComponent<Rigidbody2D>();

		baseClawScale = claw.transform.localScale;
		baseHeadScale = clawHead.transform.localScale;
	}

	// Start is called before the first frame update
	void Start()
    {
		isGrabbing = false;
		isInvulnerable = false;
		health = Constants.Player.BaseHealth;

		numBunny = 0;
		numChicken = 0;
		numCrab = 0;
		numFrog = 0;
		numShark = 0;
		numSquid = 0;
		numHippo = 0;
	}

    // Update is called once per frame
    void Update()
    {
		if (isGrabbing || isInvulnerable) return;

		// Claw Rotation
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
		mousePos.z = 0f;

		Vector3 direction = (mousePos - transform.position).normalized;
		claw.transform.position = transform.position + direction * Constants.Claw.ClawOffset;

		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		claw.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90f));
    }

	private void FixedUpdate()
	{
		// Stop movement if shooting
		if (isGrabbing || isInvulnerable)
		{
			rbody.velocity = Vector2.zero;
			return;
		}

		// Player Movement
		rbody.velocity = move.ReadValue<Vector2>() * Constants.Player.Movespeed;
	}

	private int GetNextOpenPassengerSpot()
	{
		for(int i = 0; i < passengerSpots.Count; i++)
		{
			if (passengerSpots[i].sprite == null)
			{
				return i;
			}
		}

		return -1;
	}

	private IEnumerator ExtendCoroutine()
	{
		isGrabbing = true;
		eventBroker.Publish(this, new PlayerEvents.UpdateClawState(Constants.Claw.States.Extending));

		float clawScaleY = baseClawScale.y;
		while (clawScaleY < baseClawScale.y + Constants.Claw.ClawDistance)
		{
			clawScaleY += Time.deltaTime * Constants.Claw.ClawSpeed;
			claw.transform.localScale = new Vector3(claw.transform.localScale.x, clawScaleY, claw.transform.localScale.z);
			clawHead.transform.localScale = new Vector3(clawHead.transform.localScale.x, baseHeadScale.y / claw.transform.localScale.y, clawHead.transform.localScale.z);
			yield return null;
		}

		eventBroker.Publish(this, new PlayerEvents.UpdateClawState(Constants.Claw.States.Retracting));

		clawScaleY = claw.transform.localScale.y;
		while (clawScaleY > baseClawScale.y)
		{
			clawScaleY -= Time.deltaTime * Constants.Claw.ClawSpeed;
			claw.transform.localScale = new Vector3(claw.transform.localScale.x, clawScaleY, claw.transform.localScale.z);
			clawHead.transform.localScale = new Vector3(clawHead.transform.localScale.x, baseHeadScale.y / claw.transform.localScale.y, clawHead.transform.localScale.z);
			yield return null;
		}

		claw.transform.localScale = baseClawScale;
		clawHead.transform.localScale = baseHeadScale;

		isGrabbing = false;
	}

	private IEnumerator RetractCoroutine(Transform grabbedObj)
	{
		eventBroker.Publish(this, new PlayerEvents.UpdateClawState(Constants.Claw.States.Retracting));

		float clawScaleY = claw.transform.localScale.y;
		while (clawScaleY > baseClawScale.y)
		{
			// Scale claw
			clawScaleY -= Time.deltaTime * Constants.Claw.ClawSpeed;
			claw.transform.localScale = new Vector3(claw.transform.localScale.x, clawScaleY, claw.transform.localScale.z);
			clawHead.transform.localScale = new Vector3(clawHead.transform.localScale.x, baseHeadScale.y / claw.transform.localScale.y, clawHead.transform.localScale.z);

			// Move grabbed creature
			grabbedObj.position = clawHead.transform.position;
			yield return null;
		}

		claw.transform.localScale = baseClawScale;
		clawHead.transform.localScale = baseHeadScale;

		isGrabbing = false;

		// Add creature to passenger
		int passengerIndex = GetNextOpenPassengerSpot();
		if (passengerIndex != -1)
		{
			// Check what type of creature
			passengerSpots[passengerIndex].sprite = frogSprite;
			passengerSpots[passengerIndex].transform.localPosition = frogPositions[passengerIndex];
		}

		// Destroy grabbed object
		Destroy(grabbedObj.gameObject);
	}

	private IEnumerator HitCoroutine()
	{
		isInvulnerable = true;

		float timer = 0f;
		while (timer < Constants.Player.InvulnerableDuration)
		{
			timer += Constants.Player.InvulnerableTick;

			pixie.SetActive(!pixie.activeSelf);
			yield return new WaitForSeconds(Constants.Player.InvulnerableTick);
		}

		pixieSR.gameObject.SetActive(true);
		isInvulnerable = false;
	}

	private void OnMove(InputAction.CallbackContext context)
	{
		// Flip character sprite if moving left or right
		if (context.ReadValue<Vector2>().x != 0 && !(isInvulnerable || isGrabbing))
		{
			pixieSR.flipX = context.ReadValue<Vector2>().x < 0;
		}
	}

	private void OnGrab(InputAction.CallbackContext context)
	{
		if (isGrabbing) return;

		grabCoroutine = StartCoroutine(ExtendCoroutine());
	}

	private void HandlePlayerDamage(BrokerEvent<PlayerEvents.Damage> inEvent)
	{
		health -= inEvent.Payload.Amount;

		if (health <= 0)
		{
			eventBroker.Publish(this, new PlayerEvents.Die(numBunny, numChicken, numCrab, numFrog, numShark, numSquid, numHippo));
		}
		else
		{
			Debug.Log("Hit");
			StartCoroutine(HitCoroutine());
		}
	}

	private void HandleUpdateClawState(BrokerEvent<PlayerEvents.UpdateClawState> inEvent)
	{
		if (inEvent.Payload.ClawState == Constants.Claw.States.Grabbed)
		{
			// Stop extending, start retracting immediately
			if (grabCoroutine != null)
			{
				StopCoroutine(grabCoroutine);
				StartCoroutine(RetractCoroutine(inEvent.Payload.GrabbedObj.transform));
			}
		}
	}

	private void HandleStartGame(BrokerEvent<GameEvents.StartGame> inEvent)
	{
		// Clear passengers
		for(int i = 0; i < passengerSpots.Count; i++)
		{
			passengerSpots[i].sprite = null;
		}

		// Reset player
		transform.position = Vector3.zero;
		isGrabbing = false;
		health = Constants.Player.BaseHealth;

		numBunny = 0;
		numChicken = 0;
		numCrab = 0;
		numFrog = 0;
		numShark = 0;
		numSquid = 0;
		numHippo = 0;
	}

	private void OnEnable()
	{
		move.performed += OnMove;
		grab.performed += OnGrab;

		move.Enable();
		grab.Enable();

		eventBroker.Subscribe<PlayerEvents.Damage>(HandlePlayerDamage);
		eventBroker.Subscribe<PlayerEvents.UpdateClawState>(HandleUpdateClawState);
		eventBroker.Subscribe<GameEvents.StartGame>(HandleStartGame);
	}

	private void OnDisable()
	{
		move.performed -= OnMove;
		grab.performed -= OnGrab;

		move.Disable();
		grab.Disable();

		eventBroker.Unsubscribe<PlayerEvents.Damage>(HandlePlayerDamage);
		eventBroker.Unsubscribe<PlayerEvents.UpdateClawState>(HandleUpdateClawState);
		eventBroker.Unsubscribe<GameEvents.StartGame>(HandleStartGame);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Test function for collisions
		if (collision.tag == "Enemy" && !isInvulnerable)
		{
			eventBroker.Publish(this, new PlayerEvents.Damage(1));
		}
	}
}
