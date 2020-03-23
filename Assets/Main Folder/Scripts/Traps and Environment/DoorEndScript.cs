using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DoorEndScript : MonoBehaviour
{
	public KnightMovementScript knightMovementScript;

	public List<GameObject> levelEndContent = new List<GameObject>();
	public GameObject dialogueUI;
	public GameObject star1;
	public GameObject star2;
	public GameObject star3;
	public GameObject emoticon1;
	public GameObject emoticon2;
	public GameObject emoticon3;
	public GameObject nextRoom;

	public Text healthRemains;
	KnightAttackScript knightScript;

	private bool hasKnightWon;
	    
	public string[] sceneArray;
	public string currScene;
	int levelIndex;

	void Start()
	{
		hasKnightWon = false;
		FadeInOutScript.Instance.isNextLevelLoaded = true;
		FadeInOutScript.Instance.isFadeToBlack = false;
		FadeInOutScript.Instance.isFadeToScene = false;
		
		knightMovementScript = GameObject.FindGameObjectWithTag("Knight").GetComponent<KnightMovementScript>();
		int sceneLength = SceneManager.sceneCountInBuildSettings;

		sceneArray = new string[sceneLength];

		for(int i = 0; i < sceneArray.Length; i ++)
		{
			sceneArray[i] = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
		}


		dialogueUI = GameObject.Find("Dialogue");
		star1 = GameObject.Find("star1");
		star2 = GameObject.Find("star2");
		star3 = GameObject.Find("star3");
		emoticon1 = GameObject.Find("emoticon1");
		emoticon2 = GameObject.Find("emoticon2");
		emoticon3 = GameObject.Find("emoticon3");
		nextRoom = GameObject.Find("Next Room");

		healthRemains = GameObject.Find("HealthRemains").GetComponentInChildren<Text>();
		knightScript = GameObject.FindGameObjectWithTag("Knight").GetComponent<KnightAttackScript>();
		Button btn = nextRoom.GetComponent<Button>();
		btn.onClick.AddListener(LoadNextLevel);

		levelEndContent.Add(GameObject.Find("BG"));
		levelEndContent.Add(GameObject.Find("Title"));
		levelEndContent.Add(GameObject.Find("HealthRemains"));
		levelEndContent.Add(nextRoom);

		star1.GetComponent<Image>().enabled = false;
		star2.GetComponent<Image>().enabled = false; 
		star3.GetComponent<Image>().enabled = false;
		emoticon1.GetComponent<Image>().enabled = false;
		emoticon2.GetComponent<Image>().enabled = false;
		emoticon3.GetComponent<Image>().enabled = false;
		for(int i = 0; i<levelEndContent.Count;i++)
		{
			levelEndContent[i].SetActive(false);
		}    
	}

	void Update()
	{
		currScene = SceneManager.GetActiveScene().name;

		if(Input.GetKeyDown(KeyCode.End))
		{
			SoundManagerScript.Instance.KillAllSounds();
			hasKnightWon = true;

			LoadNextLevel();
		}

		if (FadeInOutScript.Instance.isFullyFadedToBlack == true && hasKnightWon == true)
		{
			FadeInOutScript.Instance.isFullyFadedToBlack = false;
			FadeInOutScript.Instance.isNextLevelLoaded = true;
			LevelChange();
		}

	}

	void OnTriggerEnter(Collider coll)
	{
		if(coll.CompareTag("Knight"))
		{
			knightMovementScript.isMoving = false;
			hasKnightWon = true;
			LevelEnd();
		}
	}

	public void LoadNextLevel()
	{
		Time.timeScale = 1f; //might want to shift this to LevelChange();

		FadeInOutScript.Instance.isFadeToBlack = true;
		FadeInOutScript.Instance.isNextLevelLoaded = false;
		FadeInOutScript.Instance.FadeToBlack();

	}

	void LevelChange()
	{
		for(int i = 0; i < sceneArray.Length; i ++)
		{
			if(sceneArray[i] == currScene)
			{
				int sceneCheck = i + 1;

				if(sceneCheck >= sceneArray.Length - 1)
				{
					//This one starts the FadeToScene() Coroutine from the FadeInOutScript
					FadeInOutScript.Instance.isFadeToScene = true;
					FadeInOutScript.Instance.isNextLevelLoaded = true;
					FadeInOutScript.Instance.FadeToScene();

					SceneManager.LoadScene(sceneArray[0]);
				}
				else
				{
					//This one starts the FadeToScene() Coroutine from the FadeInOutScript
					FadeInOutScript.Instance.isFadeToScene = true;
					FadeInOutScript.Instance.isNextLevelLoaded = true;
					FadeInOutScript.Instance.FadeToScene();

					SceneManager.LoadScene(sceneArray[i + 1]);
				}
			}
		}
	}

	void UnlockLevels(int stars)
	{
		for (int i = 1; i<LockLevelScript.levels; i++)
		{
			if(currScene == "Level "+(i).ToString())
			{
				levelIndex = (i+1);
				PlayerPrefs.SetInt("Level"+levelIndex.ToString(),1);
				if(PlayerPrefs.GetInt(i.ToString()+"stars")<stars)
				{
					PlayerPrefs.SetInt(i.ToString()+"stars",stars);
				}
			}
		}
	}

	void LevelEnd()
	{
		SoundManagerScript.Instance.StopAllLoopingSFX();

		Time.timeScale = 0f;
		knightMovementScript.isMoving = false;

		//healthRemains.text = "Health Remain: "+ knightScript.currentStamina.ToString();
		for(int i = 0; i<levelEndContent.Count;i++)
		{
			levelEndContent[i].SetActive(true);
		}    
		if(knightScript.currentStamina == knightScript.stamina)
		{
			emoticon3.GetComponent<Image>().enabled = true;
    		star3.GetComponent<Image>().enabled = true;
			healthRemains.text = "Knight is healthy!";
    		UnlockLevels(3);   
   		}
		else if(knightScript.currentStamina < knightScript.stamina && knightScript.currentStamina > (knightScript.stamina/2.0f))
   		{
			emoticon2.GetComponent<Image>().enabled = true;
    		star2.GetComponent<Image>().enabled = true;
			healthRemains.text = "Knight is content.";
    		UnlockLevels(2);   
   		}
		else if(knightScript.currentStamina < (knightScript.stamina/2.0f))
   		{
			emoticon1.GetComponent<Image>().enabled = true;
    		star1.GetComponent<Image>().enabled = true;
			healthRemains.text = "Knight is tired...";
    		UnlockLevels(1);   
   		}
	}
}
