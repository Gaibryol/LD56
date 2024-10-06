
public partial class Constants
{
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
	}

	public static class Achievements
	{
		public enum Difficulty
		{
			Easy,
			Hard
		}

		public static class LoverSeries
		{
			public static int CountThreshold = 30;
			public static string Key = "A_LS_";
			public static string GetKeyForEnemyType(Enemy.EnemyType enemyType)
			{
				return Key + enemyType.ToString();
			}
		}

		public static class SurvivalSeries
		{
			public static int FirstThreshold = 5000;
			public static int SecondThreshold = 10000;
			public static int ThirdThreshold = 20000;

			public static string ThresholdKey = "A_SS_";
			public static string GetKeyFromThreshold(int threshold)
			{
				return ThresholdKey + threshold.ToString();
			}
		}
	}
}
