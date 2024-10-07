using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achivement : MonoBehaviour
{
    protected Constants.Difficulty achievementDifficulty;
    [SerializeField] protected string achievementKey;
    [SerializeField] private bool displayMax;
    [SerializeField] private GameObject obtainedBadge;

    [SerializeField] private Image icon;
    [SerializeField] private Sprite iconEmpty;
    [SerializeField] private Sprite iconFilled;

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
            icon.sprite = iconEmpty;
            if (obtainedBadge != null)
            {
                obtainedBadge.SetActive(false);
            }
            return;
        }

        icon.sprite = iconFilled;
        if (obtainedBadge != null)
        {
            obtainedBadge.SetActive(true);
        }

        float value = PlayerPrefs.GetFloat(key);
    }
}
