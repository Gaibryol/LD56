using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Dan.Main;
using Dan.Models;

public class UIManager : MonoBehaviour
{
	[SerializeField, Header("Main Menu UI")] private GameObject mainMenuPanel;
	[SerializeField] private Button mainMenuStartButton;
	[SerializeField] private Button mainMenuAchievementButton;
    [SerializeField] private Button mainMenuCreditsButton;
	[SerializeField] private Button mainMenuTutorialButton;
	[SerializeField] private Button mainMenuLeaderboardButton;

	[SerializeField, Header("Credits UI")] private GameObject creditsPanel;
	[SerializeField] private Button creditsMainMenuButton;

	[SerializeField, Header("Difficulty UI")] private GameObject difficultyPanel;
	[SerializeField] private Button difficultyNormalStartButton;
	[SerializeField] private Button difficultyHardStartButton;
	[SerializeField] private Sprite normalSprite;
	[SerializeField] private Sprite normalHoverSprite;
	[SerializeField] private Sprite hardSprite;
	[SerializeField] private Sprite hardHoverSprite;

	[SerializeField, Header("Gameplay UI")] private GameObject gameplayPanel;
	[SerializeField] private TMP_Text scoreText;
	[SerializeField] private TMP_Text scoreMultiplierText;
	[SerializeField] private TMP_Text heartsText;
	[SerializeField] private TMP_Text highscoreText;
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
	[SerializeField] private Animator upgradePopup;
	[SerializeField] private Animator levelUpPopup;
	[SerializeField] private Animator levelUpPopup2;
	[SerializeField] private GameObject pausePanel;
	[SerializeField] private Image healthWarning;

	[SerializeField, Header("Tutorial")] private GameObject tutorialPanel;
	[SerializeField] private Button tutorialMainMenuButton;

	[SerializeField, Header("End UI")] private GameObject endPanel;
	[SerializeField] private TMP_Text endFinalScore;
	[SerializeField] private TMP_Text endHighScore;
	[SerializeField] private TMP_Text endTimeSurvived;
	[SerializeField] private GameObject endNewHighScore;
    [SerializeField] private TMP_Text endNumBunny;
	[SerializeField] private TMP_Text endNumChicken;
	[SerializeField] private TMP_Text endNumCrab;
	[SerializeField] private TMP_Text endNumFrog;
	[SerializeField] private TMP_Text endNumHippo;
	[SerializeField] private TMP_Text endNumShark;
	[SerializeField] private TMP_Text endNumSquid;
	[SerializeField] private Button endRestartButton;
	[SerializeField] private Button endMainMenuButton;
	[SerializeField] private Button endLeaderboardButton;

	[SerializeField, Header("References")] private GameManager game;
	[SerializeField] private PlayerController player;
	[SerializeField] private Sprite bunnySprite;
	[SerializeField] private Sprite chickenSprite;
	[SerializeField] private Sprite crabSprite;
	[SerializeField] private Sprite frogSprite;
	[SerializeField] private Sprite hippoSprite;
	[SerializeField] private Sprite sharkSprite;
	[SerializeField] private Sprite squidSprite;

	[SerializeField, Header("Achievements")] private GameObject achievementPanel;
	[SerializeField] private Button achievementCloseButton;
	[SerializeField] private Button achievementHardButton;
	[SerializeField] private Button achievementEasyButton;
	[SerializeField] private GameObject achievementListContent;
	[SerializeField] private GameObject achievementListItemPrefab;
	[SerializeField] private Sprite normalButton;
	[SerializeField] private Sprite normalButtonActive;
    [SerializeField] private Sprite hardButton;
    [SerializeField] private Sprite hardButtonActive;

	[SerializeField, Header("Leaderboard")] private GameObject leaderboardPanel;
	[SerializeField] private Button leaderboardMainMenuButton;
	[SerializeField] private Button leaderboardGameButton;
	[SerializeField] private GameObject leaderboardContent;
	[SerializeField] private TournamentLeaderboardEntry tournamentLeaderboardEntry;
	[SerializeField] private GameObject leaderboardListObject;
	[SerializeField] private GameObject leaderboardLoadingObject;
	[SerializeField] private GameObject leaderboardErrorObject;

