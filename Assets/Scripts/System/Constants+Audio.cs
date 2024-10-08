using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Constants
{
	public static class Audio
	{
		public const string MusicVolumePP = "Music";
		public const string SFXVolumePP = "SFX";
		public const string MusicMutedPP = "MusicMuted";
		public const string SFXMutedPP = "SFXMuted";

		public const float DefaultMusicVolume = 0.05f;
		public const float DefaultSFXVolume = 0.05f;

		public const float MusicFadeSpeed = 0.1f;

		public class Music
		{
			public const string MainMenuTheme = "MainMenu";
		}

		public class SFX
		{
			public const string ButtonPress = "ButtonPress";

			// Enemy
			public const string BunnyShoot = "BunnyShoot";
			public const string FrogTongue = "FrogTongue";
			public const string HippoBurst = "HippoBurst";
			public const string SquidInk = "SquidInk";
			public const string SharkSpray = "SharkSpray";
			public const string CrabGun = "CrabGun";
			public const string EggLaid = "EggLaid";
			public const string EggCountdown = "EggCountdown";
			public const string Eggsplosion = "Eggsplosion";

			// Player
			public const string ClawExtend = "ClawExtend";
			public const string ClawGrab = "ClawGrab";
			public const string ClawRetract = "ClawRetract";
			public const string PlayerDie = "PlayerDie";
			public const string PlayerHit = "PlayerHit";
			public const string Upgrade = "Upgrade";
			public const string RainbowAttack = "RainbowAttack";
		}
	}
}