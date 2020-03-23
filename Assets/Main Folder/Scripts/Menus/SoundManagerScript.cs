using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioClipID
{
	BGM_MAIN_MENU = 1,
	BGM_IN_GAME,
	BGM_SETTING,
	BGM_VICTORY,
	BGM_CUTSCENE,
	BGM_DYING,

	//special
	SBGM_DRAGONGRUNT,

	//UI
	SFX_TAP,

	SFX_SQUIREFOOTSTEPS,
	SFX_KNIGHTFOOTSTEPS,
	SFX_ATTACKING,
	SFX_POTIONDRINK,

	//Crank
	SFX_TURNINGCRANK,
	SFX_PRESSUREPLATE,
	SFX_PLATFORMROTATE,
	SFX_FIREWALL_MAIN,

	//Traps
	SFX_SPRINGTRAP,
	SFX_SPIKETRAP,
	SFX_BOMBEXPLOSION,
	SFX_ARROWTRAP,
	SFX_DRAGONBREATH,

	//Enemies
	SFX_ENEMYDEATH_1,
	SFX_ENEMYDEATH_2,

	SFX_FIREWALL_START,
	SFX_FIREWALL_END,

	SBGM_RandomGrunt1,
	SBGM_RandomGrunt2,
	SBGM_RandomGrunt3,

	TOTAL
}

[System.Serializable]
public class AudioClassInfo
{
	public AudioClipID audioClipID;
	public AudioClip audioClip;
}

public class SoundManagerScript : MonoBehaviour {

	private static SoundManagerScript mInstance;

	public static SoundManagerScript Instance
	{
		get
		{
			if (mInstance == null) 
			{
				GameObject tempObject = GameObject.FindWithTag("SoundManager");

				if(tempObject == null)
				{
					GameObject obj = new GameObject("SoundManager");
					mInstance = obj.AddComponent<SoundManagerScript>();
					obj.tag = "SoundManager";
				}
				else
				{
					mInstance = tempObject.GetComponent<SoundManagerScript>();
				}
				DontDestroyOnLoad(mInstance.gameObject);
			}
			return mInstance;
		}
	}

	public static bool CheckInstanceExist()
	{
		return mInstance;
	}

	public List<AudioClassInfo> audioClipInfo = new List <AudioClassInfo>();

	public float bgmVolume = 1.0f;
	public float sfxVolume = 1.0f;

	public bool isCoroutineOn;

	public AudioSource bgmAudioSource;
	public AudioSource sfxAudioSource;

	public List<AudioSource> loopingSFXAudioSourceList = new List<AudioSource>();

	void Awake () 
	{
		if(SoundManagerScript.CheckInstanceExist())
		{
			Destroy(this.gameObject);
		}
	}

	void Start()
	{
		AudioSource[] audioSourceList = this.GetComponentsInChildren<AudioSource>();
		//audioClipInfo.Capacity = (int)AudioClipID.TOTAL; // dont activate this

		if(audioSourceList[0].gameObject.name == "BGMAudioSource")
		{
			bgmAudioSource = audioSourceList[0];
			sfxAudioSource = audioSourceList[1];
		}
		else 
		{
			bgmAudioSource = audioSourceList[1];
			sfxAudioSource = audioSourceList[0];
		}
	}


	AudioClip FindAudioClip(AudioClipID audioClipID)
	{
		for(int i = 0; i < audioClipInfo.Count; i++)
		{
			if(audioClipInfo[i].audioClipID == audioClipID)
			{
				return audioClipInfo[i].audioClip;
			}
		}

		Debug.LogError("Cannot find ID: " + audioClipID);

		return null;
	}

	public AudioClip FindSFXClip(AudioClipID audioClipID)
	{
		for(int i = 0; i < audioClipInfo.Count; i++)
		{
			if(audioClipInfo[i].audioClipID == audioClipID)
			{
				return audioClipInfo[i].audioClip;
			}
		}

		Debug.LogError("Cannot find ID: " + audioClipID);

		return null;
	}