	[SerializeField, Header("Entry")] private GameObject entryPanel;
	[SerializeField] private TMP_InputField entryNameInput;
	[SerializeField] private Toggle entryBunnyToggle;
	[SerializeField] private Toggle entryChickenToggle;
	[SerializeField] private Toggle entryCrabToggle;
	[SerializeField] private Toggle entryFrogToggle;
	[SerializeField] private Toggle entryHippoToggle;
	[SerializeField] private Toggle entrySharkToggle;
	[SerializeField] private Toggle entrySquidToggle;
	[SerializeField] private Button entryConfirmButton;
	[SerializeField] private Button entryCancelButton;

	private string leaderboardUserIcon;
	private string leaderboardUsername;
	private int leaderboardUserScore;

	private Coroutine healthWarningCoroutine;
	private Coroutine upgradeCoroutine;

	private Constants.Difficulty lastDifficulty;

	private readonly EventBrokerComponent eventBroker = new EventBrokerComponent();

    // Start is called before the first frame update
    void Start()
    {
		float highscore = PlayerPrefs.GetFloat(Constants.Game.HighscorePP, 0f);
		highscoreText.text = highscore.ToString();

		mainMenuPanel.SetActive(true);
		creditsPanel.SetActive(false);
		tutorialPanel.SetActive(false);
		difficultyPanel.SetActive(false);
		gameplayPanel.SetActive(false);
		endPanel.SetActive(false);
		achievementPanel.SetActive(false);
		pausePanel.SetActive(false);
		leaderboardPanel.SetActive(false);
		entryPanel.SetActive(false);

		leaderboardUserIcon = Constants.UI.LeaderboardIcons.Bunny;
		leaderboardUsername = PlayerPrefs.GetString(Constants.UI.LeaderboardUsernamePP, Constants.UI.LeaderboardDefaultUsername);
		leaderboardUserScore = 0;
	}

	private void Update()
	{
		scoreText.text = game.score.ToString();
		scoreMultiplierText.text = "x" + game.scoreMultiplier.ToString();
		heartsText.text = "x" + player.health.ToString();

		if (player.health == 1 && healthWarningCoroutine == null)
		{
			healthWarningCoroutine = StartCoroutine(FlashHealthWarning());
		}

		for (int i = 0; i < player.passengers.Count; i++)
		{
			gameplaySlots[i].sprite = GetAnimalSprite(player.passengers[i]);
			gameplaySlots[i].color = new Color(gameplaySlots[i].color.r, gameplaySlots[i].color.g, gameplaySlots[i].color.b, gameplaySlots[i].sprite == null ? 0f : 1f);
		}
	}

	private void GetLeaderboard()
	{
		leaderboardLoadingObject.SetActive(true);
		leaderboardListObject.SetActive(false);
		leaderboardErrorObject.SetActive(false);
		leaderboardPanel.SetActive(true);

		Leaderboards.TinyGalaxiesLeaderboard.GetEntries(OnGotLeaderboardEntries, OnGotLeaderboardError);
	}

	private void OnGotLeaderboardError(string error)
	{
		leaderboardListObject.SetActive(false);
		leaderboardLoadingObject.SetActive(false);
		leaderboardErrorObject.SetActive(true);
	}

	private void OnGotLeaderboardEntries(Entry[] entries)
	{
		leaderboardListObject.SetActive(true);
		leaderboardLoadingObject.SetActive(false);
		leaderboardErrorObject.SetActive(false);

		// Clear leaderboard
		foreach (Transform child in leaderboardContent.transform)
		{
			Destroy(child.gameObject);
		}

		for (int i = 0; i < entries.Length; i++)
		{
			TournamentLeaderboardEntry entry = Instantiate(tournamentLeaderboardEntry);
			entry.Init(entries[i].Extra, entries[i].Rank, entries[i].Username, entries[i].Score);
			entry.transform.parent = leaderboardContent.transform;

			Debug.Log($"{entries[i].Rank}. {entries[i].Username} - {entries[i].Score}");
		}
	}

	private void SetLeaderboardEntry(string username, int score, string icon)
	{
		Leaderboards.TinyGalaxiesLeaderboard.UploadNewEntry(username, score, icon, OnSetLeaderboardEntry, OnSetEntryError);
	}

