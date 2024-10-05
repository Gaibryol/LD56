using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private float maxNumberOfEnemies = 100;
    [SerializeField] private List<EnemyAbstract> enemyTypesToSpawn;


    private float gameTime = 0;
    private int enemiesKilled = 0;
    private int enemiesSpawned = 0;
    private int totalEnemiesSpawned;

    private Vector2 screenSpawnBuffer = new Vector2(5, 5);
    private Vector2 maxSpawnDistanceFromBuffer = new Vector2(10, 10);
    private Vector2 maxValidDistanceAwayFromScreen = new Vector2(15, 15);

    private Dictionary<EnemyAbstract, List<EnemyAbstract>> activeEnemies = new Dictionary<EnemyAbstract, List<EnemyAbstract>>();

    void Start()
    {
        foreach (EnemyAbstract enemy in enemyTypesToSpawn)
        {
            activeEnemies[enemy] = new List<EnemyAbstract>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        SpawnEnemies();
        CullOutOfBounds();
    }

    private void CullOutOfBounds()
    {
        foreach (KeyValuePair<EnemyAbstract, List<EnemyAbstract>> keyValuePair in activeEnemies)
        {
            for (int i = keyValuePair.Value.Count - 1; i > -1; i--)
            {
                EnemyAbstract enemy = keyValuePair.Value[i];
                if (enemy == null)
                {
                    // Enemy has been destroyed by player.
                    keyValuePair.Value.RemoveAt(i);
                    continue;
                }
                if (EnemyOutOfBounds(enemy))
                {
                    Destroy(enemy.gameObject);
                    keyValuePair.Value.RemoveAt(i);
                    enemiesSpawned--;
                }
            }
        }
    }

    private void SpawnEnemies()
    {
        if (enemiesSpawned > maxNumberOfEnemies) return;
        foreach (EnemyAbstract enemy in enemyTypesToSpawn)
        {
            float spawnChance = Random.value;
            if (enemy.SpawnChance(gameTime, enemiesKilled, enemiesSpawned, activeEnemies[enemy].Count) >= spawnChance)
            {
                EnemyAbstract spawnedEnemy = Instantiate(enemy, DetermineSpawnLocation(), Quaternion.identity);
                activeEnemies[enemy].Add(spawnedEnemy);
                enemiesSpawned++;
                totalEnemiesSpawned++;
            }
        }
    }

    private Vector3 DetermineSpawnLocation()
    {
        // Requirements:
        // 1. Spawn out side of player view

        // Determine what the player can see
        Vector3 bottomLeftBoundPosition = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 topRightBoundPosition = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
        
        // Add an offset because we don't want to spawn on edge of the screen
        bottomLeftBoundPosition -= (Vector3) screenSpawnBuffer;
        topRightBoundPosition += (Vector3) screenSpawnBuffer;

        // Pick a random spawn point
        float rand_edge = Random.value;

        Vector3 spawnPosition = new Vector3();

        float x_spawnOffset = Random.Range(0, maxSpawnDistanceFromBuffer.x);
        float y_spawnOffset = Random.Range(0, maxSpawnDistanceFromBuffer.y);

        if (rand_edge < 0.25)
        {
            // pick to spawn left edge of screen
            spawnPosition.x = bottomLeftBoundPosition.x - x_spawnOffset;
            spawnPosition.y = Random.Range(bottomLeftBoundPosition.y - maxSpawnDistanceFromBuffer.y, topRightBoundPosition.y + maxSpawnDistanceFromBuffer.y);
        } else if (rand_edge < 0.5)
        {
            // spawn on right edge of screen
            spawnPosition.x = topRightBoundPosition.x + x_spawnOffset;
            spawnPosition.y = Random.Range(bottomLeftBoundPosition.y - maxSpawnDistanceFromBuffer.y, topRightBoundPosition.y + maxSpawnDistanceFromBuffer.y);
        }
        else if  (rand_edge < 0.75)
        {
            // spawn on top edge of screen
            spawnPosition.x = Random.Range(bottomLeftBoundPosition.x - maxSpawnDistanceFromBuffer.x, topRightBoundPosition.x + maxSpawnDistanceFromBuffer.x);
            spawnPosition.y = topRightBoundPosition.y + y_spawnOffset;
        } else
        {
            // spawn bottom edge of screen
            spawnPosition.x = Random.Range(bottomLeftBoundPosition.x - maxSpawnDistanceFromBuffer.x, topRightBoundPosition.x + maxSpawnDistanceFromBuffer.x);
            spawnPosition.y = bottomLeftBoundPosition.y - y_spawnOffset;
        }
        return spawnPosition;
    }

    private bool EnemyOutOfBounds(EnemyAbstract enemy)
    {
        // Determine what the player can see
        Vector3 bottomLeftBoundPosition = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 topRightBoundPosition = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));

        bottomLeftBoundPosition -= (Vector3)maxValidDistanceAwayFromScreen;
        topRightBoundPosition += (Vector3)maxValidDistanceAwayFromScreen;

        float x = enemy.transform.position.x;
        float y = enemy.transform.position.y;
        return x < bottomLeftBoundPosition.x || x > topRightBoundPosition.x || y > topRightBoundPosition.y || y < bottomLeftBoundPosition.y;
    }
}
