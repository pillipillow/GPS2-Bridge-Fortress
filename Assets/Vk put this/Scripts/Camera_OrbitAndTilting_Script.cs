using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_OrbitAndTilting_Script : MonoBehaviour 
{
	private float maximumRotation = 45.0f; // if this is 90, it crosses over to the other side and keeps going
	private float minimumRotation = 5.0f;

	//Vector3 defaultCameraPosition = new Vector3(-5.5f, 0, -5.5f);
	//Vector3 defaultCameraRotation = new Vector3(30.0f, 45.0f, 0);

	Vector3 defaultCameraPosition;
	Vector3 defaultCameraRotation;

	Vector3 cameraRotationY = new Vector3(0, 1f, 0);
	//Vector3 cameraRotationXYZ = new Vector3(0.5f, 0, -0.5f);
	//Vector3 cameraRotationReverseXYZ = new Vector3(-0.5f, 0, -0.5f);

	public GameObject midPointY;


	public List<GameObject> touchList = new List<GameObject>();
	public GameObject[] oldTouches;
	private RaycastHit hit;

	//Mouse
	bool isMouseDown = false;



	void Start () 
	{
		defaultCameraPosition = transform.position;
		defaultCameraRotation = new Vector3(30.0f, 45.0f, 0);
	}



	void GetMouseDrag() //may not be the most efficient way to do it
	{
		if (Input.GetMouseButtonDown(2) && !isMouseDown)
		{
			isMouseDown = true;
		}

		if (isMouseDown)
		{
			gameObject.transform.RotateAround (midPointY.transform.position, cameraRotationY, Input.GetAxis("Mouse X") * 2.5f);

		}

		if (Input.GetMouseButtonUp(2))
		{
			isMouseDown = false;
		}
	}

	void Update () 
	{
		GetMouseDrag();

		{
			if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D))
			{
				gameObject.transform.RotateAround (midPointY.transform.position, cameraRotationY, 2.5f); 
			}

			if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A))
			{
				gameObject.transform.RotateAround (midPointY.transform.position, cameraRotationY, -2.5f); 
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



///////////////////////////////////////////////////////       Possible Shield Throw Camera          ////////////////////////////////////////////////////////

//this versionś difference with the one above is that its position moves as it rotates.
//   -able to achieve 90* angle and more
//   -keeps the focus centered when at topdown view 

/*if (Input.GetKey (KeyCode.UpArrow)) 
			{
				if (this.gameObject.transform.eulerAngles.x < maximumRotation) 
				{
					transform.Rotate (Vector3.right * 10.0f * Time.deltaTime);
					if (transform.position.x < 0 && transform.position.z < 0)
					{
						gameObject.transform.RotateAround(midPointY.transform.position, cameraRotationXYZ, 1f);
					}
					else if (transform.position.x > 0 && transform.position.z > 0)
					{
						gameObject.transform.RotateAround(midPointY.transform.position, cameraRotationXYZ, -1f);
					}
					else if (transform.position.x < 0 && transform.position.z > 0)
					{
						gameObject.transform.RotateAround(midPointY.transform.position, cameraRotationReverseXYZ, 1f);
					}
					else if (transform.position.x > 0 && transform.position.z < 0)
					{
						gameObject.transform.RotateAround(midPointY.transform.position, cameraRotationReverseXYZ, -1f);
					}
					else 
					{
						return;
					}
				}
			}

			if (Input.GetKey (KeyCode.DownArrow)) 
			{
				if (this.gameObject.transform.eulerAngles.x > minimumRotation) 
				{
					transform.Rotate (Vector3.left * 10.0f * Time.deltaTime);
					if (transform.position.x < 0 && transform.position.z < 0)
					{
						gameObject.transform.RotateAround(midPointY.transform.position, cameraRotationXYZ, -1f);
					}
					else if (transform.position.x > 0 && transform.position.z > 0)
					{
						gameObject.transform.RotateAround(midPointY.transform.position, cameraRotationXYZ, 1f);
					}
					else if (transform.position.x < 0 && transform.position.z > 0)
					{
						gameObject.transform.RotateAround(midPointY.transform.position, cameraRotationReverseXYZ, -1f);
					}
					else if (transform.position.x > 0 && transform.position.z < 0)
					{
						gameObject.transform.RotateAround(midPointY.transform.position, cameraRotationReverseXYZ, 1f);
					}
					else
					{
						return;
					}
				}
			}*/
