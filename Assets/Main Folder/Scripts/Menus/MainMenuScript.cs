using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuScript : MonoBehaviour {

	public GameObject creditsUI;
	public GameObject settingsUI;

	bool isBGMPlaying;

	void Start () 
	{
		creditsUI.SetActive(false);
		settingsUI.SetActive(false);

		SoundManagerScript.Instance.PlayBGM(AudioClipID.BGM_MAIN_MENU);
	}

	void Update()
	{
		if (SoundManagerScript.Instance.bgmAudioSource.isPlaying == false)
		{
			SoundManagerScript.Instance.PlayBGM(AudioClipID.BGM_MAIN_MENU);
		}
	}

	public void Play()
	{
		SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_TAP);
		SceneManagerScript.LoadLevelSelect();
	}

	public void Credits()
	{
		SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_TAP);
		creditsUI.SetActive(true);

		SoundManagerScript.Instance.PlayBGM(AudioClipID.BGM_CUTSCENE);
	}

	public void CloseCredits()
	{
		SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_TAP);
		creditsUI.SetActive(false);

		SoundManagerScript.Instance.PlayBGM(AudioClipID.BGM_MAIN_MENU);
	}
}
