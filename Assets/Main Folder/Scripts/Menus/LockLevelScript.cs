using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockLevelScript : MonoBehaviour {

	public static int levels = 18;
	public GameObject settingsUI;
	int levelIndex;

	void Start()
	{
		
	}

	void LockLevels()
	{
		for(int i = 1; i<levels; i++)
		{
			levelIndex = (i+1);
			if(!PlayerPrefs.HasKey("Level"+levelIndex.ToString()))
			{
				PlayerPrefs.SetInt("Level"+levelIndex.ToString(),0);
			}
		}
	}

	public void ResetData()
	{
		SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_TAP);
		PlayerPrefs.DeleteAll();
		LockLevels();
		settingsUI.SetActive(false);
	}

	public void NewGame()
	{
		SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_TAP);
		PlayerPrefs.DeleteAll();
		LockLevels();
		SceneManagerScript.LoadLevel0();
	}
}
