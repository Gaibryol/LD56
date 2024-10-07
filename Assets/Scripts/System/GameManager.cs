using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public float score;
	public float scoreMultiplier;

	private bool isPlaying;

	private Coroutine scoreCoroutine;

	private float startTime;

	private readonly EventBrokerComponent eventBroker = new EventBrokerComponent();

    // Start is called before the first frame update
    void Start()
    {
		isPlaying = false;
		score = 0f;
        startTime = 0;
		scoreMultiplier = Constants.Player.BaseScoreMultiplier;

		eventBroker.Publish(this, new AudioEvents.PlayMusic(Constants.Audio.Music.MainMenuTheme));
    }

	private IEnumerator IncrementScore()
	{
		while (isPlaying)
		{
			score += Constants.Player.ScoreEarnedPerSecond * scoreMultiplier;
			yield return new WaitForSeconds(1f);
		}
	}

	private void HandleStartGame(BrokerEvent<GameEvents.StartGame> inEvent)
	{
		score = 0f;
		scoreMultiplier = Constants.Player.BaseScoreMultiplier;
		isPlaying = true;
		startTime = Time.time;
		scoreCoroutine = StartCoroutine(IncrementScore());
	}

	private void HandleEarnScore(BrokerEvent<GameEvents.EarnScore> inEvent)
	{
		score += inEvent.Payload.Amount * scoreMultiplier;
	}

	private void HandleEarnScoreMultiplier(BrokerEvent<GameEvents.EarnScoreMultiplier> inEvent)
	{
		scoreMultiplier += inEvent.Payload.Amount;

		eventBroker.Publish(this, new GameEvents.NotifyAchievementObtained(Constants.Achievements.UpgradeSeries.ScoreMultiplier, scoreMultiplier));
	}

	private void HandlePlayerDie(BrokerEvent<PlayerEvents.Die> inEvent)
	{
		// End game
		StopCoroutine(scoreCoroutine);
		isPlaying = false;

		// Check for highscore
		if (PlayerPrefs.GetFloat(Constants.Game.HighscorePP, 0f) < score)
		{
			// New highscore
			PlayerPrefs.SetFloat(Constants.Game.HighscorePP, score);
			PlayerPrefs.Save();

			eventBroker.Publish(this, new GameEvents.EndGame(score, true, Time.time - startTime, Constants.Difficulty.Easy));
		}
		else
		{
			eventBroker.Publish(this, new GameEvents.EndGame(score, false, Time.time - startTime, Constants.Difficulty.Easy));
		}
	}

	private void OnEnable()
	{
		eventBroker.Subscribe<GameEvents.EarnScore>(HandleEarnScore);
		eventBroker.Subscribe<GameEvents.EarnScoreMultiplier>(HandleEarnScoreMultiplier);
		eventBroker.Subscribe<GameEvents.StartGame>(HandleStartGame);
		eventBroker.Subscribe<PlayerEvents.Die>(HandlePlayerDie);
	}

	private void OnDisable()
	{
		eventBroker.Unsubscribe<GameEvents.EarnScore>(HandleEarnScore);
		eventBroker.Unsubscribe<GameEvents.EarnScoreMultiplier>(HandleEarnScoreMultiplier);
		eventBroker.Unsubscribe<GameEvents.StartGame>(HandleStartGame);
		eventBroker.Unsubscribe<PlayerEvents.Die>(HandlePlayerDie);
	}
}
