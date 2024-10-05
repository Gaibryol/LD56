using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.OnScreen.OnScreenStick;

public abstract class EnemyAbstract : MonoBehaviour
{
    public EnemyType enemyType;
    protected EnemyMoveBehaviour moveBehaviour;

    private void Awake()
    {
        moveBehaviour = GetComponent<EnemyMoveBehaviour>();
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        moveBehaviour.Move();
    }

    protected virtual void Attack()
    {

    }

    protected virtual void Captured()
    {

    }

    public abstract float SpawnChance(float gameTime, int enemiesKilled, int enemiesRemaining, int sameEnemiesRemaining);
}

public enum EnemyType
{
    Chicken,
    Octopus
}