	public void PlayBGM(AudioClipID audioClipID)
	{
		/*if(!bgmAudioSource.isPlaying)
		{
			bgmAudioSource.clip = FindAudioClip(audioClipID);
			bgmAudioSource.volume = bgmVolume;
			bgmAudioSource.loop = true;
			bgmAudioSource.Play();
		}*/

		 //Baz Version
		AudioClip clipToPlay = FindAudioClip(audioClipID);

		if(clipToPlay == null)
		{
			return;
		}

		bgmAudioSource.clip = clipToPlay;
		bgmAudioSource.volume = bgmVolume;
		bgmAudioSource.loop = true;
		bgmAudioSource.Play();

	}

	public void FadeInPlayBGM(AudioClipID audioClipID)
	{
		if(!bgmAudioSource.isPlaying)
		{
			bgmAudioSource.clip = FindAudioClip(audioClipID);
			StopCoroutine("FadeInBGMRoutine");
			StartCoroutine("FadeInBGMRoutine");
			Debug.Log(bgmVolume);
			bgmAudioSource.loop = true;
			bgmAudioSource.Play();
		}

		/*if(!bgmAudioSource.isPlaying)
		{
			bgmAudioSource.clip = FindAudioClip(audioClipID);
			StartCoroutine(FadeIn(bgmAudioSource, bgmAudioSource.clip, 0.5f, 1f));
		}
		*/


	}

	public void FadeOutBGM()
	{
		StartCoroutine(FadeOut(bgmAudioSource, 0.75f));
	}

	public void PauseBGM()
	{
		if(bgmAudioSource.isPlaying)
		{
			bgmAudioSource.Pause();
		}
	}

	public void UnPauseBGM()
	{
		if(!bgmAudioSource.isPlaying)
		{
			bgmAudioSource.UnPause();
		}
	}

	public void StopBGM()
	{
		if(bgmAudioSource.isPlaying)
		{
			bgmAudioSource.Stop();
		}
	}

	//public void PlaySFX(AudioClipID audioClipID)
	public void PlaySFX(AudioClipID audioClipID)
	{
		sfxAudioSource.PlayOneShot(FindSFXClip(audioClipID), sfxVolume);
	}




	//===========================================================================================
	//Takes in any number of AudioClipIDs and chooses one randomly
	public void PlayRandomAlternating(params AudioClipID[] audioClips)
	{
		List<AudioClipID> randomList = new List<AudioClipID>();
		int randomize;

		foreach (AudioClipID randAudioClip in audioClips)
		{
			randomList.Add(randAudioClip);
		}

		//if (randomList.Count != 0)
		{
			randomize = Random.Range(0, randomList.Count-1);
			sfxAudioSource.PlayOneShot(FindSFXClip((AudioClipID)randomList[randomize]), sfxVolume);
		}

	}
	//===========================================================================================




	public void PlayLoopingSFX(AudioClipID audioClipID)
	{
		AudioClip clipToPlay = FindSFXClip(audioClipID);

		if(clipToPlay == null)
		{
			return;
		}

		for(int i=0; i<loopingSFXAudioSourceList.Count; i++)
		{
			if (loopingSFXAudioSourceList[i] != null)
			{
				if(loopingSFXAudioSourceList[i].clip == clipToPlay)
				{
					if(loopingSFXAudioSourceList[i].isPlaying)
					{
						return;
					}
					else 
					{
						loopingSFXAudioSourceList[i].Play();
					}

					loopingSFXAudioSourceList[i].volume = sfxVolume;
					return;
				}
			}
		}

		AudioSource newInstance = sfxAudioSource.gameObject.AddComponent<AudioSource>();
		newInstance.playOnAwake = false;
		newInstance.clip = clipToPlay;
		newInstance.volume = sfxVolume;
		newInstance.loop = false;//true;
		newInstance.Play();
		loopingSFXAudioSourceList.Add(newInstance);
	}

