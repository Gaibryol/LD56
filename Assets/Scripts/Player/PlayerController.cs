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
	[SerializeField] private List<Constants.Enemy.EnemyType> passengers;

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

	private Rigidbody2D rbody;

	public int health;
	private float moveSpeed;
	private float clawSpeed;
	private float clawDistance;
	private float invulnerabilityDuration;

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

		health = Constants.Player.BaseHealth;
		moveSpeed = Constants.Player.BaseMoveSpeed;
		clawSpeed = Constants.Claw.BaseClawSpeed;
		clawDistance = Constants.Claw.BaseClawDistance;
		invulnerabilityDuration = Constants.Player.BaseInvulnerabilityDuration;

		numBunny = 0;
		numChicken = 0;
		numCrab = 0;
		numFrog = 0;
		numShark = 0;
		numSquid = 0;
		numHippo = 0;

		// Clear passengers
		for (int i = 0; i < passengerSpots.Count; i++)
		{
			passengerSpots[i].sprite = null;
		}
		passengers = new List<Constants.Enemy.EnemyType>() { Constants.Enemy.EnemyType.None, Constants.Enemy.EnemyType.None, Constants.Enemy.EnemyType.None, Constants.Enemy.EnemyType.None, Constants.Enemy.EnemyType.None };
	}

    // Update is called once per frame
    void Update()
    {
		if (isGrabbing || health <= 0) return;

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
		if (isGrabbing || health <= 0)
		{
			rbody.velocity = Vector2.zero;
			return;
		}

		// Player Movement
		rbody.velocity = move.ReadValue<Vector2>() * moveSpeed;
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

	private void Upgrade(Constants.Enemy.EnemyType enemyType)
	{
		switch (enemyType)
		{
			case Constants.Enemy.EnemyType.Bunny:
				clawDistance += Constants.Claw.ClawDistanceIncrement;
				break;

			case Constants.Enemy.EnemyType.Chicken:
				eventBroker.Publish(this, new GameEvents.EarnScoreMultiplier(Constants.Player.ScoreMultiplierIncrement));
				break;

			case Constants.Enemy.EnemyType.Crab:
				eventBroker.Publish(this, new PlayerEvents.UpgradeBulletBlocks(Constants.Claw.NumBulletBlocksIncrement));
				break;

			case Constants.Enemy.EnemyType.Frog:
				clawSpeed += Constants.Claw.ClawSpeedIncrement;
				break;

			case Constants.Enemy.EnemyType.Hippo:
				if (health < Constants.Player.BaseHealth)
				{
					health += Constants.Player.HealthIncrement;
					eventBroker.Publish(this, new PlayerEvents.Heal(Constants.Player.HealthIncrement));
				}
				break;

			case Constants.Enemy.EnemyType.Shark:
				moveSpeed += Constants.Player.MoveSpeedIncrement;
				break;

			case Constants.Enemy.EnemyType.Squid:
				invulnerabilityDuration += Constants.Player.InvulnerabilityDurationIncrement;
				break;
		}
	}

	private IEnumerator ExtendCoroutine()
	{
		isGrabbing = true;
		eventBroker.Publish(this, new PlayerEvents.UpdateClawState(Constants.Claw.States.Extending));

		float clawScaleY = baseClawScale.y;
		while (clawScaleY < baseClawScale.y + clawDistance)
		{
			clawScaleY += Time.deltaTime * clawSpeed;
			claw.transform.localScale = new Vector3(claw.transform.localScale.x, clawScaleY, claw.transform.localScale.z);
			clawHead.transform.localScale = new Vector3(clawHead.transform.localScale.x, baseHeadScale.y / claw.transform.localScale.y, clawHead.transform.localScale.z);
			yield return null;
		}

		eventBroker.Publish(this, new PlayerEvents.UpdateClawState(Constants.Claw.States.Retracting));

		clawScaleY = claw.transform.localScale.y;
		while (clawScaleY > baseClawScale.y)
		{
			clawScaleY -= Time.deltaTime * clawSpeed;
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
			clawScaleY -= Time.deltaTime * clawSpeed;
			claw.transform.localScale = new Vector3(claw.transform.localScale.x, clawScaleY, claw.transform.localScale.z);
			clawHead.transform.localScale = new Vector3(clawHead.transform.localScale.x, baseHeadScale.y / claw.transform.localScale.y, clawHead.transform.localScale.z);

			// Move grabbed creature
			grabbedObj.position = clawHead.transform.position;
			yield return null;
		}

		claw.transform.localScale = baseClawScale;
		clawHead.transform.localScale = baseHeadScale;

		isGrabbing = false;

		// Check what type of creature
		Constants.Enemy.EnemyType enemyType = grabbedObj.GetComponent<GenericEnemy>().enemyType;

		// Check if open passenger spot
		int passengerIndex = GetNextOpenPassengerSpot();
		if (passengerIndex != -1)
		{
			// Add to passengers
			switch (enemyType)
			{
				case Constants.Enemy.EnemyType.Bunny:
					passengerSpots[passengerIndex].sprite = bunnySprite;
					passengerSpots[passengerIndex].transform.localPosition = bunnyPositions[passengerIndex];
					numBunny += 1;
					break;

				case Constants.Enemy.EnemyType.Chicken:
					passengerSpots[passengerIndex].sprite = chickenSprite;
					passengerSpots[passengerIndex].transform.localPosition = chickenPositions[passengerIndex];
					numChicken += 1;
					break;

				case Constants.Enemy.EnemyType.Crab:
					passengerSpots[passengerIndex].sprite = crabSprite;
					passengerSpots[passengerIndex].transform.localPosition = crabPositions[passengerIndex];
					numCrab += 1;
					break;

				case Constants.Enemy.EnemyType.Frog:
					passengerSpots[passengerIndex].sprite = frogSprite;
					passengerSpots[passengerIndex].transform.localPosition = frogPositions[passengerIndex];
					numFrog += 1;
					break;

				case Constants.Enemy.EnemyType.Hippo:
					passengerSpots[passengerIndex].sprite = hippoSprite;
					passengerSpots[passengerIndex].transform.localPosition = hippoPositions[passengerIndex];
					numHippo += 1;
					break;

				case Constants.Enemy.EnemyType.Shark:
					passengerSpots[passengerIndex].sprite = sharkSprite;
					passengerSpots[passengerIndex].transform.localPosition = sharkPositions[passengerIndex];
					numShark += 1;
					break;

				case Constants.Enemy.EnemyType.Squid:
					passengerSpots[passengerIndex].sprite = squidSprite;
					passengerSpots[passengerIndex].transform.localPosition = squidPositions[passengerIndex];
					numSquid += 1;
					break;
			}

			passengers[passengerIndex] = enemyType;

			// Get indexes of animal type
			List<int> indexes = new List<int>();
			for (int i = 0; i < passengers.Count; i++)
			{
				if (passengers[i] == enemyType)
				{
					indexes.Add(i);
				}
			}

			// Check if enough to upgrade in passengers + current animal being grabbed
			if (indexes.Count == Constants.Player.NumToUpgrade)
			{
				// Upgrade
				Upgrade(enemyType);

				// Remove passengers
				foreach (int index in indexes)
				{
					passengers[index] = Constants.Enemy.EnemyType.None;
					passengerSpots[index].sprite = null;
				}
			}
		}
		else
		{
			// No spots left in passengers
			switch (enemyType)
			{
				case Constants.Enemy.EnemyType.Bunny:
					numBunny += 1;
					break;

				case Constants.Enemy.EnemyType.Chicken:
					numChicken += 1;
					break;

				case Constants.Enemy.EnemyType.Crab:
					numCrab += 1;
					break;

				case Constants.Enemy.EnemyType.Frog:
					numFrog += 1;
					break;

				case Constants.Enemy.EnemyType.Hippo:
					numHippo += 1;
					break;

				case Constants.Enemy.EnemyType.Shark:
					numShark += 1;
					break;

				case Constants.Enemy.EnemyType.Squid:
					numSquid += 1;
					break;
			}

			// Get indexes of animal type
			List<int> indexes = new List<int>();
			for (int i = 0; i < passengers.Count; i++)
			{
				if (passengers[i] == enemyType)
				{
					indexes.Add(i);
				}
			}

			// Check if enough to upgrade in passengers + current animal being grabbed
			if (indexes.Count + 1 == Constants.Player.NumToUpgrade)
			{
				// Upgrade
				Upgrade(enemyType);

				// Remove passengers
				foreach (int index in indexes)
				{
					passengers[index] = Constants.Enemy.EnemyType.None;
					passengerSpots[index].sprite = null;
				}
			}
		}

		// Destroy grabbed object
		Destroy(grabbedObj.gameObject);

		// Get score
		eventBroker.Publish(this, new GameEvents.EarnScore(Constants.Player.ScoreEarnedOnGrab));
	}

	private IEnumerator HitCoroutine()
	{
		eventBroker.Publish(this, new PlayerEvents.UpdateInvulnerability(true));

		float timer = 0f;
		while (timer < invulnerabilityDuration)
		{
			timer += Constants.Player.InvulnerableTick;

			pixieSR.color = new Color(pixieSR.color.r, pixieSR.color.g, pixieSR.color.b, pixieSR.color.a == 1f ? Constants.Player.InvulnerableAlpha : 1f);
			for (int i = 0; i < passengerSpots.Count; i++)
			{
				passengerSpots[i].color = new Color(passengerSpots[i].color.r, passengerSpots[i].color.g, passengerSpots[i].color.b, passengerSpots[i].color.a == 1f ? Constants.Player.InvulnerableAlpha : 1f);
			}

			yield return new WaitForSeconds(Constants.Player.InvulnerableTick);
		}

		pixieSR.color = new Color(pixieSR.color.r, pixieSR.color.g, pixieSR.color.b, 1f);
		for (int i = 0; i < passengerSpots.Count; i++)
		{
			passengerSpots[i].color = new Color(passengerSpots[i].color.r, passengerSpots[i].color.g, passengerSpots[i].color.b, 1f);
		}

		eventBroker.Publish(this, new PlayerEvents.UpdateInvulnerability(false));
	}

	private void OnMove(InputAction.CallbackContext context)
	{
		// Flip character sprite if moving left or right
		if (context.ReadValue<Vector2>().x != 0 && !isGrabbing)
		{
			pixieSR.flipX = context.ReadValue<Vector2>().x < 0;
		}
	}

	private void OnGrab(InputAction.CallbackContext context)
	{
		if (isGrabbing || health <= 0) return;

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
		passengers = new List<Constants.Enemy.EnemyType>() { Constants.Enemy.EnemyType.None, Constants.Enemy.EnemyType.None, Constants.Enemy.EnemyType.None, Constants.Enemy.EnemyType.None, Constants.Enemy.EnemyType.None };

		// Reset player
		transform.position = Vector3.zero;
		isGrabbing = false;

		health = Constants.Player.BaseHealth;
		moveSpeed = Constants.Player.BaseMoveSpeed;
		clawSpeed = Constants.Claw.BaseClawSpeed;
		clawDistance = Constants.Claw.BaseClawDistance;
		invulnerabilityDuration = Constants.Player.BaseInvulnerabilityDuration;

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
}
