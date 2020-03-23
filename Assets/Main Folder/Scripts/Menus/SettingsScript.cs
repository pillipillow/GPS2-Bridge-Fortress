using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour {

	public GameObject settingsUI;
	public Slider[]volumeSliders;
	GameObject bgmAudioSource;
	GameObject sfxAudioSource;
	AudioSource bgmSource;
	AudioSource sfxSource;

	void Start () 
	{
		bgmAudioSource = GameObject.Find("BGMAudioSource");
		bgmSource = bgmAudioSource.GetComponent<AudioSource>();

		sfxAudioSource = GameObject.Find("SFXAudioSource");
		sfxSource = sfxAudioSource.GetComponent<AudioSource>();

		volumeSliders [0].value = SoundManagerScript.Instance.bgmVolume;
		volumeSliders [1].value = SoundManagerScript.Instance.sfxVolume;	
	}
	
	public void SetVolumeBGM()
	{
		bgmSource.volume = volumeSliders[0].value;
		SoundManagerScript.Instance.SetVolumeBGM(bgmSource.volume);
	}

	public void SetVolumeSFX()
	{
		sfxSource.volume = volumeSliders[1].value;
		SoundManagerScript.Instance.SetVolumeSFX(sfxSource.volume);
	}

	public void Settings()
	{
		SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_TAP);
		settingsUI.SetActive(true);
	}

	public void CloseSettings()
	{
		SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_TAP);
		settingsUI.SetActive(false);
	}
}
