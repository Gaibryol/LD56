using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAbstract : MonoBehaviour
{
    public Constants.Enemy.EnemyType enemyType;
    public int difficultyRating;
    protected EnemyMoveBehaviour moveBehaviour;
    protected EnemyAttackBehaviour attackBehaviour;
    [SerializeField] private bool canMoveWhileAttacking = true;
    private bool captured;

    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

	private readonly EventBrokerComponent eventBroker = new EventBrokerComponent();

    private float squidAudioTimer = 0;

    protected virtual void Awake()
    {
        moveBehaviour = GetComponent<EnemyMoveBehaviour>();
        attackBehaviour = GetComponent<EnemyAttackBehaviour>();
    }

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        squidAudioTimer = Time.time;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (captured) return;
        attackBehaviour.QueueAttack(OnAttack);
        if (canMoveWhileAttacking || (!canMoveWhileAttacking && !attackBehaviour.CurrentlyAttacking))
        {
            moveBehaviour.Move();
        }
    }

    public virtual void Captured()
    {
        captured = true;
        attackBehaviour.StopAttack();
    }

    public virtual float SpawnChance(float gameTime, int enemiesKilled, int sameEnemiesRemaining, int worldDifficultyRating)
    {
        // the number of enemy that can exist of this type on the field is equal to the current difficult rating / enemy difficult rating
        // so if the difficulty rating is 4, i'm allowed 1 shark or 1 hippo, or 2 frogs, or 4 chicken
        int numberOfTypeAllowed = Mathf.FloorToInt(worldDifficultyRating / this.difficultyRating);
        if (sameEnemiesRemaining >= numberOfTypeAllowed) return 0;  // at cap already

        return 1;
    }

    public virtual float OnAttack()
    {
        spriteRenderer.flipX = transform.position.x < Camera.main.transform.position.x;

		switch (enemyType)
		{
			case Constants.Enemy.EnemyType.Bunny:
				eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.BunnyShoot));
				break;

			case Constants.Enemy.EnemyType.Chicken:
				eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.EggLaid));
				break;

			case Constants.Enemy.EnemyType.Crab:
				eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.CrabGun));
				break;

			case Constants.Enemy.EnemyType.Hippo:
				eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.HippoBurst));
				break;

			case Constants.Enemy.EnemyType.Shark:
				eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.SharkSpray));
				break;

			case Constants.Enemy.EnemyType.Squid:
                if (squidAudioTimer + 7 > Time.time)
                {
                    break;
                }
                squidAudioTimer = Time.time;
				eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.SquidInk));
				break;
		}

        return 0;
    }
}


