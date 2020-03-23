using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowItemScript : MonoBehaviour
{
	private static ThrowItemScript mVariables;
	public static ThrowItemScript Variables
	{
		get
		{
			if(mVariables == null)
			{
				GameObject tempObject = GameObject.FindWithTag("ThrowManager");
				mVariables = tempObject.GetComponent<ThrowItemScript>();
			}
			return mVariables;
		}
	}

	public enum ItemType
	{
		Shield = 0,
		Flash
	}

	public ItemType ThrowingWhat;

	private bool[] rotateChecks;
	public bool targetLocked;
	public bool isTurning;
	public bool isAiming;
	public bool timeStop;
	public bool finishedThrow;
	public bool preparedField;
//	public bool getTarget;

	public int shieldAmount;
	public int flashAmount;
	public int viewingFloor;

	private float i;
	private float j;
	private float k;
	private float originalX;
	private float originalY;
	private float originalZ;
	private float originalCameraSize;

	public GameObject interactedObject;
	public GameObject shieldPickUp;
	public GameObject flashBomb;
//	private GameObject Shield;
//	private GameObject Flash;

//	private Vector3 shieldPos;
//	private Vector3 flashPos;
//	private Vector3 targetPos;

	private RaycastHit interactionInfo;
	private LayerMask tileLayer;
	private LayerMask enemyLayer;

	private Material interactedMaterial;
	public Material greenMaterial;

	private Vector3 cameraPos;
	private Vector3 cameraRot;
	public Shader transparent;

	public GameObject[] knightLayers;
	public GameObject squireLayer;
	public List<GameObject> floorTiles;
	public KnightCheckpointScript knightCheckpoint;

	public List<GameObject> affectedTileList;
	public GameObject knight;

	public Button[] buttonsNeeded;

	public CameraTouchInputScript cameraScript;

	void Start()
	{
		originalCameraSize = Camera.main.orthographicSize;
		cameraScript = Camera.main.GetComponent<CameraTouchInputScript>();

		ThrowingWhat = ItemType.Shield;

		knightLayers = new GameObject[3];
		knightLayers[0] = GameObject.Find("Knight Layer");
		knightLayers[1] = GameObject.Find("Knight Layer 1");
		knightLayers[2] = GameObject.Find("Knight Layer 2");
		knightCheckpoint = GameObject.FindGameObjectWithTag("Knight").GetComponent<KnightCheckpointScript>();

		buttonsNeeded = new Button[4];
		buttonsNeeded[0] = GameObject.Find("LowerFloorButton").GetComponent<Button>();
		buttonsNeeded[1] = GameObject.Find("UpperFloorButton").GetComponent<Button>();
		buttonsNeeded[2] = GameObject.Find("ShieldUseButton").GetComponent<Button>();
		buttonsNeeded[3] = GameObject.Find("FlashUseButton").GetComponent<Button>();

		knight = GameObject.FindGameObjectWithTag("Knight");

		shieldAmount = 1;
		flashAmount = 2;
		isAiming = false;
		timeStop = false;
		preparedField = false;

		cameraPos = Camera.main.transform.localPosition;
		cameraRot = Camera.main.transform.localEulerAngles;

		rotateChecks = new bool[3];

		for(int r = 0; r < rotateChecks.Length; r ++)
		{
			rotateChecks[r] = false;
		}
		targetLocked = false;
		isTurning = false;

		tileLayer = LayerMask.GetMask("Walkable");
		enemyLayer = LayerMask.GetMask("Enemy");

	//	Shield = GameObject.Find("ShieldSquire");
	//	Shield.SetActive(false);

	//	Flash = GameObject.Find("FlashSquire");
	//	Flash.SetActive(false);

		squireLayer = GameObject.Find("Squire Layer");

		buttonsNeeded[0].gameObject.SetActive(false);
		buttonsNeeded[1].gameObject.SetActive(false);

		GameObject[] gameObjectArray = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];

		for(int a = 0; a < gameObjectArray.Length; a ++)
		{
			if(gameObjectArray[a].layer == 8 && gameObjectArray[a].tag != "RotatingBridge" && gameObjectArray[a].tag != "EndPoint")
			{
				floorTiles.Add(gameObjectArray[a]);
				gameObjectArray[a].GetComponent<MeshRenderer>().material.shader = transparent;
			}
		}
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.B))
		{
			BeginShieldAim();
		}

		BeginAim();

		if(finishedThrow)
		{
			finishedThrow = false;

			if(flashAmount > 0)
			{
				buttonsNeeded[3].interactable = true;
			}
			if(shieldAmount > 0)
			{
				buttonsNeeded[2].interactable = true;
			}

			isAiming = false;
			isTurning = true;
			targetLocked = false;
			isAiming = false;
			timeStop = false;
		}
	}

	void RevertToNormal()
	{
		cameraScript.cameraSize = originalCameraSize;
		Camera.main.orthographicSize = originalCameraSize;
	//	getTarget = false;

		preparedField = false;
		knightLayers[0].SetActive(true);

		if(knightLayers[1] != null)
		{
			knightLayers[1].SetActive(true);
		}
		if(knightLayers[2] != null)
		{
			knightLayers[2].SetActive(true);
		}
		squireLayer.SetActive(true);

		for(int a = affectedTileList.Count - 1; a > -1; a --)
		{
			MeshRenderer objMesh = affectedTileList[a].transform.GetComponent<MeshRenderer>();
			Color objColor = objMesh.material.color;
			objMesh.material.color = new Color(objColor.r, objColor.g, objColor.b, 1.0f);
			affectedTileList.RemoveAt(a);
		}

		knight.transform.parent = null;
	}

	public void BeginShieldAim()
	{
		if(shieldAmount > 0)
		{
			Camera.main.transform.localPosition = cameraPos;
			Camera.main.transform.localEulerAngles = cameraRot;

			if(!isAiming && !isTurning)
			{
				isTurning = true;
				isAiming = true;
				ThrowingWhat = ItemType.Shield;
	//			Shield.SetActive(true);
	//			shieldPos = Shield.transform.position;

				buttonsNeeded[3].interactable = false;
			}
			else if(isAiming && !isTurning)
			{
				isTurning = true;
				finishedThrow = false;
	//			Shield.SetActive(false);
				targetLocked = false;
				isAiming = false;
				timeStop = false;

				if(flashAmount > 0)
				{
					buttonsNeeded[3].interactable = true;
				}

				RevertToNormal();
			}
		}
	}

	public void BeginFlashAim()
	{
		if(flashAmount > 0)
		{
			Camera.main.transform.localPosition = cameraPos;
			Camera.main.transform.localEulerAngles = cameraRot;

			if(!isAiming && !isTurning)
			{
				isTurning = true;
				isAiming = true;
				ThrowingWhat = ItemType.Flash;
	//			Flash.SetActive(true);
	//			flashPos = Flash.transform.position;

				buttonsNeeded[2].interactable = false;
			}
			else if(isAiming && !isTurning)
			{
				isTurning = true;
				finishedThrow = false;
	//			Flash.SetActive(false);
				targetLocked = false;
				isAiming = false;
				timeStop = false;

				if(shieldAmount > 0)
				{
					buttonsNeeded[2].interactable = true;
				}

				RevertToNormal();
			}
		}
	}

	public void ViewLowerFloor()
	{
		if(rotateChecks[0] && rotateChecks[1] && rotateChecks[2] && isAiming && viewingFloor > 0)
		{
			viewingFloor -= 1;

			if(viewingFloor == 1)
			{
				knightLayers[2].SetActive(false);
				knightLayers[1].SetActive(true);
				knightLayers[0].SetActive(false);
			}
			else if(viewingFloor == 0)
			{
				if(knightLayers[2] != null)
				{
					knightLayers[2].SetActive(false);
				}
				knightLayers[1].SetActive(false);
				knightLayers[0].SetActive(true);
			}
		}
	}

	public void ViewUpperFloor()
	{
		if(rotateChecks[0] && rotateChecks[1] && rotateChecks[2] && isAiming && viewingFloor < 2)
		{
			viewingFloor += 1;

			if(viewingFloor == 2)
			{
				knightLayers[2].SetActive(true);
				knightLayers[1].SetActive(false);
				knightLayers[0].SetActive(false);
			}
			else if(viewingFloor == 1)
			{
				if(knightLayers[2] != null)
				{
					knightLayers[2].SetActive(false);
				}

				knightLayers[1].SetActive(true);
				knightLayers[0].SetActive(false);
			}
		}
	}

	void BeginAim()
	{
		if(isAiming)
		{
			if(!rotateChecks[0])
			{
				i += 60 * Time.deltaTime;

				transform.eulerAngles = new Vector3(i, originalY, originalZ);
				originalX = transform.eulerAngles.x;

				if(i > 38)
				{
					rotateChecks[0] = true;
					originalX = 38.0f;
				}
			}

			if(!rotateChecks[1])
			{
				j -= 20 * Time.deltaTime;

				transform.eulerAngles = new Vector3(originalX, j, originalZ);
				originalY = transform.eulerAngles.y;

				if(j < -9)
				{
					rotateChecks[1] = true;
					originalY = -9.0f;
				}
			}

			if(!rotateChecks[2])
			{
				k -= 80 * Time.deltaTime;

				transform.eulerAngles = new Vector3(originalX, originalY, k);
				originalZ = transform.eulerAngles.z;

				if(k < -51)
				{
					rotateChecks[2] = true;
					originalZ = -51.0f;
				}
			}

			if(rotateChecks[0] && rotateChecks[1] && rotateChecks[2])
			{
				transform.eulerAngles = new Vector3(originalX, originalY, originalZ);
				timeStop = true;
				isTurning = false;

				if(!preparedField)
				{
					preparedField = true;

					if(Camera.main.orthographicSize > 19)
					{
						Camera.main.orthographicSize = 19;
						cameraScript.cameraSize = 19;
					}

					for(int a = 0; a < floorTiles.Count; a ++)
					{
						RaycastHit hit;
						if(Physics.Raycast(floorTiles[a].transform.position - floorTiles[a].transform.up * 1.5f, Vector3.down, out hit, Mathf.Infinity, tileLayer))
						{
							affectedTileList.Add(hit.transform.gameObject);
							Color hitColor = hit.transform.GetComponent<MeshRenderer>().material.color;
							hit.transform.GetComponent<MeshRenderer>().material.color = new Color(hitColor.r, hitColor.g, hitColor.b, 0.2f);
						}
					}

					squireLayer.SetActive(false);

					if(knightCheckpoint.checkPointNum == 0)
					{
						knight.transform.parent = knightLayers[0].transform;
						if(knightLayers[1] != null)
						{
							knightLayers[1].SetActive(false);
						}
						if(knightLayers[2] != null)
						{
							knightLayers[2].SetActive(false);
						}
						viewingFloor = 0;
					}
					else if(knightCheckpoint.checkPointNum == 1)
					{
						knight.transform.parent = knightLayers[1].transform;
						knightLayers[0].SetActive(false);
						if(knightLayers[2] != null)
						{
							knightLayers[2].SetActive(false);
						}
						viewingFloor = 1;
					}
					else if(knightCheckpoint.checkPointNum == 2)
					{
						knight.transform.parent = knightLayers[2].transform;
						knightLayers[0].SetActive(false);
						knightLayers[1].SetActive(false);
						viewingFloor = 2;
					}

					buttonsNeeded[0].gameObject.SetActive(true);
					buttonsNeeded[1].gameObject.SetActive(true);
				}
			}
		}
		else
		{
			buttonsNeeded[0].gameObject.SetActive(false);
			buttonsNeeded[1].gameObject.SetActive(false);

			if(rotateChecks[0])
			{
				i -= 60 * Time.deltaTime;

				transform.eulerAngles = new Vector3(i, originalY, originalZ);
				originalX = transform.eulerAngles.x;

				if(i < 0)
				{
					rotateChecks[0] = false;
					originalX = 0.0f;
				}
			}

			if(rotateChecks[1])
			{
				j += 20 * Time.deltaTime;

				transform.eulerAngles = new Vector3(originalX, j, originalZ);
				originalY = transform.eulerAngles.y;

				if(j > 0)
				{
					rotateChecks[1] = false;
					originalY = 0.0f;
				}
			}

			if(rotateChecks[2])
			{
				k += 80 * Time.deltaTime;

				transform.eulerAngles = new Vector3(originalX, originalY, k);
				originalZ = transform.eulerAngles.z;

				if(k > 0)
				{
					rotateChecks[2] = false;
					originalZ = 0.0f;
				}
			}

			if(!rotateChecks[0] && !rotateChecks[1] && !rotateChecks[2])
			{
				transform.eulerAngles = new Vector3(originalX, originalY, originalZ);
				isTurning = false;

				if(interactedObject != null)
				{
					interactedObject.GetComponent<MeshRenderer>().material = interactedMaterial;
					interactedMaterial = null;
					interactedObject = null;
				}
			}
		}

		TargetTile();
	}

	void TargetTile()
	{
		if(rotateChecks[0] && rotateChecks[1] && rotateChecks[2] && isAiming)
		{
			if(Input.GetMouseButtonDown(0) && !targetLocked && Application.platform == RuntimePlatform.WindowsEditor)
			{
				GetMouseInteraction();
			}
			if(!targetLocked && Application.platform == RuntimePlatform.Android)
			{
				GetTouchInteraction();
			}

			if(targetLocked)
			{
				Launch();
			}
			ManageButtons();
		}
	}

	void ManageButtons()
	{
		if(viewingFloor == 2)
		{
			buttonsNeeded[1].interactable = false;
		}
		else if(viewingFloor == 1)
		{
			if(knightLayers[2] == null)
			{
				buttonsNeeded[1].interactable = false;
			}
			else
			{
				buttonsNeeded[1].interactable = true;
			}

			buttonsNeeded[0].interactable = true;
		}
		else if(viewingFloor == 0)
		{
			if(knightLayers[1] != null)
			{
				buttonsNeeded[1].interactable = true;
			}
			else
			{
				buttonsNeeded[1].interactable = false;
			}

			buttonsNeeded[0].interactable = false;
		}
	}

	void GetTouchInteraction()
	{
		if(Input.touches.Length == 1)
		{
			if(Input.GetTouch(0).phase == TouchPhase.Began)
			{
				Ray interactionRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

				if(Physics.Raycast(interactionRay, out interactionInfo, Mathf.Infinity, tileLayer))
				{
					if(interactedObject != interactionInfo.transform.gameObject && !interactionInfo.transform.CompareTag("EndPoint"))
					{
						if(interactedObject != null)
						{
							interactedObject.GetComponent<MeshRenderer>().material = interactedMaterial;
						}

						interactedObject = interactionInfo.transform.gameObject;
						interactedMaterial = interactedObject.GetComponent<MeshRenderer>().material;
						interactedObject.GetComponent<MeshRenderer>().material = greenMaterial;
					}
					else if(interactedObject != null && interactedObject == interactionInfo.transform.gameObject)
					{
						targetLocked = true;
					}
				}
			}
		}
	}

	void GetMouseInteraction()
	{
		Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		if(Physics.Raycast(interactionRay,out interactionInfo, Mathf.Infinity, tileLayer))
		{
			if(interactedObject != interactionInfo.transform.gameObject && !interactionInfo.transform.CompareTag("EndPoint"))
			{
				if(interactedObject != null)
				{
					interactedObject.GetComponent<MeshRenderer>().material = interactedMaterial;
				}

				bool pass = true;

				for(int a = 0; a < affectedTileList.Count; a ++)
				{
					if(affectedTileList[a] == interactionInfo.transform.gameObject)
					{
						pass = false;
					}
				}

				if(pass)
				{
					interactedObject = interactionInfo.transform.gameObject;
					interactedMaterial = interactedObject.GetComponent<MeshRenderer>().material;
					interactedObject.GetComponent<MeshRenderer>().material = greenMaterial;
				}
			}
			else if(interactedObject != null && interactedObject == interactionInfo.transform.gameObject)
			{
				targetLocked = true;
			}
		}
	}

	void Launch()
	{
		if(ThrowingWhat == ItemType.Shield)
		{
			RevertToNormal();
			targetLocked = false;
			finishedThrow = true;
			shieldAmount -= 1;

			bool spawnShield = true;
			RaycastHit hit;

			if(shieldAmount <= 0)
			{
				buttonsNeeded[2].interactable = false;
			}

			if(Physics.Raycast(interactedObject.transform.position, Vector3.up, out hit, 8.0f, enemyLayer) ||
				Physics.Raycast(interactedObject.transform.position, Vector3.up, out hit, 8.0f, tileLayer))
			{
				if(hit.transform.CompareTag("Enemy"))
				{
					if(hit.transform.GetComponent<EnemyAIScript>().health == 1)
					{
						hit.transform.GetComponent<EnemyAIScript>().tilt = true;
						spawnShield = false;
					}
				}
				else if(hit.transform.CompareTag("FireWall"))
				{
					ParticleSystem fireWall = hit.transform.parent.GetComponentInChildren<ParticleSystem>();

					if(fireWall.isPlaying)
					{
						spawnShield = false;
					}
				}
			}

			if(spawnShield)
			{
				GameObject shieldObj = Instantiate(shieldPickUp);
				shieldObj.transform.position = new Vector3(interactedObject.transform.position.x, interactedObject.transform.position.y + 0.5f, interactedObject.transform.position.z);
			}


			/*
			if(Shield.transform.position != targetPos)
			{
				Shield.transform.position = Vector3.MoveTowards(Shield.transform.position, targetPos, 10.0f * Time.deltaTime);
			}
			else
			{
				RevertToNormal();

				targetLocked = false;

				finishedThrow = true;
				shieldAmount -= 1;

				if(shieldAmount <= 0)
				{
					buttonsNeeded[2].interactable = false;
				}

				RaycastHit hit;
				bool spawnShield = true;

				if(Physics.Raycast(Shield.transform.position, Vector3.down, out hit, Mathf.Infinity, enemyLayer) ||
					Physics.Raycast(Shield.transform.position, Vector3.down, out hit, Mathf.Infinity, tileLayer))
				{
					if(hit.transform.CompareTag("Enemy"))
					{
						if(hit.transform.GetComponent<EnemyAIScript>().health == 1)
						{
							hit.transform.GetComponent<EnemyAIScript>().tilt = true;
							spawnShield = false;
						}
					}
					else if(hit.transform.CompareTag("FireWall"))
					{
						ParticleSystem fireWall = hit.transform.parent.GetComponentInChildren<ParticleSystem>();

						if(fireWall.isPlaying)
						{
							spawnShield = false;
						}
					}
				}

				if(spawnShield)
				{
					
				}

				Shield.transform.position = new Vector3(shieldPos.x, shieldPos.y, shieldPos.z);
				Shield.SetActive(false);
			}*/
		}
		else if(ThrowingWhat == ItemType.Flash)
		{
			RevertToNormal();

			targetLocked = false;

			finishedThrow = true;
			flashAmount -= 1;

			if(flashAmount <= 0)
			{
				buttonsNeeded[3].interactable = false;
			}

			GameObject flashObj = Instantiate(flashBomb);
			flashObj.transform.position = new Vector3(interactedObject.transform.position.x, interactedObject.transform.position.y + 0.5f, interactedObject.transform.position.z);

		/*	if(!getTarget)
			{
				getTarget = true;
				targetPos = new Vector3(interactedObject.transform.position.x, Flash.transform.position.y, interactedObject.transform.position.z);
			}

			if(Flash.transform.position != targetPos)
			{
				Flash.transform.position = Vector3.MoveTowards(Flash.transform.position, targetPos, 10.0f * Time.deltaTime);
			}
			else
			{
				

				Flash.transform.position = new Vector3(flashPos.x, flashPos.y, flashPos.z);
				Flash.SetActive(false);
			}*/
		}
	}
}
