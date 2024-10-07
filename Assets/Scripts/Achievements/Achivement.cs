using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Achivement : MonoBehaviour
{
    protected Constants.Difficulty achievementDifficulty;
    [SerializeField] protected string achievementKey;
    [SerializeField] private bool displayMax;

    [SerializeField] TMP_Text scoreText;

    [SerializeField] private Image rowImage;
    [SerializeField] private Sprite rowSpriteLocked;
    [SerializeField] private Sprite rowSpriteUnlocked;

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
        UpdateAchievement();
    }

    private void UpdateAchievement()
    {
        string key = achievementKey + "_" + achievementDifficulty.ToString();
        bool hasKey = PlayerPrefs.HasKey(key);
        if (!hasKey)
        {
            if (rowImage != null)
            {
                rowImage.sprite = rowSpriteLocked;
            }
            icon.sprite = iconEmpty;
            if (scoreText != null)
            {
                scoreText.text = "";
            }
            return;
        }

        float value = PlayerPrefs.GetFloat(key);
        if (scoreText != null)
        {
            if (displayMax)
            {
                scoreText.text = "Score " + value;
            } else
            {
                scoreText.text = "";
            }
        }

        if (rowImage != null)
        {
            rowImage.sprite = rowSpriteUnlocked;
        }
        icon.sprite = iconFilled;

    }
}
