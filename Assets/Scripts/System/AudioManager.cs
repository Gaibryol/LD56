using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioManager : MonoBehaviour
{
	[SerializeField, Header("Sources")] private AudioSource musicSource;
	[SerializeField] private AudioSource sfxSource;

	[SerializeField, Header("Music")] private AudioClip mainMenu;

	[SerializeField, Header("SFX")] private AudioClip buttonPress;
	[SerializeField] private AudioClip bunnyShoot;
	[SerializeField] private AudioClip clawExtend;
	[SerializeField] private AudioClip clawGrab;
	[SerializeField] private AudioClip clawRetract;
	[SerializeField] private AudioClip frogTongue;
	[SerializeField] private AudioClip hippoBurst;
	[SerializeField] private AudioClip playerDie;
	[SerializeField] private AudioClip playerHit;
	[SerializeField] private AudioClip squidInk;
	[SerializeField] private AudioClip sharkSpray;
	[SerializeField] private AudioClip crabGun;
	[SerializeField] private AudioClip eggLaid;
	[SerializeField] private AudioClip eggsplosion;
	[SerializeField] private AudioClip eggCountdown;
	[SerializeField] private AudioClip upgrade;
	[SerializeField] private AudioClip rainbowAttack;

	private float musicVolume;
	private float sfxVolume;

	private AudioClip oldMusic;
	private float oldTime;

	private Dictionary<string, AudioClip> music = new Dictionary<string, AudioClip>();
	private Dictionary<string, AudioClip> sfx = new Dictionary<string, AudioClip>();

	private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	private Coroutine playMusicFadeCoroutine;

	private void Awake()
	{
		// Set up music and sfx dictionaries
		music.Add(Constants.Audio.Music.MainMenuTheme, mainMenu);

		sfx.Add(Constants.Audio.SFX.ButtonPress, buttonPress);
		sfx.Add(Constants.Audio.SFX.BunnyShoot, bunnyShoot);
		sfx.Add(Constants.Audio.SFX.ClawExtend, clawExtend);
		sfx.Add(Constants.Audio.SFX.ClawGrab, clawGrab);
		sfx.Add(Constants.Audio.SFX.ClawRetract, clawRetract);
		sfx.Add(Constants.Audio.SFX.FrogTongue, frogTongue);
		sfx.Add(Constants.Audio.SFX.HippoBurst, hippoBurst);
		sfx.Add(Constants.Audio.SFX.PlayerDie, playerDie);
		sfx.Add(Constants.Audio.SFX.PlayerHit, playerHit);
		sfx.Add(Constants.Audio.SFX.SquidInk, squidInk);
		sfx.Add(Constants.Audio.SFX.SharkSpray, sharkSpray);
		sfx.Add(Constants.Audio.SFX.CrabGun, crabGun);
		sfx.Add(Constants.Audio.SFX.EggLaid, eggLaid);
		sfx.Add(Constants.Audio.SFX.Eggsplosion, eggsplosion);
		sfx.Add(Constants.Audio.SFX.EggCountdown, eggCountdown);
		sfx.Add(Constants.Audio.SFX.Upgrade, upgrade);
		sfx.Add(Constants.Audio.SFX.RainbowAttack, rainbowAttack);
	}

	private void ChangeMusicVolumeHandler(BrokerEvent<AudioEvents.ChangeMusicVolume> inEvent)
	{
		musicVolume = inEvent.Payload.NewVolume;
		musicSource.volume = musicVolume;

		PlayerPrefs.SetFloat(Constants.Audio.MusicVolumePP, musicVolume);
	}

	private void ChangeSFXVolumeHandler(BrokerEvent<AudioEvents.ChangeSFXVolume> inEvent)
	{
		sfxVolume = inEvent.Payload.NewVolume;
		sfxSource.volume = sfxVolume;

		PlayerPrefs.SetFloat(Constants.Audio.SFXVolumePP, sfxVolume);
	}

	private void PlayMusicHandler(BrokerEvent<AudioEvents.PlayMusic> inEvent)
	{
		if (playMusicFadeCoroutine != null)
		{
			StopCoroutine(playMusicFadeCoroutine);
		}

		if (inEvent.Payload.Transition)
		{
			playMusicFadeCoroutine = StartCoroutine(FadeToSong(inEvent.Payload.MusicName));
		}
		else
		{
			PlayMusic(inEvent.Payload.MusicName);
		}
	}

	private void PlaySFXHandler(BrokerEvent<AudioEvents.PlaySFX> inEvent)
	{
		if (sfx.ContainsKey(inEvent.Payload.SFXName))
		{
			sfxSource.PlayOneShot(sfx[inEvent.Payload.SFXName]);
		}
		else
		{
			Debug.LogError("Cannot find sfx named " + inEvent.Payload.SFXName);
		}
	}

	private void PlayTemporaryMusicHandler(BrokerEvent<AudioEvents.PlayTemporaryMusic> inEvent)
	{
		oldMusic = musicSource.clip;
		oldTime = musicSource.time;
		StartCoroutine(FadeToSong(inEvent.Payload.MusicName, 0, false));
	}

	private void StopTemporaryMusicHandler(BrokerEvent<AudioEvents.StopTemporaryMusic> inEvent)
	{
		StartCoroutine(FadeToSong(oldMusic, oldTime));
	}

	private void GetSongLengthHandler(BrokerEvent<AudioEvents.GetSongLength> inEvent)
	{
		if (music.ContainsKey(inEvent.Payload.Title))
		{
			inEvent.Payload.ProcessData.DynamicInvoke(music[inEvent.Payload.Title].length);
		}
		else
		{
			Debug.LogError("Cannot find music named " + inEvent.Payload.Title);
		}
	}

	private void StopMusicHandler(BrokerEvent<AudioEvents.StopMusic> inEvent)
	{
		musicSource.Stop();
	}

	private void ToggleSFXHandler(BrokerEvent<AudioEvents.ToggleSFX> inEvent)
	{
		sfxSource.mute = !sfxSource.mute;
		PlayerPrefs.SetInt(Constants.Audio.SFXMutedPP, sfxSource.mute ? 1 : 0);
		PlayerPrefs.Save();

		inEvent.Payload.ProcessNewState.DynamicInvoke(sfxSource.mute);
	}

	private void ToggleMusicHandler(BrokerEvent<AudioEvents.ToggleMusic> inEvent)
	{
		musicSource.mute = !musicSource.mute;
		PlayerPrefs.SetInt(Constants.Audio.MusicMutedPP, musicSource.mute ? 1 : 0);
		PlayerPrefs.Save();

		inEvent.Payload.ProcessNewState.DynamicInvoke(musicSource.mute);
	}

	private void PlayMusic(string song, float time = 0f, bool loop = true)
	{
		if (music.ContainsKey(song))
		{
			musicSource.Stop();
			musicSource.clip = music[song];
			musicSource.loop = loop;
			musicSource.Play();
			musicSource.time = time;
		}
		else
		{
			Debug.LogError("Cannot find music named " + song);
		}
	}

	private void PlayMusic(AudioClip song, float time = 0f, bool loop = true)
	{
		musicSource.Stop();
		musicSource.clip = song;
		musicSource.loop = loop;
		musicSource.Play();
		musicSource.time = time;
	}

	private IEnumerator FadeToSong(string song, float time = 0f, bool loop = true)
	{
		while (musicSource.volume > 0)
		{
			musicSource.volume -= Constants.Audio.MusicFadeSpeed * Time.deltaTime;
			yield return null;
		}

		PlayMusic(song, time, loop);

		while (musicSource.volume < musicVolume)
		{
			musicSource.volume += Constants.Audio.MusicFadeSpeed * Time.deltaTime;
			yield return null;
		}
	}

	private IEnumerator FadeToSong(AudioClip song, float time = 0f, bool loop = true)
	{
		while (musicSource.volume > 0)
		{
			musicSource.volume -= Constants.Audio.MusicFadeSpeed * Time.deltaTime;
			yield return null;
		}

		PlayMusic(song, time, loop);

		while (musicSource.volume < musicVolume)
		{
			musicSource.volume += Constants.Audio.MusicFadeSpeed * Time.deltaTime;
			yield return null;
		}
	}

	private void OnEnable()
	{
		eventBrokerComponent.Subscribe<AudioEvents.PlayMusic>(PlayMusicHandler);
		eventBrokerComponent.Subscribe<AudioEvents.PlaySFX>(PlaySFXHandler);
		eventBrokerComponent.Subscribe<AudioEvents.ChangeMusicVolume>(ChangeMusicVolumeHandler);
		eventBrokerComponent.Subscribe<AudioEvents.ChangeSFXVolume>(ChangeSFXVolumeHandler);
		eventBrokerComponent.Subscribe<AudioEvents.PlayTemporaryMusic>(PlayTemporaryMusicHandler);
		eventBrokerComponent.Subscribe<AudioEvents.StopTemporaryMusic>(StopTemporaryMusicHandler);
		eventBrokerComponent.Subscribe<AudioEvents.GetSongLength>(GetSongLengthHandler);
		eventBrokerComponent.Subscribe<AudioEvents.StopMusic>(StopMusicHandler);
		eventBrokerComponent.Subscribe<AudioEvents.ToggleMusic>(ToggleMusicHandler);
		eventBrokerComponent.Subscribe<AudioEvents.ToggleSFX>(ToggleSFXHandler);

		//float musicLevel = PlayerPrefs.GetFloat(Constants.Audio.MusicVolumePP, Constants.Audio.DefaultMusicVolume);
		//float sfxLevel = PlayerPrefs.GetFloat(Constants.Audio.SFXVolumePP, Constants.Audio.DefaultSFXVolume);

		//musicVolume = musicLevel;
		//sfxVolume = sfxLevel;
		musicSource.volume = Constants.Audio.DefaultMusicVolume;
		sfxSource.volume = Constants.Audio.DefaultSFXVolume;

		musicSource.mute = PlayerPrefs.GetInt(Constants.Audio.MusicMutedPP) == 1;
		sfxSource.mute = PlayerPrefs.GetInt(Constants.Audio.SFXMutedPP) == 1;
	}

	private void OnDisable()
	{
		eventBrokerComponent.Unsubscribe<AudioEvents.PlayMusic>(PlayMusicHandler);
		eventBrokerComponent.Unsubscribe<AudioEvents.PlaySFX>(PlaySFXHandler);
		eventBrokerComponent.Unsubscribe<AudioEvents.ChangeMusicVolume>(ChangeMusicVolumeHandler);
		eventBrokerComponent.Unsubscribe<AudioEvents.ChangeSFXVolume>(ChangeSFXVolumeHandler);
		eventBrokerComponent.Unsubscribe<AudioEvents.PlayTemporaryMusic>(PlayTemporaryMusicHandler);
		eventBrokerComponent.Unsubscribe<AudioEvents.StopTemporaryMusic>(StopTemporaryMusicHandler);
		eventBrokerComponent.Unsubscribe<AudioEvents.GetSongLength>(GetSongLengthHandler);
		eventBrokerComponent.Unsubscribe<AudioEvents.StopMusic>(StopMusicHandler);
		eventBrokerComponent.Unsubscribe<AudioEvents.ToggleMusic>(ToggleMusicHandler);
		eventBrokerComponent.Unsubscribe<AudioEvents.ToggleSFX>(ToggleSFXHandler);
	}
}