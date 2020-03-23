using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Zooming_Script : MonoBehaviour {

	public float zoomFactor; // both x and z values will refer to this factor
	//public float defaultOrthCameraSize; // I should create a thing that automatically sets the default !!!!!!!!!!!!!
	public float minimumCameraSize;

	//for Mouse
	bool isMouse0Down = false;

	//All
	//Vector2 startPos = Vector2.zero;
	//float startingDifference = 0.0f;

	//touch input notes
	//when touch is implemented, I will make it such that the camera will only resize to its maximum value after the finger is lifted off

	//public float levelsCounter; //used in order to scale variableMaximumCameraSize and also the defaultOrthCameraSize 
	//public float variableMaximumCameraSize; // game will check this after fingers are lifted / when TouchPhase.Ended

	public float absoluteMaximumCameraSize; // only this is checked during TouchPhase.Moved. so players cant zoom out indefinitely

	// Use this for initialization
	void Start () 
	{
		minimumCameraSize = this.GetComponent<Camera>().orthographicSize;
		zoomFactor = 15.0f;
		//defaultOrthCameraSize = minimumCameraSize + 3.0f;
		absoluteMaximumCameraSize = minimumCameraSize + 12.0f;
	}

	void GetMouseZoom() //to test if logic works //can remove after
	{
		//Vector3 startPos2 = Vector3.zero;


		if (Input.GetMouseButtonDown(0) && !isMouse0Down)
		{
			/*startingDifference 	= Mathf.Abs(Mathf.Sqrt((Input.mousePosition.x - startPos.x) * (Input.mousePosition.x - startPos.x) 
													+ (Input.mousePosition.y - startPos.y) * (Input.mousePosition.y - startPos.y)));
			*/
			isMouse0Down = true;
		}

		if (isMouse0Down)
		{
			//Wrong one
			/*float distanceBetweenFingers 	= Mathf.Abs(Mathf.Sqrt(Input.mousePosition.x * Input.mousePosition.x + Input.mousePosition.y * Input.mousePosition.y) 
											- Mathf.Sqrt(startPos.x * startPos.x + startPos.y * startPos.y));
			*/

			//Supposed Correct formula
			/*float distanceBetweenFingers 	= Mathf.Abs(Mathf.Sqrt((Input.mousePosition.x - startPos.x) * (Input.mousePosition.x - startPos.x) 
																+ (Input.mousePosition.y - startPos.y) * (Input.mousePosition.y - startPos.y)));
																*/

			//if  (distanceBetweenFingers < startingDifference)
			{
				//GetComponent<Camera>().orthographicSize += 2.0f * Time.deltaTime;
				if (Input.GetAxis("Mouse X") > 0 && Input.GetAxis("Mouse Y") > 0)
				{
					GetComponent<Camera>().orthographicSize += Mathf.Abs(Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")) / 2.0f;
				}
				else if (Input.GetAxis("Mouse X") < 0 && Input.GetAxis("Mouse Y") < 0)
				{
					GetComponent<Camera>().orthographicSize += Mathf.Abs(Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")) / 2.0f;
				}
				else if (Input.GetAxis("Mouse X") > 0 && Input.GetAxis("Mouse Y") < 0)
				{
					GetComponent<Camera>().orthographicSize += Mathf.Abs(Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")) / 2.0f;
				}
				else if (Input.GetAxis("Mouse X") < 0 && Input.GetAxis("Mouse Y") > 0)
				{
					GetComponent<Camera>().orthographicSize += Mathf.Abs(Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")) / 2.0f;
				}
			}

			/*if (distanceBetweenFingers > startingDifference)
			{
				GetComponent<Camera>().orthographicSize -= 2.0f * Time.deltaTime;
			}
			*/

			//still buggy :<
			//GetComponent<Camera>().orthographicSize += (Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")) / 2.0f;  // No limits but works fine
			//GetComponent<Camera>().orthographicSize += (-Mathf.Abs(Input.GetAxis("Mouse X") + Mathf.Abs(Input.GetAxis("Mouse Y")))) / 2.0f;


			//Version 2
			//Didn't work as I imagined
			/*
			//Vector3 mouseTransform = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//float angle = Mathf.Atan2 (mouseTransform.y, mouseTransform.x) * Mathf.Rad2Deg;

			Vector2 vectorDifference = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - startPos1;
			float angle = Mathf.Atan2 (vectorDifference.y, vectorDifference.x) * Mathf.Rad2Deg;
			Debug.Log(angle);

			if (angle >= 0.0f && angle < 90.0f)
			{
				GetComponent<Camera>().orthographicSize += Mathf.Abs((Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y"))) / 2.0f;
			}
			else if (angle >= 90.0f && angle < 180.0f)
			{
				GetComponent<Camera>().orthographicSize += (Mathf.Abs(Input.GetAxis("Mouse X")) - Mathf.Abs(Input.GetAxis("Mouse Y"))) / 2.0f;
			}
			else if (angle >= 180.0f && angle < 270.0f)
			{
				GetComponent<Camera>().orthographicSize += -Mathf.Abs((Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y"))) / 2.0f;
			}
			else if (angle >= 270.0f && angle < 360.0f)
			{
				GetComponent<Camera>().orthographicSize += (-Mathf.Abs(Input.GetAxis("Mouse X")) + Mathf.Abs(Input.GetAxis("Mouse Y"))) / 2.0f;
			}
			*/ 

			//Version 1
			//have limits but are buggy :<
			/*if (GetComponent<Camera>().orthographicSize > minimumCameraSize)
			{
				GetComponent<Camera>().orthographicSize -= Mathf.Abs(Input.GetAxis("Mouse X") * -1.0f);
			}

			if (GetComponent<Camera>().orthographicSize < absoluteMaximumCameraSize)
			{
				GetComponent<Camera>().orthographicSize += Mathf.Abs(Input.GetAxis("Mouse X") * -1.0f);
			}
			*/
		}

		if (Input.GetMouseButtonUp(0))
		{
			isMouse0Down = false;
		}
	}

	void Update () 
	{
		GetMouseZoom();

		if (Input.GetKey(KeyCode.I) || Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			//I want to increase the camera distance to be closer in the editor //maybe think of a way to automatically adjust the min and max size according to map??
			if ( GetComponent<Camera>().orthographicSize > minimumCameraSize)
			{
				GetComponent<Camera>().orthographicSize -= zoomFactor * Time.deltaTime;
			}
		}

		if (Input.GetKey(KeyCode.K) || Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			if ( GetComponent<Camera>().orthographicSize < absoluteMaximumCameraSize)
			{
				GetComponent<Camera>().orthographicSize += zoomFactor * Time.deltaTime;
			}
		}

		if (Input.GetKeyDown(KeyCode.R)) //Should have a button for this
		{
			GetComponent<Camera>().orthographicSize = minimumCameraSize;
		}
	}
}
