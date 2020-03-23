using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour {

	public GameObject pauseUI;
	public GameObject settingUI;
	public GameObject quitUI;

	private bool isRestartPressed;

	void Start()
	{
		pauseUI.SetActive(false);
		settingUI.SetActive(false);
		quitUI.SetActive(false);

		isRestartPressed = false;
	}

	void Update()
	{
		if (FadeInOutScript.Instance.isFullyFadedToBlack == true && FadeInOutScript.Instance.isNextLevelLoaded == false && isRestartPressed == true)
		{
			FadeInOutScript.Instance.isFullyFadedToBlack = false;
			FadeInOutScript.Instance.isNextLevelLoaded = true;
			LoadThisScene();
			isRestartPressed = false;
		}
	}

	public void MainMenu()
	{
		SceneManagerScript.LoadMainMenuScene();
		Time.timeScale = 1f;

		SoundManagerScript.Instance.KillAllSounds();
	}

	public void Restart()
	{
		isRestartPressed = true;
		ClosePause();
		Time.timeScale = 1f;
		FadeInOutScript.Instance.isFadeToBlack = true;
		FadeInOutScript.Instance.isNextLevelLoaded = false;
		FadeInOutScript.Instance.FadeToBlack();

		SoundManagerScript.Instance.FadeOutBGM();
	}

	void LoadThisScene()
	{
		FadeInOutScript.Instance.isFadeToScene = true;
		FadeInOutScript.Instance.isNextLevelLoaded = false;
		FadeInOutScript.Instance.FadeToScene();

		SceneManagerScript.LoadCurrentScene();
	}

	public void Pause()
	{
		Time.timeScale = 0f;
		SoundManagerScript.Instance.PauseAllSounds();

		if(pauseUI.activeSelf)
		{
			ClosePause();
		}
		else
		{
			pauseUI.SetActive(true);
		}

		SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_TAP);
	}

	public void ClosePause()
	{
		if (quitUI.activeSelf)
		{
			return;
		}

		if (settingUI.activeSelf)
		{
			settingUI.SetActive(false);
		}

		Time.timeScale = 1f;

		SoundManagerScript.Instance.UnPauseAllSounds();

		pauseUI.SetActive(false);

		SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_TAP);
	}

	public void Quit()
	{
		quitUI.SetActive(true);
	}

	public void No()
	{
		quitUI.SetActive(false);
	}
}
