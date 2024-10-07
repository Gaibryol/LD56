using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor.PackageManager;
using UnityEngine;
using static Constants;
using static GameEvents;

public class AchievementManager : MonoBehaviour
{
	private readonly EventBrokerComponent eventBroker = new EventBrokerComponent();

    private Dictionary<Enemy.EnemyType, int> enemyCollectionCounter;

    private bool gameStarted = false;
    private int numberOfRainbowAttacks = 0;

    private void OnEnable()
    {
        eventBroker.Subscribe<StartGame>(OnStartGame);
        eventBroker.Subscribe<EndGame>(OnEndGame);
        eventBroker.Subscribe<PlayerEvents.UpdateClawState>(OnUpdateClawState);
        eventBroker.Subscribe<PlayerEvents.RainbowAttack>(OnRainbowAttack);
        eventBroker.Subscribe<NotifyAchievementObtained>(OnNotifyAchievementObtained);
    }

    private void OnDisable()
    {
        eventBroker.Unsubscribe<StartGame>(OnStartGame);
        eventBroker.Unsubscribe<EndGame>(OnEndGame);
        eventBroker.Unsubscribe<PlayerEvents.UpdateClawState>(OnUpdateClawState);
        eventBroker.Unsubscribe<PlayerEvents.RainbowAttack>(OnRainbowAttack);
        eventBroker.Unsubscribe<NotifyAchievementObtained>(OnNotifyAchievementObtained);

    }

    private void OnNotifyAchievementObtained(BrokerEvent<NotifyAchievementObtained> @event)
    {
        if (@event.Payload.NewValue < @event.Payload.Threshold) return;
        float currentSavedValue = PlayerPrefs.GetFloat(@event.Payload.Key, -1);
        if (@event.Payload.NewValue > currentSavedValue)
        {
            PlayerPrefs.SetFloat(@event.Payload.Key, @event.Payload.NewValue);
        }
    }

    private void OnRainbowAttack(BrokerEvent<PlayerEvents.RainbowAttack> @event)
    {
        if (!gameStarted) return;
        numberOfRainbowAttacks++;
    }

    private void OnEndGame(BrokerEvent<GameEvents.EndGame> @event)
    {
        gameStarted = false;
        Difficulty difficulty = @event.Payload.Difficulty;

        // Lover Series
        foreach (KeyValuePair<Enemy.EnemyType, int> kvp in enemyCollectionCounter)
        {
            string key = Achievements.LoverSeries.GetKeyForEnemyType(kvp.Key, difficulty);
            float currentValue = PlayerPrefs.GetFloat(key, -1);
            if (kvp.Value >= Achievements.LoverSeries.CountThreshold && kvp.Value > currentValue)
            {
                PlayerPrefs.SetFloat(Achievements.LoverSeries.GetKeyForEnemyType(kvp.Key, difficulty), kvp.Value);
            }
        }

        // Surivial Series
        float gameTime = @event.Payload.TotalGameTime;
        HandleThresholdAchievement(Achievements.SurvivalSeries.FirstThreshold, gameTime, difficulty);
        HandleThresholdAchievement(Achievements.SurvivalSeries.SecondThreshold, gameTime, difficulty);
        HandleThresholdAchievement(Achievements.SurvivalSeries.ThirdThreshold, gameTime, difficulty);

        // Score Series
        float finalScore = @event.Payload.FinalScore;
        HandleThresholdAchievement(Achievements.ScoreSeries.FirstThreshold, finalScore, difficulty);
        HandleThresholdAchievement(Achievements.ScoreSeries.SecondThreshold, finalScore, difficulty);
        HandleThresholdAchievement(Achievements.ScoreSeries.ThirdThreshold, finalScore, difficulty);


        // RainbowSeries
        HandleThresholdAchievement(Achievements.RainbowSeries.FirstThreshold, numberOfRainbowAttacks, difficulty);
        HandleThresholdAchievement(Achievements.RainbowSeries.SecondThreshold, numberOfRainbowAttacks, difficulty);
        HandleThresholdAchievement(Achievements.RainbowSeries.ThirdThreshold, numberOfRainbowAttacks, difficulty);

    }

    private void HandleThresholdAchievement((string, float) keyValue, float value, Difficulty difficulty)
    {
        string key = Achievements.GetKeyFromThreshold(keyValue.Item1, difficulty);
        float currentValue = PlayerPrefs.GetFloat(key, -1);
        if (value >= keyValue.Item2 && value >= currentValue)
        {
            PlayerPrefs.SetFloat(key, value);
        }
    }

    private void OnStartGame(BrokerEvent<StartGame> @event)
    {
        enemyCollectionCounter = new Dictionary<Enemy.EnemyType, int>();

        gameStarted = true;
        numberOfRainbowAttacks = 0;
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