	public void PlayLoopingSFXWithFadeIn(AudioClipID audioClipID, float fadeInDuration)
	{
		AudioClip clipToPlay = FindAudioClip(audioClipID);

		if(clipToPlay == null)
		{
			return;
		}

		for(int i=0; i<loopingSFXAudioSourceList.Count; i++)
		{
			if(loopingSFXAudioSourceList[i].clip == clipToPlay)
			{
				if(loopingSFXAudioSourceList[i].isPlaying)
				{
					return;
				}

				loopingSFXAudioSourceList[i].volume = sfxVolume;
				StartCoroutine(FadeIn(loopingSFXAudioSourceList[i], clipToPlay, fadeInDuration, sfxVolume));
				return;
			}
		}

		AudioSource newInstance = sfxAudioSource.gameObject.AddComponent<AudioSource>();
		newInstance.playOnAwake = false;
		newInstance.loop = true;

		StartCoroutine(FadeIn(newInstance, clipToPlay, fadeInDuration, sfxVolume));
		loopingSFXAudioSourceList.Add(newInstance);
	}

	public void PauseLoopingSFX(AudioClipID audioClipID)
	{
		AudioClip clipToPause = FindAudioClip(audioClipID);

		if(clipToPause == null)
		{
			return;
		}

		for(int i=0; i<loopingSFXAudioSourceList.Count; i++)
		{
			if(loopingSFXAudioSourceList[i].clip == clipToPause)
			{
				loopingSFXAudioSourceList[i].Pause();
				return;
			}
		}
	}

	public void MuteLoopingSFX(AudioClipID audioClipID)
	{
		AudioClip clipToPause = FindAudioClip(audioClipID);

		if(clipToPause == null)
		{
			return;
		}

		for(int i=0; i<loopingSFXAudioSourceList.Count; i++)
		{
			if(loopingSFXAudioSourceList[i].clip == clipToPause)
			{
				loopingSFXAudioSourceList[i].Pause();
				loopingSFXAudioSourceList[i].mute = true;
				return;
			}
		}
	}

	public void UnmuteLoopingSFX(AudioClipID audioClipID)
	{
		AudioClip clipToPause = FindAudioClip(audioClipID);

		if(clipToPause == null)
		{
			return;
		}

		for(int i=0; i<loopingSFXAudioSourceList.Count; i++)
		{
			if(loopingSFXAudioSourceList[i].clip == clipToPause)
			{
				loopingSFXAudioSourceList[i].UnPause();
				loopingSFXAudioSourceList[i].mute = false;
				return;
			}
		}
	}

	public void StopLoopingSFX(AudioClipID audioClipID)
	{
		AudioClip clipToStop = FindAudioClip(audioClipID);

		if(clipToStop == null)
		{
			return;
		}

		for(int i=0; i<=loopingSFXAudioSourceList.Count; i++)
		{
			if(loopingSFXAudioSourceList[i].clip == clipToStop)
			{
				loopingSFXAudioSourceList[i].Stop();
				return;
			}
		}
	}

	public void StopSFX()
	{
		if(sfxAudioSource.isPlaying)
		{
			sfxAudioSource.Stop();
		}
	}

	public void PauseAllSounds()
	{
		if ( loopingSFXAudioSourceList.Count > 0)
		{
			for(int i = 0; i<loopingSFXAudioSourceList.Count; i++)
			{
				loopingSFXAudioSourceList[i].loop = false;
				loopingSFXAudioSourceList[i].Pause();       ///////////////// Just so you know, Pause() doesn't do shit to this Looping SFX :<
															////////////////  The true one working is the Movement Check if Timescale > 0 in KnightMovementScript
			}

			//PauseBGM();				//Removed now because having it makes more sense. Able to test bgm volume and such.
			//PauseLoopingSFX();
		}
	}

