using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTouchInputScript : MonoBehaviour 
{
	public GameObject midPointY;

	public Transform tOne;
	public Transform tTwo;

	public Vector3 velocity;
	private Vector2 direction;

	private Vector2 startPos;
	public float minSwipeDistance;

	private float magnitude;
	private float initDistance;
	public float cameraSize;
	private float minCameraSize = 18.0f;
	private float maxCameraSize = 36.0f;

	public float returnSpeed;
	public float zoomSpeed;
	public float fingerOffset = 10.0f;

	public float shakeDuration;
	public float shakeIntensity = 1f;
	public bool isShrunk;

	//Keyboardstuff
	public Vector3 defaultCameraPosition;
	public Vector3 defaultCameraRotation;

	//public bool isDragging = false; //Interpolate velocity back to 0 while not dragging
	public bool isPinching = false; 

	//Rotation snap
	public int currentCorner;

	public enum CORNERS
	{
		NORTH = 0,
		EAST,
		SOUTH,
		WEST,

		TOTAL = 5
	}

	public CORNERS corner = CORNERS.SOUTH;

	void Start()
	{
			// If it's too small, the pinch zoom will sometimes activate rotate.
			//0.57 screen size seems like magic after many, many trials
		midPointY = GameObject.Find("MidPoint");
		minSwipeDistance = Screen.width * 0.57f; 
		returnSpeed = 0f;//= 10.0f; for Video Recording purposes
		zoomSpeed = 5.0f;
			
		cameraSize = Camera.main.orthographicSize;

		defaultCameraPosition = transform.position;
		defaultCameraRotation = transform.eulerAngles;

		currentCorner = (int)corner;

		shakeDuration = 0f;
		shakeIntensity = 1f;
	}

	void RotateNearestCorner()
	{
		/*
		case TouchPhase.Ended:
		{
			Vector2 endPos = Input.GetTouch(0).position;

			magnitude = startPos.x - endPos.x;

			if (magnitude < (0 - minSwipeDistance))
			{
				// go to corner on the left
				currentCorner -= 1;

				if (currentCorner < (int)CORNERS.NORTH)
				{
					currentCorner = (int)CORNERS.WEST;
				}
			}

			if (magnitude > (0 + minSwipeDistance))
			{
				// go right
				currentCorner += 1;

				if (currentCorner < (int)CORNERS.WEST)
				{
					currentCorner = (int)CORNERS.NORTH;
				}
			}
		}
		*/
	}

	void Rotate90Deg()
	{

		if (Input.touches.Length == 1)
		{
			
			switch (Input.GetTouch(0).phase)
			{
			case TouchPhase.Began: startPos = Input.GetTouch(0).position; break;
			case TouchPhase.Moved:
				{
					//this.transform.RotateAround( midPointY.transform.position, Vector3.up, 1.0f); //need some tests for the feels
					//tOne.rotationm = Quaternion.Lerp( tOne.rotation, tTwo,rotation, Time.deltaTime);
				}break;

			case TouchPhase.Ended:
				{
					//Current Working one
					Vector2 endPos = Input.GetTouch(0).position;

					magnitude = startPos.x - endPos.x;

					if (magnitude > (0 + minSwipeDistance))
					{
						this.transform.RotateAround( midPointY.transform.position, Vector3.up, -90); // ,Vector3.up, -90 + (??)
					}
					else if (magnitude < (0 - minSwipeDistance))
					{
						this.transform.RotateAround( midPointY.transform.position, Vector3.up, 90);
					}
				}; break;
			}
		}
	}


	void PinchZoom()
	{
		if (!isPinching && cameraSize > maxCameraSize)
		{
			cameraSize -= returnSpeed * Time.fixedDeltaTime; 
		}
			
		if (!isPinching && cameraSize < minCameraSize)
		{
			cameraSize += returnSpeed * Time.fixedDeltaTime; 
		}

		Camera.main.orthographicSize = cameraSize;

		if (Input.touches.Length == 2)
		{
			isPinching = true;
			//get init Distance as soon as second input is detected
			if (Input.GetTouch(1).phase == TouchPhase.Began)
			{
				initDistance = Vector2.Distance( Input.GetTouch(0).position, Input.GetTouch(1).position);
			}

			if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved )
			{
				float curDistance = Vector2.Distance( Input.GetTouch(0).position, Input.GetTouch(1).position);
				magnitude = initDistance - curDistance;

				if (magnitude > 0 && cameraSize < maxCameraSize || magnitude < 0 && cameraSize > minCameraSize)
				{
					cameraSize += magnitude * zoomSpeed * Time.fixedDeltaTime;
				}
			}
		}
		else isPinching = false;
	}

	void Update () 
	{
		//Rotate90Deg(); //separated for now, awaiting inverted axis UI  //no longer has this func
		//PinchZoom(); //temporarily removed 

		if (shakeDuration > 0f && !ThrowItemScript.Variables.isAiming)
		{
			transform.position = new Vector3(defaultCameraPosition.x, defaultCameraPosition.y, defaultCameraPosition.z) + Random.insideUnitSphere * 0.5f * shakeIntensity;
			transform.eulerAngles = new Vector3(defaultCameraRotation.x, defaultCameraRotation.y, defaultCameraRotation.z) + Random.insideUnitSphere * 0.1f * shakeIntensity;

			shakeDuration -= Time.deltaTime;

			if (shakeDuration <= 0f)
			{
				transform.position = new Vector3(defaultCameraPosition.x, defaultCameraPosition.y, defaultCameraPosition.z); // will always go back to original
				transform.eulerAngles = new Vector3(defaultCameraRotation.x, defaultCameraRotation.y, defaultCameraRotation.z);
			}
		}

		if (Input.anyKey) KeyBoardRotation(); //if ur lazy to connect to phone to test stuff
	}	

	void KeyBoardRotation()
	{
		{
			if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D))
			{
				gameObject.transform.RotateAround (midPointY.transform.position, Vector3.up, 2.5f); 
			}

			if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A))
			{
				gameObject.transform.RotateAround (midPointY.transform.position, Vector3.up, -2.5f); 
			}
		}

		if (Input.GetKeyDown(KeyCode.R)) //Should have a button for this
		{
			transform.position = defaultCameraPosition;
			transform.eulerAngles = defaultCameraRotation;
			GetComponent<Camera>().orthographicSize = minCameraSize;
		}
	}
}


