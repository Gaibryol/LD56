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

    protected virtual void Awake()
    {
        moveBehaviour = GetComponent<EnemyMoveBehaviour>();
        attackBehaviour = GetComponent<EnemyAttackBehaviour>();
    }

    protected virtual void Start()
    {

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

    protected virtual void Captured()
    {
        captured = true;
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
        return 0;
    }
}


