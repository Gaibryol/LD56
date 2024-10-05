

public partial class Constants
{
	public static class Player
	{
		public const float Movespeed = 3f;
		public const int BaseHealth = 5;
		public const float InvulnerableDuration = 1f;
		public const float InvulnerableTick = 0.2f;
	}

	public static class Claw
	{
		public const float ClawOffset = 0.35f;
		public const float ClawDistance = 3f;
		public const float ClawSpeed = 5f;

		public enum States { Extending, Retracting, Grabbed };
	}
}