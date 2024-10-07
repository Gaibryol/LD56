
using System.Collections.Generic;
using UnityEngine;

public partial class Constants
{
	public static class Player
	{
		public const int BaseHealth = 5;
		public const float BaseMoveSpeed = 3f;
		public const float BaseInvulnerabilityDuration = 1.5f;
		public const float BaseScoreMultiplier = 1f;
		public const float BaseScoreMultiplierHard = 2f;

		public const float InvulnerableTick = 0.2f;
		public const float InvulnerableAlpha = 0.25f;

		public const int HealthIncrement = 1;
		public const float MoveSpeedIncrement = 1f;
		public const float InvulnerabilityDurationIncrement = 1f;
		public const float ScoreMultiplierIncrement = 0.25f;

		public const int NumToUpgrade = 3;

		public const float ScoreEarnedOnGrab = 500f;
		public const float ScoreEarnedPerSecond = 100f;

		public const float ScreenShakeAmount = 0.02f;
		public const float ScreenShakeDuration = 0.5f;
	}

	public static class Claw
	{
		public const int BaseNumBulletBlocks = 0;
		public const float BaseClawDistance = 3f;
		public const float BaseClawSpeed = 5f;

		public const float ClawOffset = 0.35f;

		public const int NumBulletBlocksIncrement = 1;
		public const float ClawDistanceIncrement = 0.5f;
		public const float ClawSpeedIncrement = 1.5f;

		public enum States { Extending, Retracting, Grabbed };
	}

	public static class RainbowAttack
	{
		public const int NumWaves = 3;
		public const int NumProjectiles = 50;

		public const float WaveDelayTime = 0.15f;

		public const float ProjectileLifespan = 2f;

		public const float MoveSpeed = 3f;
		public static readonly Vector2 maxValidDistanceAwayFromScreen = new Vector2(4, 4);

		public const float ScoreEarnedOnAttack = 2500f;

		public static readonly List<Color> Colors = new List<Color>() 
		{ 
			new Color(1.0f, 0.33f, 0.41f), // Red
			new Color(1.0f, 0.6f, 0.38f),	// Orange
			new Color(0.98f, 0.78f, 0.28f), // Yellow
			new Color(0.44f, 1.0f, 0.49f), // Green
			new Color(0.37f, 1.0f, 0.98f), // Cyan
			new Color(0.27f, 0.35f, 0.95f), // Blue 
			new Color(1.0f, 0.37f, 0.74f), // Pink
		};
	}
}