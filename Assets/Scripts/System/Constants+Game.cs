
using System;
using Unity.VisualScripting;
using UnityEngine.Rendering;

public partial class Constants
{
    public enum Difficulty
    {
        Easy,
        Hard
    }

    public static class Game
	{
		public const string HighscorePP = "Highscore";

		public const string UpgradeAnimTrigger = "Upgrade";


	}

	public static class UI
	{
		public class UpgradePopups
		{
			public const string ClawSpeed = "ClawSpeed";
			public const string ClawLength = "ClawLength";
			public const string ClawDefense = "ClawDefense";
			public const string Hearts = "Hearts";
			public const string Invulnerability = "Invulnerability";
			public const string MoveSpeed = "MoveSpeed";
			public const string ScoreMultiplier = "ScoreMultiplier";
			public const string LevelUp = "LevelUp";
		}

		public const float HealthWarningFlashSpeed = 0.8f;
		public const float HealthWarningMinAlpha = 0.25f;
		public const float HealthWarningMaxAlpha = 0.75f;

		public class LeaderboardIcons
		{
			public const string Bunny = "Bunny";
			public const string Chicken = "Chicken";
			public const string Crab = "Crab";
			public const string Frog = "Frog";
			public const string Hippo = "Hippo";
			public const string Shark = "Shark";
			public const string Squid = "Squid";
		}

		public const string LeaderboardUsernamePP = "LeaderboardUsername";
		public const string LeaderboardDefaultUsername = "leaderboardusernamenotset10072024";
	}

	public static class Achievements
	{
		
		public static class LoverSeries
		{
			public static int CountThreshold = 30;
			public static string Key = "A_LS_";
			public static string GetKeyForEnemyType(Enemy.EnemyType enemyType, Difficulty difficulty)
			{
				return Key + enemyType.ToString() + "_" + difficulty.ToString();
			}
		}

        public static string GetKeyFromThreshold(string key, Difficulty difficulty)
        {
            return key + "_" + difficulty.ToString();
        }

        public static class ScoreSeries
		{
			public static (string, float) FirstThreshold = ("A_SS_1", 5000);
			public static (string, float) SecondThreshold = ("A_SS_2", 30000);
			public static (string, float) ThirdThreshold = ("A_SS_3", 100000);
		}

        public static class SurvivalSeries
        {
			// in seconds
            public static (string, float) FirstThreshold = ("A_SurS_1", 3 * 60);
            public static (string, float) SecondThreshold = ("A_SurS_2", 5 * 60);
            public static (string, float) ThirdThreshold = ("A_SurS_3", 10 * 60);
        }

		public static class RainbowSeries
		{
			public static (string, float) FirstThreshold = ("A_RS_1", 1);
			public static (string, float) SecondThreshold = ("A_RS_2", 5);
			public static (string, float) ThirdThreshold = ("A_RS_3", 10);
        }

		public static class UpgradeSeries
		{
			public static (string, float) ScoreMultiplier = ("A_US_SM", 2f);
			public static (string, float) ClawLength = ("A_US_CL", 6);
			public static (string, float) Invulnerability = ("A_US_I", 4f);
			public static (string, float) Health = ("A_US_H", 10f);
			public static (string, float) ClawSpeed = ("A_US_CS", 10f);
			public static (string, float) MoveSpeed = ("A_US_MS", 6f);
			public static (string, float) ClawDurability = ("A_US_CD", 3f);
		}
    }
}