	public void UnPauseAllSounds()
	{
		if ( loopingSFXAudioSourceList.Count > 0)
		{
			for(int i = 0; i<loopingSFXAudioSourceList.Count; i++)
			{
				loopingSFXAudioSourceList[i].loop = true;
				loopingSFXAudioSourceList[i].UnPause();
			}

			//UnPauseBGM();
			//Unpauselooping
		}
	}

	public void StopAllLoopingSFX()
	{
		if (loopingSFXAudioSourceList.Count == 0)
		{
			loopingSFXAudioSourceList.Clear();
			return;
		}

		else if (loopingSFXAudioSourceList.Count > 0)
		{
			for(int i=0; i<loopingSFXAudioSourceList.Count; i++)
			{
				if (loopingSFXAudioSourceList[i] != null)
				{
					loopingSFXAudioSourceList[i].loop = false;
					loopingSFXAudioSourceList[i].Stop();
				}
			}
		}
	}

	public void KillAllSounds()
	{
		if ( loopingSFXAudioSourceList.Count > 0)
		{
			for(int i=0; i<loopingSFXAudioSourceList.Count; i++)
			{
				if (loopingSFXAudioSourceList[i] != null)
				{
					loopingSFXAudioSourceList[i].loop = false;
					loopingSFXAudioSourceList[i].Stop();

					if (i == loopingSFXAudioSourceList.Count-1)
					{
						Component[] audioSources = loopingSFXAudioSourceList[i].gameObject.GetComponents<AudioSource>() as Component[];
						Destroy(audioSources[i+1]); //if its i, it destroys the first index. This is because the current loopingSFXAudioSourceList is only 1

						SoundManagerScript.Instance.loopingSFXAudioSourceList.Capacity -=1;
					}
				}
			}
		}

		StopSFX();
		StopBGM();
	}

	public void ChangePitchLoopingSFX(AudioClipID audioClipID, float value)
	{
		AudioClip clipToStop = FindAudioClip(audioClipID);

		for(int i=0; i<loopingSFXAudioSourceList.Count; i++)
		{
			if(loopingSFXAudioSourceList[i].clip == clipToStop)
			{
				loopingSFXAudioSourceList[i].pitch = value;
				return;
			}
		}
	}

	public void SetVolumeBGM(float volume)
	{
		bgmVolume = volume;
	}

	public void SetVolumeSFX(float volume)
	{
		sfxVolume = volume;
	}

	public void TestVolumeBGM(float volume)
	{
		bgmAudioSource.volume = volume;
	}

	public void TestVolumeSFX(float volume)
	{
		sfxAudioSource.volume = volume;
	}

	public IEnumerator FadeOutBGMRoutine()
	{
		while(bgmAudioSource.volume > 0)
		{
			Debug.Log("Fading");
			bgmAudioSource.volume -= Time.unscaledDeltaTime / 1f;
			yield return null;
		}

		StopBGM();
	}

	public IEnumerator FadeInBGMRoutine()
	{
		while(bgmAudioSource.volume <= bgmVolume)
		{
			bgmAudioSource.volume += Time.unscaledDeltaTime / 1f;
			yield return null;
		}
	}

	IEnumerator FadeOut(AudioSource audioSource, float fadeOutDuration)
	{
		float fadeOutTimer = 0.0f;
		float fadeOutSpeed = audioSource.volume / fadeOutDuration * Time.unscaledDeltaTime;;

		while(fadeOutTimer < fadeOutDuration)
		{
			fadeOutTimer += Time.unscaledDeltaTime;
			audioSource.volume -= fadeOutSpeed;
			yield return null;
		}
		audioSource.volume = 0.0f;
		audioSource.Stop();

		audioSource.volume = 1f;
	}

