using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_VerticalRotation_Script : MonoBehaviour 
{
	private float maximumRotation = 30.0f;
	private float minimumRotation = 5.0f;

	//Vector3 defaultCameraPosition = new Vector3(-15f, 0, -15f);
	//Vector3 defaultCameraRotation = new Vector3(30.0f, 45.0f, 0);

	Vector3 defaultCameraPosition;
	Vector3 defaultCameraRotation;

	Vector3 cameraRotationY = new Vector3(0, 1f, 0);
	//Vector3 cameraRotationXYZ = new Vector3(0.5f, 0, -0.5f);
	//Vector3 cameraRotationReverseXYZ = new Vector3(-0.5f, 0, -0.5f);

	public GameObject midPoint;
	public Vector2 startPos;

	public List<GameObject> touchList = new List<GameObject>();
	public GameObject[] oldTouches;
	private RaycastHit hit;

	bool isMouseDown = false;

	// Use this for initialization
	void Start ()
	{
		defaultCameraPosition = transform.position;
		defaultCameraRotation = new Vector3(30.0f, 45.0f, 0);
		print(defaultCameraPosition);
	}
		
	void GetMouseDrag() //may not be the most efficient way to do it
	{
		if (Input.GetMouseButtonDown(2) && !isMouseDown)
		{
			startPos = Input.mousePosition;
			isMouseDown = true;
		}

		if (isMouseDown)
		{
			//float magnitudeX = Input.mousePosition.x - startPos.x;

			gameObject.transform.RotateAround (midPoint.transform.position, cameraRotationY, Input.GetAxis("Mouse X") * 2.5f);

			/*if (magnitudeX < 0)
			{
				gameObject.transform.RotateAround (midPoint.transform.position, cameraRotationY, Input.GetAxis("Mouse X") * 2.5f);
			}
			else if (magnitudeX > 0)
			{
				gameObject.transform.RotateAround (midPoint.transform.position, cameraRotationY, Input.GetAxis("Mouse X") * 2.5f); 
			}*/
		}

		if (Input.GetMouseButtonUp(2))
		{
			isMouseDown = false;
		}
	}

	
	// Update is called once per frame
	void Update () 
	{
		GetMouseDrag();

		{ 	//Just for testing
			//These should be manipulated with Touch.input

			if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D))
			{
				gameObject.transform.RotateAround (midPoint.transform.position, cameraRotationY, 2.5f); 
			}

			if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A))
			{
				gameObject.transform.RotateAround (midPoint.transform.position, cameraRotationY, -2.5f); 
			}

			if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W)) 
			{
				if (this.gameObject.transform.eulerAngles.x < maximumRotation) 
				{
					transform.Rotate (Vector3.right * 10.0f * Time.deltaTime);
				}
			}

			if (Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S))
			{
				if (this.gameObject.transform.eulerAngles.x > minimumRotation) 
				{
					transform.Rotate (Vector3.left * 10.0f * Time.deltaTime);
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.R)) //Should have a button for this
		{
			transform.position = defaultCameraPosition;
			transform.eulerAngles = defaultCameraRotation;
		}

	}
}
