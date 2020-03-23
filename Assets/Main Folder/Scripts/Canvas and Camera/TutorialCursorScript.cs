using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCursorScript : MonoBehaviour
{
	public enum FingerType
	{
		Object = 0,
		UI
	}

	public enum TutorialLevel
	{
		Cranking = 0,
		Shield
	}

	public TutorialLevel whichTutorial;
	public FingerType whatFinger;
	public SpriteRenderer thisSR;
	public Image thisImage;
	public GameObject textScreen;
	public Vector3 sinPos;
	public float timer;
	public int phase;

	public GameObject[] neededObjects;

	void Start()
	{
		
		textScreen = GameObject.Find("Dialogue");

		if(whatFinger == FingerType.Object)
		{
			thisSR = GetComponent<SpriteRenderer>();
			thisSR.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
		}
		else
		{
			thisImage = GetComponent<Image>();
			thisImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
		}

		phase = 0;

		if(whichTutorial == TutorialLevel.Cranking)
		{
			neededObjects = new GameObject[2];
			neededObjects[0] = GameObject.Find("Crank01");
			neededObjects[1] = GameObject.Find("Crank02");
			transform.position = new Vector3(neededObjects[0].transform.position.x, neededObjects[0].transform.position.y + 3.3f, 
				neededObjects[0].transform.position.z);
		}
		else
		{
			if(whatFinger == FingerType.Object)
			{
				neededObjects = new GameObject[2];
				neededObjects[0] = GameObject.FindGameObjectWithTag("ThrowManager");
				neededObjects[1] = GameObject.FindGameObjectWithTag("Player");
				neededObjects[1].GetComponent<SquireMovementScript>().enabled = false;
			}
			else
			{
				neededObjects = new GameObject[1];
				neededObjects[0] = GameObject.FindGameObjectWithTag("ThrowManager");
			}
		}

		sinPos = transform.position;
	}

	void Update()
	{
		if(!textScreen.activeSelf)
		{
			if(whatFinger == FingerType.Object && whichTutorial == TutorialLevel.Cranking)
			{
				thisSR.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			}
			else if(whatFinger == FingerType.UI)
			{
				thisImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			}

			if(Time.timeScale != 0.0f)
			{
				timer += Time.deltaTime * 1.5f;

				if(whatFinger == FingerType.Object)
				{
					if(whichTutorial == TutorialLevel.Cranking)
					{
						sinPos.y = sinPos.y + Mathf.Sin(timer) * 0.02f;
					}
					else
					{
						sinPos.z = sinPos.z + Mathf.Sin(timer) * 0.01f;
					}
				}
				else
				{
					sinPos.y = sinPos.y + Mathf.Sin(timer) * 0.3f;
				}

				transform.position = sinPos;
			}


		}

		if(whichTutorial == TutorialLevel.Cranking && !textScreen.activeSelf)
		{
			if(neededObjects[0].GetComponent<CrankActivateScript>().activate && phase == 0)
			{
				transform.position = new Vector3(neededObjects[1].transform.position.x, neededObjects[1].transform.position.y + 3.3f, 
					neededObjects[1].transform.position.z);
				sinPos = transform.position;
				phase = 1;
				timer = 0;
			}

			if(neededObjects[1].GetComponent<CrankActivateScript>().activate && phase == 1)
			{
				transform.position = new Vector3(neededObjects[0].transform.position.x, neededObjects[0].transform.position.y + 3.3f, 
					neededObjects[0].transform.position.z);
				sinPos = transform.position;
				phase = 2;
				timer = 0;
			}

			if(neededObjects[0].GetComponent<CrankActivateScript>().activate && phase == 2)
			{
				gameObject.SetActive(false);
			}
		}
		else
		{
			if(whatFinger == FingerType.UI && !textScreen.activeSelf)
			{
				if(neededObjects[0].GetComponent<ThrowItemScript>().isAiming)
				{
					thisImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
				}
				else if(!neededObjects[0].GetComponent<ThrowItemScript>().isAiming && !neededObjects[0].GetComponent<ThrowItemScript>().isTurning)
				{
					thisImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
				}

				if(neededObjects[0].GetComponent<ThrowItemScript>().shieldAmount < 1)
				{
					gameObject.SetActive(false);
				}
			}
			else if(whatFinger == FingerType.Object && !textScreen.activeSelf)
			{
				if(neededObjects[0].GetComponent<ThrowItemScript>().isAiming && phase == 0)
				{
					thisSR.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
					timer = 0.0f;
					phase = 1;
				}
				else if(!neededObjects[0].GetComponent<ThrowItemScript>().isAiming)
				{
					thisSR.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
					phase = 0;
				}

				if(neededObjects[0].GetComponent<ThrowItemScript>().shieldAmount < 1)
				{
					neededObjects[1].GetComponent<SquireMovementScript>().enabled = true;
					gameObject.SetActive(false);
				}
			}
		}
	}
}
