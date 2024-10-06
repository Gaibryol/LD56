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

		public const float DefaultMusicVolume = 0.25f;
		public const float DefaultSFXVolume = 0.25f;

		public const float MusicFadeSpeed = 0.1f;

		public class Music
		{
			public const string MainMenuTheme = "MainMenu";
		}

		public class SFX
		{
			public const string ButtonPress = "ButtonPress";
		}
	}
}