using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAbstract : MonoBehaviour
{
    public Constants.Enemy.EnemyType enemyType;
    protected EnemyMoveBehaviour moveBehaviour;
    protected EnemyAttackBehaviour attackBehaviour;

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
        moveBehaviour.Move();
        attackBehaviour.Attack();
    }

    protected virtual void Captured()
    {

    }

    public abstract float SpawnChance(float gameTime, int enemiesKilled, int enemiesRemaining, int sameEnemiesRemaining);
}