	IEnumerator FadeOutIn(AudioSource audioSource, AudioClip audioClip, float fadeOutDuration, float fadeInDuration, float maxVolume)
	{
		float fadeOutTimer = 0.0f;
		float originalVolume = audioSource.volume;
		float fadeOutSpeed = originalVolume / fadeOutDuration * Time.deltaTime;

		while(fadeOutTimer < fadeOutDuration)
		{
			fadeOutTimer += Time.unscaledDeltaTime;
			audioSource.volume -= fadeOutSpeed;
			yield return null;
		}
		StartCoroutine(FadeIn(audioSource, audioClip, fadeInDuration, maxVolume));
	}

	IEnumerator FadeIn(AudioSource audioSource, AudioClip audioClip, float fadeInDuration, float maxVolume)
	{
		audioSource.clip = audioClip;
		audioSource.volume = 0.0f;
		audioSource.Play();

		float fadeInTimer = 0.0f;
		float fadeInSpeed = maxVolume / fadeInDuration * Time.unscaledDeltaTime;

		while(fadeInTimer < fadeInDuration)
		{
			fadeInTimer += Time.unscaledDeltaTime;
			audioSource.volume += fadeInSpeed;
			yield return null;
		}
		audioSource.volume = maxVolume;
	}

	IEnumerator FadeOutAll(List<AudioSource> audioSourceList, float fadeOutDuration)
	{
		float fadeOutTimer = 0.0f;
		List<float> fadeOutSpeedList = new List<float>();

		for(int i=0; i<audioSourceList.Count; i++)
		{
			fadeOutSpeedList.Add(audioSourceList[i].volume / fadeOutDuration * Time.unscaledDeltaTime);
		}

		while(fadeOutTimer < fadeOutDuration)
		{
			fadeOutTimer += Time.unscaledDeltaTime;
			for(int i=0; i<audioSourceList.Count; i++)
			{
				audioSourceList[i].volume -= fadeOutSpeedList[i];
			}
			yield return null;
		}
		for(int i=0; i<audioSourceList.Count; i++)
		{
			audioSourceList[i].volume = 0.0f;
			audioSourceList[i].Stop();
		}
	}

	IEnumerator FadeOutInAll(List<AudioSource> audioSourceList, float fadeOutDuration, float fadeInDuration)
	{
		float fadeOutTimer = 0.0f;
		List<float> fadeOutSpeedList = new List<float>();
		List<float> maxVolumeList = new List<float>();

		for(int i=0; i<audioSourceList.Count; i++)
		{
			fadeOutSpeedList.Add(audioSourceList[i].volume / fadeOutDuration * Time.unscaledDeltaTime);
			maxVolumeList.Add(audioSourceList[i].volume);
		}

		while(fadeOutTimer < fadeOutDuration)
		{
			fadeOutTimer += Time.unscaledDeltaTime;
			for(int i=0; i<audioSourceList.Count; i++)
			{
				audioSourceList[i].volume -= fadeOutSpeedList[i];
			}
			yield return null;
		}
		StartCoroutine(FadeInAll(audioSourceList, fadeInDuration, maxVolumeList));
	}

	IEnumerator FadeInAll(List<AudioSource> audioSourceList, float fadeInDuration, List<float> maxVolumeList)
	{
		float fadeInTimer = 0.0f;
		List<float> fadeInSpeedList = new List<float>();

		for(int i=0; i<audioSourceList.Count; i++)
		{
			audioSourceList[i].volume = 0.0f;
			audioSourceList[i].Play();
			fadeInSpeedList.Add(maxVolumeList[i] / fadeInDuration * Time.unscaledDeltaTime);
		}

		while(fadeInTimer < fadeInDuration)
		{
			fadeInTimer += Time.unscaledDeltaTime;
			for(int i=0; i<audioSourceList.Count; i++)
			{				
				audioSourceList[i].volume += fadeInSpeedList[i];
			}
			yield return null;
		}
		for(int i=0; i<audioSourceList.Count; i++)
		{
			audioSourceList[i].volume = maxVolumeList[i];
		}
	}
}

