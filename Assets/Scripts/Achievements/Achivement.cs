using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achivement : MonoBehaviour
{
    protected Constants.Difficulty achievementDifficulty;
    [SerializeField] protected string achievementKey;
    [SerializeField] private bool displayMax;
    [SerializeField] private GameObject obtainedBadge;

    private readonly EventBrokerComponent eventBroker = new EventBrokerComponent();


    private void OnEnable()
    {
        eventBroker.Subscribe<GameEvents.NotifyAchievementButtonPressed>(OnAchievementButtonPressed);
        UpdateAchievement();
    }

    
    private void OnDisable()
    {
        eventBroker.Unsubscribe<GameEvents.NotifyAchievementButtonPressed>(OnAchievementButtonPressed);
    }

    private void OnAchievementButtonPressed(BrokerEvent<GameEvents.NotifyAchievementButtonPressed> @event)
    {
        achievementDifficulty = @event.Payload.Difficulty;
    }

    private void UpdateAchievement()
    {
        string key = achievementKey + "_" + achievementDifficulty.ToString();
        bool hasKey = PlayerPrefs.HasKey(key);
        if (!hasKey)
        {
            obtainedBadge.SetActive(false);
            return;
        }

        obtainedBadge.SetActive(true);

        float value = PlayerPrefs.GetFloat(key);
    }
}
