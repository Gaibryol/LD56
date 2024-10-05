

public partial class Constants
{
	public static class Player
	{
		public const int BaseHealth = 5;
		public const float BaseMoveSpeed = 3f;
		public const float BaseInvulnerabilityDuration = 1.5f;
		public const float BaseScoreMultiplier = 1f;

		public const float InvulnerableTick = 0.2f;
		public const float InvulnerableAlpha = 0.25f;

		public const int HealthIncrement = 1;
		public const float MoveSpeedIncrement = 1f;
		public const float InvulnerabilityDurationIncrement = 1f;
		public const float ScoreMultiplierIncrement = 0.25f;

		public const int NumToUpgrade = 3;
	}

	public static class Claw
	{
		public const int BaseNumBulletBlocks = 1;
		public const float BaseClawDistance = 3f;
		public const float BaseClawSpeed = 5f;

		public const float ClawOffset = 0.35f;

		public const int NumBulletBlocksIncrement = 1;
		public const float ClawDistanceIncrement = 0.5f;
		public const float ClawSpeedIncrement = 1.5f;

		public enum States { Extending, Retracting, Grabbed };
	}
}