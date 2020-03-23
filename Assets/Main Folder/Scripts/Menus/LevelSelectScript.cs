using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectScript : MonoBehaviour {

	int levelIndex;
	int stars = 0;

	private string targetLevel;

	// Use this for initialization
	void Start () 
	{
		CheckLockedLevels();	
	}

	public void Update()
	{
		if (FadeInOutScript.Instance.isFullyFadedToBlack == true && FadeInOutScript.Instance.isNextLevelLoaded == false)
		{
			FadeInOutScript.Instance.isFullyFadedToBlack = false;
			FadeInOutScript.Instance.isNextLevelLoaded = true;
			LevelChange(targetLevel);
		}
	}

	public void SelectLevel(string levelNum)
	{
		SoundManagerScript.Instance.FadeOutBGM();
		targetLevel = levelNum;

		//This one starts the FadeToBlack() Coroutine from the FadeInOutScript
		FadeInOutScript.Instance.isFadeToBlack = true;
		FadeInOutScript.Instance.isNextLevelLoaded = false;
		FadeInOutScript.Instance.FadeToBlack();
	}

	public void LevelChange(string levelNum)
	{
		//This one starts the FadeToScene() Coroutine from the FadeInOutScript
		FadeInOutScript.Instance.isFadeToScene = true;
		FadeInOutScript.Instance.isNextLevelLoaded = false;
		FadeInOutScript.Instance.FadeToScene();

		SceneManager.LoadScene("Level "+levelNum);
	}

	void CheckLockedLevels()
	{
		/*	for(int i = 1; i<LockLevelScript.levels; i++)
		{
			levelIndex = (i+1);
			if(PlayerPrefs.GetInt("Level"+levelIndex.ToString())== 1)
			{
				GameObject.Find("LockedLevel"+(i+1)).GetComponent<Image>().enabled = false;
				Debug.Log("Level unlocked");
			}
		} */

		Scene scene = SceneManager.GetActiveScene();

		if(scene.name == "Level Select")
		{
			for(int i = 1; i<6; i++)
			{
				stars = PlayerPrefs.GetInt(i.ToString()+"stars");
				GameObject.Find(i+"star"+stars).GetComponent<Image>().enabled = true;
				Debug.Log(i+"star"+stars);

				levelIndex = (i+1);
				if(PlayerPrefs.GetInt("Level"+levelIndex.ToString())== 1)
				{
					GameObject.Find("LockedLevel"+(i+1)).GetComponent<Image>().enabled = false;
					//Debug.Log("Level unlocked");
				}
			}

			stars = PlayerPrefs.GetInt(6.ToString()+"stars");
			GameObject.Find(6+"star"+stars).GetComponent<Image>().enabled = true;
			Debug.Log(6+"star"+stars);
		}
		else if(scene.name == "Level Select 2")
		{
			for(int i = 6; i<12; i++)
			{
				stars = PlayerPrefs.GetInt((i+1).ToString()+"stars");
				GameObject.Find((i+1)+"star"+stars).GetComponent<Image>().enabled = true;
				Debug.Log((i+1)+"star"+stars);

				levelIndex = (i+1);
				if(PlayerPrefs.GetInt("Level"+levelIndex.ToString())== 1)
				{
					GameObject.Find("LockedLevel"+(i+1)).GetComponent<Image>().enabled = false;
					//Debug.Log("Level unlocked " + i+1);
				}
			} 
		}
		else if(scene.name == "Level Select 3")
		{
			for(int i = 12; i<18; i++)
			{
				stars = PlayerPrefs.GetInt((i+1).ToString()+"stars");
				GameObject.Find((i+1)+"star"+stars).GetComponent<Image>().enabled = true;
				Debug.Log((i+1)+"star"+stars);

				levelIndex = (i+1);
				if(PlayerPrefs.GetInt("Level"+levelIndex.ToString())== 1)
				{
					GameObject.Find("LockedLevel"+(i+1)).GetComponent<Image>().enabled = false;
					//Debug.Log("Level unlocked");
				}
			}
		}
	}

	public void LoadMainMenu()
	{
		SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_TAP);
		SceneManagerScript.LoadMainMenuScene();
	}

	public void LoadLevelSelect()
	{
		SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_TAP);
		SceneManagerScript.LoadLevelSelect();
	}

	public void LoadLevelSelect2()
	{
		SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_TAP);
		SceneManagerScript.LoadLevelSelect2();
	}

	public void LoadLevelSelect3()
	{
		SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_TAP);
		SceneManagerScript.LoadLevelSelect3();
	}
}
