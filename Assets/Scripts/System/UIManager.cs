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
	[SerializeField] private TMP_Text heartsText;
	[SerializeField] private Button gameplayPauseButton;
	[SerializeField] private Button gameplayMusicButton;
	[SerializeField] private Button gameplaySFXButton;
	[SerializeField] private Sprite pauseOn;
	[SerializeField] private Sprite pauseOff;
	[SerializeField] private Sprite musicOn;
	[SerializeField] private Sprite musicOff;
	[SerializeField] private Sprite sfxOn;
	[SerializeField] private Sprite sfxOff;
	[SerializeField] private List<Image> gameplaySlots;
	[SerializeField] private List<Animator> gameplayAnims;

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
	[SerializeField] private Sprite bunnySprite;
	[SerializeField] private Sprite chickenSprite;
	[SerializeField] private Sprite crabSprite;
	[SerializeField] private Sprite frogSprite;
	[SerializeField] private Sprite hippoSprite;
	[SerializeField] private Sprite sharkSprite;
	[SerializeField] private Sprite squidSprite;

	private readonly EventBrokerComponent eventBroker = new EventBrokerComponent();

    // Start is called before the first frame update
    void Start()
    {
		mainMenuPanel.SetActive(true);
		creditsPanel.SetActive(false);
		gameplayPanel.SetActive(false);
		endPanel.SetActive(false);
	}

	private void Update()
	{
		scoreText.text = game.score.ToString();
		scoreMultiplierText.text = "x" + game.scoreMultiplier.ToString();
		heartsText.text = "x" + player.health.ToString();

		for (int i = 0; i < player.passengers.Count; i++)
		{
			gameplaySlots[i].sprite = GetAnimalSprite(player.passengers[i]);
			gameplaySlots[i].color = new Color(gameplaySlots[i].color.r, gameplaySlots[i].color.g, gameplaySlots[i].color.b, gameplaySlots[i].sprite == null ? 0f : 1f);
		}
	}

	private Sprite GetAnimalSprite(Constants.Enemy.EnemyType enemyType)
	{
		return enemyType switch
		{
			Constants.Enemy.EnemyType.Bunny => bunnySprite,
			Constants.Enemy.EnemyType.Chicken => chickenSprite,
			Constants.Enemy.EnemyType.Crab => crabSprite,
			Constants.Enemy.EnemyType.Frog => frogSprite,
			Constants.Enemy.EnemyType.Hippo => hippoSprite,
			Constants.Enemy.EnemyType.Shark => sharkSprite,
			Constants.Enemy.EnemyType.Squid => squidSprite,
			Constants.Enemy.EnemyType.None => null,
			_ => null
		};
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

	private void OnSFXButton()
	{
		eventBroker.Publish(this, new AudioEvents.ToggleSFX((newState) => 
		{
			gameplaySFXButton.GetComponent<Image>().sprite = newState ? sfxOn : sfxOff;
		}));
	}

	private void OnMusicButton()
	{
		eventBroker.Publish(this, new AudioEvents.ToggleMusic((newState) => 
		{
			gameplayMusicButton.GetComponent<Image>().sprite = newState ? musicOn : musicOff;
		}));
	}

	private void OnPauseButton()
	{
		if (Time.timeScale == 1f)
		{
			gameplayPauseButton.GetComponent<Image>().sprite = pauseOff;
			Time.timeScale = 0f;
			
		}
		else
		{
			gameplayPauseButton.GetComponent<Image>().sprite = pauseOn;
			Time.timeScale = 1f;
		}
	}

	private void HandlePlayerUpgrade(BrokerEvent<PlayerEvents.Upgrade> inEvent)
	{
		List<int> indexes = inEvent.Payload.Indexes;
		for (int i = 0; i < indexes.Count; i++)
		{
			gameplayAnims[i].SetTrigger(Constants.Game.UpgradeAnimTrigger);
		}
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
		eventBroker.Subscribe<PlayerEvents.Upgrade>(HandlePlayerUpgrade);

		mainMenuStartButton.onClick.AddListener(OnStartButton);
		mainMenuCreditsButton.onClick.AddListener(OnCreditsButton);
		creditsMainMenuButton.onClick.AddListener(OnMainMenuButton);
		endMainMenuButton.onClick.AddListener(OnMainMenuButton);
		endRestartButton.onClick.AddListener(OnRestartButton);

		gameplayPauseButton.onClick.AddListener(OnPauseButton);
		gameplayMusicButton.onClick.AddListener(OnMusicButton);
		gameplaySFXButton.onClick.AddListener(OnSFXButton);

		gameplayMusicButton.GetComponent<Image>().sprite = PlayerPrefs.GetFloat(Constants.Audio.MusicVolumePP, Constants.Audio.DefaultMusicVolume) != 0 ? musicOn : musicOff;
		gameplaySFXButton.GetComponent<Image>().sprite = PlayerPrefs.GetFloat(Constants.Audio.SFXVolumePP, Constants.Audio.DefaultSFXVolume) != 0 ? sfxOn : sfxOff;
	}

	private void OnDisable()
	{
		eventBroker.Unsubscribe<GameEvents.EndGame>(HandleEndGame);
		eventBroker.Unsubscribe<PlayerEvents.Die>(HandlePlayerDie);
		eventBroker.Unsubscribe<PlayerEvents.Upgrade>(HandlePlayerUpgrade);

		mainMenuStartButton.onClick.RemoveListener(OnStartButton);
		mainMenuCreditsButton.onClick.RemoveListener(OnCreditsButton);
		creditsMainMenuButton.onClick.RemoveListener(OnMainMenuButton);
		endMainMenuButton.onClick.RemoveListener(OnMainMenuButton);
		endRestartButton.onClick.RemoveListener(OnRestartButton);

		gameplayPauseButton.onClick.RemoveListener(OnPauseButton);
		gameplayMusicButton.onClick.RemoveListener(OnMusicButton);
		gameplaySFXButton.onClick.RemoveListener(OnSFXButton);
	}
}
