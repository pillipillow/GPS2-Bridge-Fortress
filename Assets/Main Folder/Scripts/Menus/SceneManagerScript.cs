using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour {

	public static void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	public static void LoadMainMenuScene()
	{
		LoadScene("Main Menu");
	}

	public static void LoadCurrentScene()
	{
		string currentScene = SceneManager.GetActiveScene().name;
		LoadScene(currentScene);
	}

	public static void LoadLevelSelect()
	{
		LoadScene("Level Select");
	}

	public static void LoadLevelSelect2()
	{
		LoadScene("Level Select 2");
	}

	public static void LoadLevelSelect3()
	{
		LoadScene("Level Select 3");
	}

	public static void LoadLevel0()
	{
		LoadScene("Level 0");
	}
}
