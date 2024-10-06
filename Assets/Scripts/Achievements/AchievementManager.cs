using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
	private readonly EventBrokerComponent eventBroker = new EventBrokerComponent();

    private Dictionary<Constants.Enemy.EnemyType, int> enemyCollectionCounter;

    private bool gameStarted = false;

    private void OnEnable()
    {
        eventBroker.Subscribe<GameEvents.StartGame>(OnStartGame);
        eventBroker.Subscribe<GameEvents.EndGame>(OnEndGame);
        eventBroker.Subscribe<PlayerEvents.UpdateClawState>(OnUpdateClawState);
    }

    private void OnDisable()
    {
        eventBroker.Unsubscribe<GameEvents.StartGame>(OnStartGame);
        eventBroker.Unsubscribe<GameEvents.EndGame>(OnEndGame);
        eventBroker.Unsubscribe<PlayerEvents.UpdateClawState>(OnUpdateClawState);
    }

    private void OnEndGame(BrokerEvent<GameEvents.EndGame> @event)
    {
        gameStarted = false;

        // Lover Series
        foreach (KeyValuePair<Constants.Enemy.EnemyType, int> kvp in enemyCollectionCounter)
        {
            if (kvp.Value >= Constants.Achievements.LoverSeries.CountThreshold)
            {
                PlayerPrefs.SetInt(Constants.Achievements.LoverSeries.GetKeyForEnemyType(kvp.Key), 1);
            }
        }

        // Survival Series
        if (@event.Payload.FinalScore >= Constants.Achievements.SurvivalSeries.FirstThreshold)
        {
            //PlayerPrefs.SetInt(Constants.Achievements.LoverSeries.GetKeyForEnemyType(kvp.Key), 1);
        }
    }

    private void OnStartGame(BrokerEvent<GameEvents.StartGame> @event)
    {
        enemyCollectionCounter = new Dictionary<Constants.Enemy.EnemyType, int>();

        gameStarted = true;
    }

    private void OnUpdateClawState(BrokerEvent<PlayerEvents.UpdateClawState> @event)
    {
        if (!gameStarted) return;

        if (@event.Payload.GrabbedObj == null) return;

        EnemyAbstract enemy = @event.Payload.GrabbedObj.GetComponent<EnemyAbstract>();
        if (!enemyCollectionCounter.ContainsKey(enemy.enemyType))
        {
            enemyCollectionCounter.Add(enemy.enemyType, 0);
        }

        enemyCollectionCounter[enemy.enemyType]++;
    }
}
