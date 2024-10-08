using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private Texture2D cursor;

	public float score;
	public float scoreMultiplier;

	private bool isPlaying;

	private Coroutine scoreCoroutine;

	private float startTime;

	private Constants.Difficulty currentDifficulty;

	private readonly EventBrokerComponent eventBroker = new EventBrokerComponent();

	private void Awake()
	{
		Cursor.SetCursor(cursor, new Vector2(cursor.width / 2f, cursor.height / 2f), CursorMode.Auto);
	}

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
		
		if (inEvent.Payload.Difficulty == Constants.Difficulty.Easy)
		{
			scoreMultiplier = Constants.Player.BaseScoreMultiplier;
		}
		else if (inEvent.Payload.Difficulty == Constants.Difficulty.Hard)
		{
			scoreMultiplier = Constants.Player.BaseScoreMultiplierHard;
		}

		currentDifficulty = inEvent.Payload.Difficulty;
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

			eventBroker.Publish(this, new GameEvents.EndGame(score, true, Time.time - startTime, currentDifficulty));
		}
		else
		{
			eventBroker.Publish(this, new GameEvents.EndGame(score, false, Time.time - startTime, currentDifficulty));
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