	private void OnSetEntryError(string error)
	{
		Debug.LogWarning("Error setting entry: " + error);
	}

	private void OnSetLeaderboardEntry(bool obj)
	{
		GetLeaderboard();
	}

	private IEnumerator FlashHealthWarning()
	{
		healthWarning.gameObject.SetActive(true);

		while (player.health == 1)
		{
			Color color = healthWarning.color;
			while (color.a < Constants.UI.HealthWarningMaxAlpha)
			{
				color.a += Time.deltaTime * Constants.UI.HealthWarningFlashSpeed;

				healthWarning.color = color;
				yield return null;
			}

			color.a = Constants.UI.HealthWarningMaxAlpha;
			healthWarning.color = color;

			while (color.a > Constants.UI.HealthWarningMinAlpha)
			{
				color.a -= Time.deltaTime * Constants.UI.HealthWarningFlashSpeed;

				healthWarning.color = color;
				yield return null;
			}

			color.a = Constants.UI.HealthWarningMinAlpha;
			healthWarning.color = color;

			yield return null;
		}

		Color endColor = healthWarning.color;
		while (endColor.a > 0)
		{
			endColor.a -= Time.deltaTime * Constants.UI.HealthWarningFlashSpeed;

			healthWarning.color = endColor;
			yield return null;
		}

		healthWarning.color = new Color(healthWarning.color.r, healthWarning.color.g, healthWarning.color.b, 0f);
		healthWarning.gameObject.SetActive(false);
		healthWarningCoroutine = null;
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

	private void OnMainMenuStartButton()
	{
		mainMenuPanel.SetActive(false);
		creditsPanel.SetActive(false);
		tutorialPanel.SetActive(false);
		difficultyPanel.SetActive(true);
		gameplayPanel.SetActive(false);
		endPanel.SetActive(false);
        achievementPanel.SetActive(false);
		pausePanel.SetActive(false);
		leaderboardPanel.SetActive(false);
	}

	private void OnDifficultyNormalStartButton()
	{
		float highscore = PlayerPrefs.GetFloat(Constants.Game.HighscorePP, 0f);
		highscoreText.text = highscore.ToString();

		mainMenuPanel.SetActive(false);
		creditsPanel.SetActive(false);
		tutorialPanel.SetActive(false);
		difficultyPanel.SetActive(false);
		gameplayPanel.SetActive(true);
		endPanel.SetActive(false);
		achievementPanel.SetActive(false);
		pausePanel.SetActive(false);
		leaderboardPanel.SetActive(false);

		lastDifficulty = Constants.Difficulty.Easy;

		eventBroker.Publish(this, new GameEvents.StartGame(Constants.Difficulty.Easy));
		eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.ButtonPress));
	}

	private void OnDifficultyHardStartButton()
	{
		float highscore = PlayerPrefs.GetFloat(Constants.Game.HighscorePP, 0f);
		highscoreText.text = highscore.ToString();

		mainMenuPanel.SetActive(false);
		creditsPanel.SetActive(false);
		tutorialPanel.SetActive(false);
		difficultyPanel.SetActive(false);
		gameplayPanel.SetActive(true);
		endPanel.SetActive(false);
		achievementPanel.SetActive(false);
		pausePanel.SetActive(false);
		leaderboardPanel.SetActive(false);

		lastDifficulty = Constants.Difficulty.Hard;

		eventBroker.Publish(this, new GameEvents.StartGame(Constants.Difficulty.Hard));
		eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.ButtonPress));
	}

	private void OnMainMenuButton()
	{
		mainMenuPanel.SetActive(true);
		creditsPanel.SetActive(false);
		tutorialPanel.SetActive(false);
		difficultyPanel.SetActive(false);
		gameplayPanel.SetActive(false);
		endPanel.SetActive(false);
		achievementPanel.SetActive(false);
		pausePanel.SetActive(false);
		leaderboardPanel.SetActive(false);

		eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.ButtonPress));
	}

	private void OnCreditsButton()
	{
		mainMenuPanel.SetActive(false);
		creditsPanel.SetActive(true);
		tutorialPanel.SetActive(false);
		difficultyPanel.SetActive(false);
		gameplayPanel.SetActive(false);
		endPanel.SetActive(false);
		achievementPanel.SetActive(false);
		pausePanel.SetActive(false);
		leaderboardPanel.SetActive(false);

		eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.ButtonPress));
	}

	private void OnAchievementsButton()
	{
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(false);
		tutorialPanel.SetActive(false);
		difficultyPanel.SetActive(false);
        gameplayPanel.SetActive(false);
        endPanel.SetActive(false);
        achievementPanel.SetActive(true);
		leaderboardPanel.SetActive(false);

		eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.ButtonPress));
    }

	private void OnAchievementsEasyButtion()
	{
		achievementEasyButton.GetComponent<Image>().sprite = normalButtonActive;
		achievementHardButton.GetComponent<Image>().sprite = hardButton;
        eventBroker.Publish(this, new GameEvents.NotifyAchievementButtonPressed(Constants.Difficulty.Easy));
        eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.ButtonPress));
    }

    private void OnAchievementsHardButtion()
    {
        achievementEasyButton.GetComponent<Image>().sprite = normalButton;
        achievementHardButton.GetComponent<Image>().sprite = hardButtonActive;
        eventBroker.Publish(this, new GameEvents.NotifyAchievementButtonPressed(Constants.Difficulty.Hard));
        eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.ButtonPress));
    }

    private void OnRestartButton()
	{
		mainMenuPanel.SetActive(false);
		creditsPanel.SetActive(false);
		tutorialPanel.SetActive(false);
		difficultyPanel.SetActive(false);
		gameplayPanel.SetActive(true);
		endPanel.SetActive(false);
        achievementPanel.SetActive(false);
		pausePanel.SetActive(false);
		leaderboardPanel.SetActive(false);

		Color offColor = upgradePopup.GetComponent<Image>().color;
		offColor.a = 0f;
		upgradePopup.GetComponent<Image>().color = offColor;
		levelUpPopup.GetComponent<Image>().color = offColor;
		levelUpPopup2.GetComponent<Image>().color = offColor;

		eventBroker.Publish(this, new GameEvents.StartGame(lastDifficulty));
		eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.ButtonPress));
	}

	private void OnTutorialButton()
	{
		mainMenuPanel.SetActive(false);
		creditsPanel.SetActive(false);
		tutorialPanel.SetActive(true);
		difficultyPanel.SetActive(false);
		gameplayPanel.SetActive(false);
		endPanel.SetActive(false);
		achievementPanel.SetActive(false);
		pausePanel.SetActive(false);
		leaderboardPanel.SetActive(false);

		eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.ButtonPress));
	}

	private void OnLeaderboardButtonFromGame()
	{
		GetLeaderboard();

		leaderboardMainMenuButton.gameObject.SetActive(false);
		leaderboardGameButton.gameObject.SetActive(true);
	}

	private void OnLeaderboardButtonFromMainMenu()
	{
		GetLeaderboard();

		leaderboardMainMenuButton.gameObject.SetActive(true);
		leaderboardGameButton.gameObject.SetActive(false);
	}

	private void OnLeaderboardMainMenuButton()
	{
		OnMainMenuButton();
	}

	private void OnLeaderboardGameButton()
	{
		endPanel.SetActive(true);
		leaderboardPanel.SetActive(false);
		eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.ButtonPress));
	}

	private void OnSFXButton()
	{
		eventBroker.Publish(this, new AudioEvents.ToggleSFX((newState) => 
		{
			gameplaySFXButton.GetComponent<Image>().sprite = newState ? sfxOff : sfxOn;
		}));

		eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.ButtonPress));
	}

	private void OnMusicButton()
	{
		eventBroker.Publish(this, new AudioEvents.ToggleMusic((newState) => 
		{
			gameplayMusicButton.GetComponent<Image>().sprite = newState ? musicOff : musicOn;
		}));

		eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.ButtonPress));
	}

	private void OnPauseButton()
	{
		if (Time.timeScale == 1f)
		{
			gameplayPauseButton.GetComponent<Image>().sprite = pauseOff;
			pausePanel.SetActive(true);
			Time.timeScale = 0f;
			
		}
		else
		{
			gameplayPauseButton.GetComponent<Image>().sprite = pauseOn;
			pausePanel.SetActive(false);
			Time.timeScale = 1f;
		}

		eventBroker.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.ButtonPress));
	}

	private void OnBunnyToggle(bool isOn)
	{
		if (isOn)
		{
			leaderboardUserIcon = Constants.UI.LeaderboardIcons.Bunny;
		}
	}

	private void OnChickenToggle(bool isOn)
	{
		if (isOn)
		{
			leaderboardUserIcon = Constants.UI.LeaderboardIcons.Chicken;
		}
	}

	private void OnCrabToggle(bool isOn)
	{
		if (isOn)
		{
			leaderboardUserIcon = Constants.UI.LeaderboardIcons.Crab;
		}
	}

	private void OnFrogToggle(bool isOn)
	{
		if (isOn)
		{
			leaderboardUserIcon = Constants.UI.LeaderboardIcons.Frog;
		}
	}

	private void OnHippoToggle(bool isOn)
	{
		if (isOn)
		{
			leaderboardUserIcon = Constants.UI.LeaderboardIcons.Hippo;
		}
	}

	private void OnSharkToggle(bool isOn)
	{
		if (isOn)
		{
			leaderboardUserIcon = Constants.UI.LeaderboardIcons.Shark;
		}
	}

	private void OnSquidToggle(bool isOn)
	{
		if (isOn)
		{
			leaderboardUserIcon = Constants.UI.LeaderboardIcons.Squid;
		}
	}

	private void OnEntryCancel()
	{
		entryPanel.SetActive(false);
	}

	private void OnEntryConfirm()
	{
		LeaderboardCreator.Ping((isOnline) => 
		{
			if (isOnline)
			{
				leaderboardUsername = entryNameInput.text;
				SetLeaderboardEntry(leaderboardUsername, leaderboardUserScore, leaderboardUserIcon);
			}
			else
			{
				Debug.LogWarning("Leaderboard could not be reached");
			}
		});
		
		entryPanel.SetActive(false);
	}

	private void HandlePlayerUpgrade(BrokerEvent<PlayerEvents.Upgrade> inEvent)
	{
		List<int> indexes = inEvent.Payload.Indexes;
		for (int i = 0; i < indexes.Count; i++)
		{
			gameplayAnims[indexes[i]].SetTrigger(Constants.Game.UpgradeAnimTrigger);
		}

		if (upgradeCoroutine != null)
		{
			StopCoroutine(upgradeCoroutine);
			upgradeCoroutine = null;
		}

		upgradeCoroutine = StartCoroutine(UpgradePopup(inEvent.Payload.Type));
	}

	private IEnumerator UpgradePopup(Constants.Enemy.EnemyType enemyType)
	{
		Color onColor = upgradePopup.GetComponent<Image>().color;
		onColor.a = 1f;
		upgradePopup.GetComponent<Image>().color = onColor;
		levelUpPopup.GetComponent<Image>().color = onColor;
		levelUpPopup2.GetComponent<Image>().color = onColor;

		switch (enemyType)
		{
			case Constants.Enemy.EnemyType.Bunny:
				upgradePopup.SetTrigger(Constants.UI.UpgradePopups.ClawLength);
				break;

			case Constants.Enemy.EnemyType.Chicken:
				upgradePopup.SetTrigger(Constants.UI.UpgradePopups.ScoreMultiplier);
				break;

			case Constants.Enemy.EnemyType.Crab:
				upgradePopup.SetTrigger(Constants.UI.UpgradePopups.ClawDefense);
				break;

			case Constants.Enemy.EnemyType.Frog:
				upgradePopup.SetTrigger(Constants.UI.UpgradePopups.ClawSpeed);
				break;

			case Constants.Enemy.EnemyType.Hippo:
				upgradePopup.SetTrigger(Constants.UI.UpgradePopups.Hearts);
				break;

			case Constants.Enemy.EnemyType.Shark:
				upgradePopup.SetTrigger(Constants.UI.UpgradePopups.MoveSpeed);
				break;

			case Constants.Enemy.EnemyType.Squid:
				upgradePopup.SetTrigger(Constants.UI.UpgradePopups.Invulnerability);
				break;
		}

		levelUpPopup.SetTrigger(Constants.UI.UpgradePopups.LevelUp);
		levelUpPopup2.SetTrigger(Constants.UI.UpgradePopups.LevelUp);

		yield return new WaitForSeconds(0.25f);

		Color offColor = upgradePopup.GetComponent<Image>().color;
		offColor.a = 0f;
		upgradePopup.GetComponent<Image>().color = offColor;
		levelUpPopup.GetComponent<Image>().color = offColor;
		levelUpPopup2.GetComponent<Image>().color = offColor;
	}

	private void HandleEndGame(BrokerEvent<GameEvents.EndGame> inEvent)
	{
		StartCoroutine(EndGameDelay(inEvent));
    }

    private IEnumerator EndGameDelay(BrokerEvent<GameEvents.EndGame> inEvent)
	{
		yield return new WaitForSeconds(.1f);
        endFinalScore.text = inEvent.Payload.FinalScore.ToString();
        endHighScore.text = PlayerPrefs.GetFloat(Constants.Game.HighscorePP, 0f).ToString();

        TimeSpan time = TimeSpan.FromSeconds(inEvent.Payload.TotalGameTime);
        endTimeSurvived.text = time.ToString(@"mm\:ss");

        if (inEvent.Payload.IsHighscore)
        {
            highscoreText.text = inEvent.Payload.FinalScore.ToString();

			LeaderboardCreator.Ping((isOnline) => 
			{ 
				if (isOnline)
				{
					if (leaderboardUsername == Constants.UI.LeaderboardDefaultUsername)
					{
						leaderboardUserScore = (int)inEvent.Payload.FinalScore;
						entryPanel.SetActive(true);
					}
					else
					{
						SetLeaderboardEntry(leaderboardUsername, (int)inEvent.Payload.FinalScore, leaderboardUserIcon);
					}
				}
			});
        }

        endPanel.SetActive(true);
        endNewHighScore.SetActive(inEvent.Payload.IsHighscore);
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

	private void HandleRainbowAttack(BrokerEvent<PlayerEvents.RainbowAttack> inEvent)
	{
		for (int i = 0; i < gameplayAnims.Count; i++)
		{
			gameplayAnims[i].SetTrigger(Constants.Game.UpgradeAnimTrigger);
		}
	}

	private void OnEnable()
	{
		eventBroker.Subscribe<GameEvents.EndGame>(HandleEndGame);
		eventBroker.Subscribe<PlayerEvents.Die>(HandlePlayerDie);
		eventBroker.Subscribe<PlayerEvents.Upgrade>(HandlePlayerUpgrade);
		eventBroker.Subscribe<PlayerEvents.RainbowAttack>(HandleRainbowAttack);

		mainMenuStartButton.onClick.AddListener(OnMainMenuStartButton);
        mainMenuAchievementButton.onClick.AddListener(OnAchievementsButton);
        mainMenuCreditsButton.onClick.AddListener(OnCreditsButton);
		mainMenuTutorialButton.onClick.AddListener(OnTutorialButton);
		mainMenuLeaderboardButton.onClick.AddListener(OnLeaderboardButtonFromMainMenu);
		creditsMainMenuButton.onClick.AddListener(OnMainMenuButton);
		tutorialMainMenuButton.onClick.AddListener(OnMainMenuButton);
		achievementCloseButton.onClick.AddListener(OnMainMenuButton);
		endMainMenuButton.onClick.AddListener(OnMainMenuButton);
		endRestartButton.onClick.AddListener(OnRestartButton);
		endLeaderboardButton.onClick.AddListener(OnLeaderboardButtonFromGame);
		leaderboardMainMenuButton.onClick.AddListener(OnLeaderboardMainMenuButton);
		leaderboardGameButton.onClick.AddListener(OnLeaderboardGameButton);

		entryBunnyToggle.onValueChanged.AddListener(OnBunnyToggle);
		entryChickenToggle.onValueChanged.AddListener(OnChickenToggle);
		entryCrabToggle.onValueChanged.AddListener(OnCrabToggle);
		entryFrogToggle.onValueChanged.AddListener(OnFrogToggle);
		entryHippoToggle.onValueChanged.AddListener(OnHippoToggle);
		entrySharkToggle.onValueChanged.AddListener(OnSharkToggle);
		entrySquidToggle.onValueChanged.AddListener(OnSquidToggle);
		entryConfirmButton.onClick.AddListener(OnEntryConfirm);
		entryCancelButton.onClick.AddListener(OnEntryCancel);

		difficultyNormalStartButton.onClick.AddListener(OnDifficultyNormalStartButton);
		difficultyHardStartButton.onClick.AddListener(OnDifficultyHardStartButton);

		gameplayPauseButton.onClick.AddListener(OnPauseButton);
		gameplayMusicButton.onClick.AddListener(OnMusicButton);
		gameplaySFXButton.onClick.AddListener(OnSFXButton);

		achievementEasyButton.onClick.AddListener(OnAchievementsEasyButtion);
		achievementHardButton.onClick.AddListener(OnAchievementsHardButtion);

		gameplayMusicButton.GetComponent<Image>().sprite = PlayerPrefs.GetInt(Constants.Audio.MusicMutedPP, 0) == 1 ? musicOff : musicOn;
		gameplaySFXButton.GetComponent<Image>().sprite = PlayerPrefs.GetInt(Constants.Audio.SFXMutedPP, 0) == 1 ? sfxOff : sfxOn;
	}

	private void OnDisable()
	{
		eventBroker.Unsubscribe<GameEvents.EndGame>(HandleEndGame);
		eventBroker.Unsubscribe<PlayerEvents.Die>(HandlePlayerDie);
		eventBroker.Unsubscribe<PlayerEvents.Upgrade>(HandlePlayerUpgrade);
		eventBroker.Unsubscribe<PlayerEvents.RainbowAttack>(HandleRainbowAttack);

		mainMenuStartButton.onClick.RemoveListener(OnMainMenuStartButton);
        mainMenuAchievementButton.onClick.RemoveListener(OnAchievementsButton);
        mainMenuCreditsButton.onClick.RemoveListener(OnCreditsButton);
		mainMenuTutorialButton.onClick.RemoveListener(OnTutorialButton);
		mainMenuLeaderboardButton.onClick.RemoveListener(OnLeaderboardButtonFromMainMenu);
		creditsMainMenuButton.onClick.RemoveListener(OnMainMenuButton);
		tutorialMainMenuButton.onClick.RemoveListener(OnMainMenuButton);
		achievementCloseButton.onClick.RemoveListener(OnMainMenuButton);
        endMainMenuButton.onClick.RemoveListener(OnMainMenuButton);
		endRestartButton.onClick.RemoveListener(OnRestartButton);
		endLeaderboardButton.onClick.RemoveListener(OnLeaderboardButtonFromGame);
		leaderboardMainMenuButton.onClick.RemoveListener(OnLeaderboardMainMenuButton);
		leaderboardGameButton.onClick.RemoveListener(OnLeaderboardGameButton);

		entryBunnyToggle.onValueChanged.RemoveListener(OnBunnyToggle);
		entryChickenToggle.onValueChanged.RemoveListener(OnChickenToggle);
		entryCrabToggle.onValueChanged.RemoveListener(OnCrabToggle);
		entryFrogToggle.onValueChanged.RemoveListener(OnFrogToggle);
		entryHippoToggle.onValueChanged.RemoveListener(OnHippoToggle);
		entrySharkToggle.onValueChanged.RemoveListener(OnSharkToggle);
		entrySquidToggle.onValueChanged.RemoveListener(OnSquidToggle);
		entryConfirmButton.onClick.RemoveListener(OnEntryConfirm);
		entryCancelButton.onClick.RemoveListener(OnEntryCancel);

		difficultyNormalStartButton.onClick.RemoveListener(OnDifficultyNormalStartButton);
		difficultyHardStartButton.onClick.RemoveListener(OnDifficultyHardStartButton);

		gameplayPauseButton.onClick.RemoveListener(OnPauseButton);
		gameplayMusicButton.onClick.RemoveListener(OnMusicButton);
		gameplaySFXButton.onClick.RemoveListener(OnSFXButton);

        achievementEasyButton.onClick.RemoveListener(OnAchievementsEasyButtion);
        achievementHardButton.onClick.RemoveListener(OnAchievementsHardButtion);
    }
}
