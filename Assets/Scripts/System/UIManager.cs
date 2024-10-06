using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
	[SerializeField, Header("Gameplay UI")] private GameObject gameplayPanel;
	[SerializeField] private TMP_Text scoreText;
	[SerializeField] private TMP_Text scoreMultiplierText;
	[SerializeField] private List<Image> hearts;
	[SerializeField] private Sprite heartOn;
	[SerializeField] private Sprite heartOff;

	[SerializeField, Header("End UI")] private GameObject endPanel;
	[SerializeField] private TMP_Text endFinalScore;
	[SerializeField] private TMP_Text endNumBunny;
	[SerializeField] private TMP_Text endNumChicken;
	[SerializeField] private TMP_Text endNumCrab;
	[SerializeField] private TMP_Text endNumFrog;
	[SerializeField] private TMP_Text endNumHippo;
	[SerializeField] private TMP_Text endNumShark;
	[SerializeField] private TMP_Text endNumSquid;
	[SerializeField] private Button restartButton;

	[SerializeField, Header("References")] private GameManager game;
	[SerializeField] private PlayerController player;

	private readonly EventBrokerComponent eventBroker = new EventBrokerComponent();

    // Start is called before the first frame update
    void Start()
    {
		InitHealthbar();
	}

	private void InitHealthbar()
	{
		int i = 0;
		for (; i < Constants.Player.BaseHealth; i++)
		{
			hearts[i].sprite = heartOn;
		}

		for (; i < hearts.Count; i++)
		{
			hearts[i].sprite = heartOff;
		}
	}

	private void HandleHealthbar()
	{
		int i = 0;
		for (; i < player.health; i++)
		{
			hearts[i].sprite = heartOn;
		}

		for (; i < hearts.Count; i++)
		{
			hearts[i].sprite = heartOff;
		}
	}

	private void Update()
	{
		scoreText.text = game.score.ToString();
		scoreMultiplierText.text = game.scoreMultiplier.ToString();
		HandleHealthbar();
	}

	private void OnRestartButton()
	{
		eventBroker.Publish(this, new GameEvents.StartGame());
		eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.ButtonPress));
	}

	private void HandleStartGame(BrokerEvent<GameEvents.StartGame> inEvent)
	{
		InitHealthbar();

		endPanel.SetActive(false);
		gameplayPanel.SetActive(true);
	}

	private void HandleEndGame(BrokerEvent<GameEvents.EndGame> inEvent)
	{
		endFinalScore.text = inEvent.Payload.FinalScore.ToString();

		endPanel.SetActive(true);
		gameplayPanel.SetActive(false);
	}

	private void HandlePlayerDie(BrokerEvent<PlayerEvents.Die> inEvent)
	{
		endNumBunny.text = "x " + inEvent.Payload.NumBunny.ToString();
		endNumChicken.text = "x " + inEvent.Payload.NumChicken.ToString();
		endNumCrab.text = "x " +  inEvent.Payload.NumCrab.ToString();
		endNumFrog.text = "x " + inEvent.Payload.NumFrog.ToString();
		endNumHippo.text = "x " + inEvent.Payload.NumHippo.ToString();
		endNumShark.text = "x " + inEvent.Payload.NumShark.ToString();
		endNumSquid.text = "x " + inEvent.Payload.NumSquid.ToString();
	}

	private void OnEnable()
	{
		eventBroker.Subscribe<GameEvents.StartGame>(HandleStartGame);
		eventBroker.Subscribe<GameEvents.EndGame>(HandleEndGame);
		eventBroker.Subscribe<PlayerEvents.Die>(HandlePlayerDie);

		restartButton.onClick.AddListener(OnRestartButton);
	}

	private void OnDisable()
	{
		eventBroker.Unsubscribe<GameEvents.StartGame>(HandleStartGame);
		eventBroker.Unsubscribe<GameEvents.EndGame>(HandleEndGame);
		eventBroker.Unsubscribe<PlayerEvents.Die>(HandlePlayerDie);

		restartButton.onClick.RemoveListener(OnRestartButton);
	}
}
