using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private int score;

	private bool isPlaying;

	private Coroutine scoreCoroutine;

	private readonly EventBrokerComponent eventBroker = new EventBrokerComponent();

    // Start is called before the first frame update
    void Start()
    {
		isPlaying = false;
		score = 0;

		eventBroker.Publish(this, new GameEvents.StartGame());
    }

	private IEnumerator IncrementScore()
	{
		while (isPlaying)
		{
			score += 1;
			yield return new WaitForSeconds(1f);
		}
	}

	private void HandleStartGame(BrokerEvent<GameEvents.StartGame> inEvent)
	{
		score = 0;
		isPlaying = true;
		scoreCoroutine = StartCoroutine(IncrementScore());
	}

	private void HandleEarnScore(BrokerEvent<GameEvents.EarnScore> inEvent)
	{
		score += inEvent.Payload.Amount;
	}

	private void HandlePlayerDie(BrokerEvent<PlayerEvents.Die> inEvent)
	{
		StopCoroutine(scoreCoroutine);
		isPlaying = false;

		eventBroker.Publish(this, new GameEvents.EndGame(score));
		Debug.Log("Final Score: " + score);
	}

	private void OnEnable()
	{
		eventBroker.Subscribe<GameEvents.EarnScore>(HandleEarnScore);
		eventBroker.Subscribe<GameEvents.StartGame>(HandleStartGame);
		eventBroker.Subscribe<PlayerEvents.Die>(HandlePlayerDie);
	}

	private void OnDisable()
	{
		eventBroker.Unsubscribe<GameEvents.EarnScore>(HandleEarnScore);
		eventBroker.Unsubscribe<GameEvents.StartGame>(HandleStartGame);
		eventBroker.Unsubscribe<PlayerEvents.Die>(HandlePlayerDie);
	}
}
