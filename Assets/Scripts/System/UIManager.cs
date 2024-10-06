using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
	[SerializeField, Header("Main Menu UI")] private GameObject mainMenuPanel;
	[SerializeField] private Button mainMenuStartButton;
	[SerializeField] private Button mainMenuCreditsButton;

	[SerializeField, Header("Credits UI")] private GameObject creditsPanel;
	[SerializeField] private Button creditsMainMenuButton;

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
	[SerializeField] private Button endRestartButton;
	[SerializeField] private Button endMainMenuButton;

	[SerializeField, Header("References")] private GameManager game;
	[SerializeField] private PlayerController player;

	private readonly EventBrokerComponent eventBroker = new EventBrokerComponent();

    // Start is called before the first frame update
    void Start()
    {
		InitHealthbar();

		mainMenuPanel.SetActive(true);
		creditsPanel.SetActive(false);
		gameplayPanel.SetActive(false);
		endPanel.SetActive(false);
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

	private void OnStartButton()
	{
		mainMenuPanel.SetActive(false);
		creditsPanel.SetActive(false);
		gameplayPanel.SetActive(true);
		endPanel.SetActive(false);

		eventBroker.Publish(this, new GameEvents.StartGame());
		eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.ButtonPress));
	}

	private void OnMainMenuButton()
	{
		mainMenuPanel.SetActive(true);
		creditsPanel.SetActive(false);
		gameplayPanel.SetActive(false);
		endPanel.SetActive(false);

		eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.ButtonPress));
	}

	private void OnCreditsButton()
	{
		mainMenuPanel.SetActive(false);
		creditsPanel.SetActive(true);
		gameplayPanel.SetActive(false);
		endPanel.SetActive(false);

		eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.ButtonPress));
	}

	private void OnRestartButton()
	{
		mainMenuPanel.SetActive(false);
		creditsPanel.SetActive(false);
		gameplayPanel.SetActive(true);
		endPanel.SetActive(false);

		eventBroker.Publish(this, new GameEvents.StartGame());
		eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.ButtonPress));
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
		eventBroker.Subscribe<GameEvents.EndGame>(HandleEndGame);
		eventBroker.Subscribe<PlayerEvents.Die>(HandlePlayerDie);

		mainMenuStartButton.onClick.AddListener(OnStartButton);
		mainMenuCreditsButton.onClick.AddListener(OnCreditsButton);
		creditsMainMenuButton.onClick.AddListener(OnMainMenuButton);
		endMainMenuButton.onClick.AddListener(OnMainMenuButton);
		endRestartButton.onClick.AddListener(OnRestartButton);
	}

	private void OnDisable()
	{
		eventBroker.Unsubscribe<GameEvents.EndGame>(HandleEndGame);
		eventBroker.Unsubscribe<PlayerEvents.Die>(HandlePlayerDie);

		mainMenuStartButton.onClick.RemoveListener(OnStartButton);
		mainMenuCreditsButton.onClick.RemoveListener(OnCreditsButton);
		creditsMainMenuButton.onClick.RemoveListener(OnMainMenuButton);
		endMainMenuButton.onClick.RemoveListener(OnMainMenuButton);
		endRestartButton.onClick.RemoveListener(OnRestartButton);
	}
}
