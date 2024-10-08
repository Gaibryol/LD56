using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TournamentLeaderboardEntry : MonoBehaviour
{
	[SerializeField] private Image bannerImage;
	[SerializeField] private Image iconImage;
	[SerializeField] private TMP_Text rankText;
	[SerializeField] private TMP_Text nameText;
	[SerializeField] private TMP_Text scoreText;

	[SerializeField, Header("Sprites")] private Sprite bunnySprite;
	[SerializeField] private Sprite chickenSprite;
	[SerializeField] private Sprite crabSprite;
	[SerializeField] private Sprite frogSprite;
	[SerializeField] private Sprite hippoSprite;
	[SerializeField] private Sprite sharkSprite;
	[SerializeField] private Sprite squidSprite;
	[SerializeField] private Sprite top3Banner;
	[SerializeField] private Sprite regularBanner;

	public void Init(string icon, int rank, string name, int score)
	{
		if (rank <= 3)
		{
			bannerImage.sprite = top3Banner;
		}
		else
		{
			bannerImage.sprite = regularBanner;
		}

		rankText.text = "Rank " + rank.ToString();
		nameText.text = name;
		scoreText.text = score.ToString();

		switch (icon)
		{
			case Constants.UI.LeaderboardIcons.Bunny:
				iconImage.sprite = bunnySprite;
				break;

			case Constants.UI.LeaderboardIcons.Chicken:
				iconImage.sprite = chickenSprite;
				break;

			case Constants.UI.LeaderboardIcons.Crab:
				iconImage.sprite = crabSprite;
				break;

			case Constants.UI.LeaderboardIcons.Frog:
				iconImage.sprite = frogSprite;
				break;

			case Constants.UI.LeaderboardIcons.Hippo:
				iconImage.sprite = hippoSprite;
				break;

			case Constants.UI.LeaderboardIcons.Shark:
				iconImage.sprite = sharkSprite;
				break;

			case Constants.UI.LeaderboardIcons.Squid:
				iconImage.sprite = squidSprite;
				break;
		}
	}
}
