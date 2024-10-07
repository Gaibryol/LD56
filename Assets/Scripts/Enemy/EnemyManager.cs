using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private float startingNumberOfEnemies = 10;
    private float maxNumberOfEnemies = 10;
    [SerializeField] private float enemiesKilledScaling = 1;
    [SerializeField] private float timeScaling = 0.05f;

    [SerializeField] private List<EnemyAbstract> enemyTypesToSpawn;

    private bool gameStarted = false;

    private float gameTime = 0;
    private int enemiesKilled = 0;
    private int enemiesSpawned = 0;
    private int currentDifficultyRating = 1;

	private List<EnemyAbstract> enemiesKilledByRainbow;

    private Dictionary<EnemyAbstract, List<EnemyAbstract>> activeEnemies = new Dictionary<EnemyAbstract, List<EnemyAbstract>>();
    private readonly EventBrokerComponent eventBroker = new EventBrokerComponent();

    void Start()
    {
		enemiesKilledByRainbow = new List<EnemyAbstract>();
    }

    private void OnEnable()
    {
        eventBroker.Subscribe<GameEvents.StartGame>(StartGameHandler);
        eventBroker.Subscribe<GameEvents.EndGame>(EndGameHandler);
        eventBroker.Subscribe<PlayerEvents.Die>(PlayerDieHandler);
		eventBroker.Subscribe<PlayerEvents.RainbowKilledEnemy>(RainbowKilledEnemyHandler);
    }

	private void OnDisable()
    {
        eventBroker.Unsubscribe<GameEvents.StartGame>(StartGameHandler);
        eventBroker.Unsubscribe<GameEvents.EndGame>(EndGameHandler);
        eventBroker.Unsubscribe<PlayerEvents.Die>(PlayerDieHandler);
		eventBroker.Unsubscribe<PlayerEvents.RainbowKilledEnemy>(RainbowKilledEnemyHandler);
	}

    // Update is called once per frame
    void Update()
    {
        if (!gameStarted) return;
        gameTime += Time.deltaTime;
        ScaleDifficulty();
        SpawnEnemies();
        CullOutOfBounds();
    }

    private void EndGameHandler(BrokerEvent<GameEvents.EndGame> @event)
    {
        gameStarted = false;
    }

    private void PlayerDieHandler(BrokerEvent<PlayerEvents.Die> @event)
    {
        CleanUp();
        gameStarted = false;
    }

    private void StartGameHandler(BrokerEvent<GameEvents.StartGame> @event)
    {
        enemiesKilled = 0;
        gameTime = 0;
        enemiesSpawned = 0;
        activeEnemies = new Dictionary<EnemyAbstract, List<EnemyAbstract>>();
		enemiesKilledByRainbow = new List<EnemyAbstract>();

		if (@event.Payload.Difficulty == Constants.Difficulty.Easy)
		{
			enemiesKilledScaling = 1f;
			timeScaling = 0.075f;
			startingNumberOfEnemies = 10;
		}
		else if (@event.Payload.Difficulty == Constants.Difficulty.Hard)
		{
			enemiesKilledScaling = 1.35f;
			timeScaling = 0.15f;
			startingNumberOfEnemies = 15;
		}

        foreach (EnemyAbstract enemy in enemyTypesToSpawn)
        {
            activeEnemies[enemy] = new List<EnemyAbstract>();
        }

        CleanUp();
        
        gameStarted = true;
    }

	private void RainbowKilledEnemyHandler(BrokerEvent<PlayerEvents.RainbowKilledEnemy> inEvent)
	{
		foreach (KeyValuePair<EnemyAbstract, List<EnemyAbstract>> pair in activeEnemies)
		{
			int index = pair.Value.IndexOf(inEvent.Payload.Enemy);
			if (index != -1)
			{
				// Found enemy
				enemiesKilledByRainbow.Add(inEvent.Payload.Enemy);
			}
		}
	}

	private void CleanUp()
    {
        foreach (EnemyAbstract enemy in FindObjectsByType<EnemyAbstract>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID))
        {
            Destroy(enemy.gameObject);
        }

        foreach (EnemyAttackObject enemy in FindObjectsByType<EnemyAttackObject>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID))
        {
            Destroy(enemy.gameObject);
        }
    }
    
    private void ScaleDifficulty()
    {
        // based on enemies killed, and game time
        maxNumberOfEnemies = startingNumberOfEnemies + enemiesKilledScaling * enemiesKilled;
        maxNumberOfEnemies += timeScaling * gameTime;
        currentDifficultyRating = Mathf.RoundToInt(maxNumberOfEnemies / startingNumberOfEnemies);
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

					int index = enemiesKilledByRainbow.IndexOf(enemy);
					if (index != - 1)
					{
						enemiesKilledByRainbow.RemoveAt(index);
					}
					else
					{
						enemiesKilled++;
					}
                    continue;
                }
                if (EnemyUtilities.OutOfBounds(enemy.transform.position, Constants.EnemyManager.maxValidDistanceAwayFromScreen))
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
        if (enemiesSpawned >= maxNumberOfEnemies) return;
        foreach (EnemyAbstract enemy in enemyTypesToSpawn)
        {
            float spawnChance = Random.value;
            if (enemy.SpawnChance(gameTime, enemiesKilled, activeEnemies[enemy].Count, currentDifficultyRating) >= spawnChance)
            {
                EnemyAbstract spawnedEnemy = Instantiate(enemy, DetermineSpawnLocation(), Quaternion.identity);
                activeEnemies[enemy].Add(spawnedEnemy);
                enemiesSpawned++;
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
        bottomLeftBoundPosition -= (Vector3) Constants.EnemyManager.screenSpawnBuffer;
        topRightBoundPosition += (Vector3) Constants.EnemyManager.screenSpawnBuffer;

        // Pick a random spawn edge
        float rand_edge = Random.value;

        Vector3 spawnPosition = new Vector3();

        Vector2 maxSpawnDistanceFromBuffer = Constants.EnemyManager.maxSpawnDistanceFromBuffer;
        float x_spawnOffset = Random.Range(0, maxSpawnDistanceFromBuffer.x);
        float y_spawnOffset = Random.Range(0, maxSpawnDistanceFromBuffer.y);
        float x_randSpawnValue = Random.Range(bottomLeftBoundPosition.x - maxSpawnDistanceFromBuffer.x, topRightBoundPosition.x + maxSpawnDistanceFromBuffer.x);
        float y_randSpawnValue = Random.Range(bottomLeftBoundPosition.y - maxSpawnDistanceFromBuffer.y, topRightBoundPosition.y + maxSpawnDistanceFromBuffer.y);

        if (rand_edge < 0.25)
        {
            // pick to spawn left edge of screen
            spawnPosition.x = bottomLeftBoundPosition.x - x_spawnOffset;
            spawnPosition.y = y_randSpawnValue;
        } else if (rand_edge < 0.5)
        {
            // spawn on right edge of screen
            spawnPosition.x = topRightBoundPosition.x + x_spawnOffset;
            spawnPosition.y = y_randSpawnValue;
        }
        else if  (rand_edge < 0.75)
        {
            // spawn on top edge of screen
            spawnPosition.x = x_randSpawnValue;
            spawnPosition.y = topRightBoundPosition.y + y_spawnOffset;
        } else
        {
            // spawn bottom edge of screen
            spawnPosition.x = x_randSpawnValue;
            spawnPosition.y = bottomLeftBoundPosition.y - y_spawnOffset;
        }
        return spawnPosition;
    }
}
