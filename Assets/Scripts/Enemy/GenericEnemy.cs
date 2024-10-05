using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class GenericEnemy : EnemyAbstract
{
    public override float SpawnChance(float gameTime, int enemiesKilled, int enemiesRemaining, int sameEnemiesRemaining)
    {
        return 1f;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    

}
