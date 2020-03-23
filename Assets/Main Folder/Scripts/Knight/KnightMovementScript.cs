using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*HT Psuedocode for player raycasting
	Loop
`		Raycast all directions
		Move Towards a direction
	 	Stop at edge
*/

//HT Script used for player movement
public class KnightMovementScript : MonoBehaviour 
{
	//Jun: To make VK's coding life for the attack script easier
	public enum MovementState
	{
		CanMove = 0,
		StopMove,
		Stunned
	}

	public MovementState MoveState;

	//HT for hit detection purposes
	private RaycastHit hit;
	//HT Raycasted hit distance
	private float raycastDistance;
	//HT Speed of the player
	public float speed;
	//HT To record the base speed of the player
	private float baseSpeed;
	//HT To check if the player is moving
	public bool isMoving;
	//HT To determine a direction to raycast
	public Vector3 raycastDirection;
	// To check in this order: Front, Left, Right, Back
	public int raycastCheck;

	// Jun: To ensure the ray will only detect this specific layer
	private LayerMask floorLayer;
	private LayerMask wallLayer;
	private LayerMask stairsLayer;
	private LayerMask flashLayer;
	public LayerMask endLayer;

	// Jun: To ensure that it doesn't spin around endlessly while waiting for the player to connect the first platform
	public bool proceedMovement;

	//Jun: For rotating platforms
	public bool rotating;
	private bool pass;

	// For Stun Mechanic
	public float stunTimer;

	//Jun: To get animations running
	private Animation thisAni;

	//Jun: New model = new bugs. This is your fix.
	private Vector3 slightlyUp;
	private Vector3 slightlyDown;
	private Vector3 startingRayPos;
	public GameObject knightBone;
	private Vector3 knightBonePos;
	public GameObject sword;
	private Vector3 swordPos;
	private Vector3 swordRotation;

	public int checkTimes;

	public bool tilt;
	private Rigidbody thisRB;
	private CapsuleCollider thisCC;
	private int randomDirection;

	void Start ()
	{
		MoveState = MovementState.CanMove;
		raycastDirection = transform.forward;
		isMoving = false;
		rotating = false;
		raycastDistance = 3;
		speed = 3;
		baseSpeed = speed;
		raycastCheck = 1;
		floorLayer = LayerMask.GetMask("Walkable");
		wallLayer = LayerMask.GetMask("Wall");
		endLayer = LayerMask.GetMask("EndPoint");
		stairsLayer = LayerMask.GetMask("Stairs");
		flashLayer = LayerMask.GetMask("Flash");
		thisAni = GetComponent<Animation>();

		checkTimes = 0;

		stunTimer = -1.0f;
		slightlyUp = new Vector3(0, 1, 0);
		slightlyDown = new Vector3(0, -0.5f, 0);

		knightBonePos = knightBone.transform.localPosition;
		swordPos = sword.transform.localPosition;
		swordRotation = sword.transform.localEulerAngles;

		tilt = false;
		thisRB = GetComponent<Rigidbody>();
		thisCC = GetComponent<CapsuleCollider>();
		randomDirection = Random.Range(0, 11);
	}

	void Update ()
	{
		/*if (!isMoving)
		{
			//SoundManagerScript.Instance.StopAllLoopingSFX();
		}*/

		if(!ThrowItemScript.Variables.timeStop && !tilt)
		{
			raycastDirection = transform.forward;

			startingRayPos = transform.position + slightlyUp;

			if(MoveState == MovementState.CanMove)
			{
				if(!proceedMovement)
				{
					if(!thisAni.IsPlaying("KnightIdleAnimation"))
					{
						thisAni.Stop();
						knightBone.transform.localPosition = knightBonePos;
						sword.transform.localPosition = swordPos;
						sword.transform.localEulerAngles = swordRotation;
						thisAni.Play(animation:"KnightIdleAnimation");
					}
					StartMoving();
				}
				else
				{
					//HT Raycast check
					if (!isMoving)
					{
						CheckNextDirection();

						//SoundManagerScript.Instance.StopLoopingSFX(AudioClipID.SFX_KNIGHTFOOTSTEPS);
					}
					//HT Player Moving
					else
					{
						MoveForward();
					}
				}
			}

			if(stunTimer > 0.0f)
			{
				stunTimer -= 1.0f * Time.deltaTime;	
				MoveState = MovementState.Stunned;
				thisAni.Stop();

				//SoundManagerScript.Instance.StopLoopingSFX(AudioClipID.SFX_KNIGHTFOOTSTEPS);
			}
			else if(MoveState != MovementState.StopMove && stunTimer <= 0.0f)
			{
				MoveState = MovementState.CanMove;
			}
		}
		else if(ThrowItemScript.Variables.timeStop && !tilt)
		{
			if(thisAni.isPlaying)
			{
				thisAni.Stop();
			}
		}
		else if(!ThrowItemScript.Variables.timeStop && tilt)
		{
			if(thisAni.isPlaying)
			{
				thisAni.Stop();
			}

			thisRB.freezeRotation = false;
			int randomRotX = Random.Range(-1, 3);

			if(randomDirection > 5)
			{
				randomRotX = -randomRotX;
			}

			int randomRotY = Random.Range(-1, 3);

			if(randomDirection > 5)
			{
				randomRotY = -randomRotY;
			}

			int randomRotZ = Random.Range(-1, 3);

			if(randomDirection > 5)
			{
				randomRotZ = -randomRotZ;
			}

			thisCC.height = 0.327f;
			thisCC.center = new Vector3(thisCC.center.x, 0.1772377f, thisCC.center.z);
			thisRB.AddForce(randomRotX, randomRotY, randomRotZ);
		}
	}

	void StartMoving()
	{
		Debug.DrawRay(startingRayPos + raycastDirection, Vector3.down, Color.green);
		raycastDirection = transform.forward;

		if (Physics.Raycast(startingRayPos + raycastDirection, Vector3.down , raycastDistance, floorLayer) || 
			Physics.Raycast(startingRayPos + raycastDirection, Vector3.down , out hit, raycastDistance, endLayer) || 
			Physics.Raycast(startingRayPos + raycastDirection, Vector3.down , out hit, raycastDistance, stairsLayer))
		{
			proceedMovement = true;
			isMoving = true;
		}
	}

	void CheckRotate()
	{
		switch (raycastCheck)
		{
		case 1:
			transform.RotateAround(transform.position, Vector3.up, 0);
			raycastCheck++;
			break;
		case 2:
			transform.RotateAround(transform.position, Vector3.up, -90);
			raycastCheck++;
			break;
		case 3:
			transform.RotateAround(transform.position, Vector3.up, 180);
			raycastCheck++;
			break;
		case 4:
			transform.RotateAround(transform.position, Vector3.up, 90);
			break;
		}
	}

	//Jun: Make sure it doesnt fall
	void DontFallOff()
	{
		int angle = Mathf.RoundToInt (transform.localEulerAngles.y);
		float originalX = transform.localEulerAngles.x;
		float originalZ = transform.localEulerAngles.z;

		if(angle < 90)
		{
			transform.localEulerAngles = new Vector3(originalX, 0, originalZ);
		}
		else if(angle < 180)
		{
			transform.localEulerAngles = new Vector3(originalX, 90, originalZ);
		}
		else if(angle < 270)
		{
			transform.localEulerAngles = new Vector3(originalX, 180, originalZ);
		}
		else if(angle < 360)
		{
			transform.localEulerAngles = new Vector3(originalX, 270, originalZ);
		}
		else
		{
			transform.localEulerAngles = new Vector3(originalX, 360, originalZ);
		}
	}

	void CheckNextDirection()
	{
		if(checkTimes <= 5)
		{
			CheckRotate();

			if(rotating)
			{
				DontFallOff();
			}

			raycastDirection = transform.forward;

			//HT if a collision is detected
			if(Physics.Raycast(startingRayPos + raycastDirection, Vector3.down , raycastDistance, floorLayer) ||
				Physics.Raycast(startingRayPos + raycastDirection, Vector3.down , raycastDistance, endLayer) || 
				Physics.Raycast(startingRayPos + raycastDirection, Vector3.down , raycastDistance, stairsLayer))
			{
				isMoving = true;
				checkTimes = 0;
			}
			else
			{
				checkTimes += 1;
			}
		}
		else
		{
			if(!thisAni.IsPlaying("KnightIdleAnimation"))
			{
				thisAni.Stop();
				knightBone.transform.localPosition = knightBonePos;
				sword.transform.localPosition = swordPos;
				sword.transform.localEulerAngles = swordRotation;
				thisAni.Play(animation:"KnightIdleAnimation");
			}

			if(Physics.Raycast(startingRayPos + transform.forward, Vector3.down , raycastDistance, floorLayer) ||
				Physics.Raycast(startingRayPos - transform.forward, Vector3.down , raycastDistance, floorLayer) || 
				Physics.Raycast(startingRayPos + transform.right, Vector3.down , raycastDistance, floorLayer) ||
				Physics.Raycast(startingRayPos - transform.right, Vector3.down , raycastDistance, floorLayer))
			{
				isMoving = true;
				checkTimes = 0;
			}
		}
	}

	void CheckWhileRotating()
	{
		int angle = Mathf.RoundToInt (transform.localEulerAngles.y);
		float originalX = transform.localEulerAngles.x;
		float originalZ = transform.localEulerAngles.z;

		if(angle <= 44 && angle >= -44)
		{
			transform.localEulerAngles = new Vector3(originalX, 0, originalZ);
		}
		else if(angle <= 134 && angle >= 46)
		{
			transform.localEulerAngles = new Vector3(originalX, 90, originalZ);
		}
		else if(angle <= 224 && angle >= 136)
		{
			transform.localEulerAngles = new Vector3(originalX, 180, originalZ);
		}
		else if(angle <= 314 && angle >= 226)
		{
			transform.localEulerAngles = new Vector3(originalX, 270, originalZ);
		}
		else if(angle >= 316 && angle <= -46)
		{
			transform.localEulerAngles = new Vector3(originalX, 360, originalZ);
		}
	}

	void MoveForward()
	{
		//HT Raycast to detect Flash Bomb
		if (Physics.Raycast(startingRayPos + slightlyDown, transform.forward, out hit, raycastDistance, flashLayer))
		{
			//HT to store the flashbomb's remaining lifetime temporarly
			float _lifetimeLeft = hit.collider.gameObject.GetComponent<FlashBombItemScript>().lifetime - hit.collider.gameObject.GetComponent<FlashBombItemScript>().timeElapsed;
			//HT This is to make sure the flash bombs stuns only when it has started or before it reaches its end phase (The final second)
			if(_lifetimeLeft < 5.0f && _lifetimeLeft > 1.0f) 
			{
				//HT stunned as much as the remaining lifetime of the flashbomb
				stunTimer = _lifetimeLeft;
			}
		}

		//HT if there is ground on the front
		if (Physics.Raycast(startingRayPos + raycastDirection / 1.5f, Vector3.down , out hit, raycastDistance, floorLayer) ||
			Physics.Raycast(startingRayPos + raycastDirection / 1.5f, Vector3.down , out hit, raycastDistance, endLayer) ||
			Physics.Raycast(startingRayPos + raycastDirection / 1.5f, Vector3.down , out hit, raycastDistance, stairsLayer) ||
			Physics.Raycast(startingRayPos + raycastDirection, Vector3.down , out hit, raycastDistance, floorLayer) ||
			Physics.Raycast(startingRayPos + raycastDirection, Vector3.down , out hit, raycastDistance, endLayer) ||
			Physics.Raycast(startingRayPos + raycastDirection, Vector3.down , out hit, raycastDistance, stairsLayer))
		{
			transform.Translate(Vector3.forward * speed * Time.deltaTime);

			if(!thisAni.IsPlaying("KnightWalkAnimation"))
			{
				thisAni.Stop();
				knightBone.transform.localPosition = knightBonePos;
				sword.transform.localPosition = swordPos;
				sword.transform.localEulerAngles = swordRotation;
				thisAni.Play(animation:"KnightWalkAnimation");
			}
			CheckWhileRotating();
			CheckStairs();

			/*if (Time.timeScale > 0 && isMoving && !ThrowItemScript.Variables.timeStop)
			{
				//SoundManagerScript.Instance.PlayLoopingSFX(AudioClipID.SFX_KNIGHTFOOTSTEPS);
			}*/
		}
		else
		{
			raycastCheck = 1;
			isMoving = false;
		}

		if(Physics.Raycast(startingRayPos, raycastDirection, 1f, wallLayer))
		{
			raycastCheck = 3;
			isMoving = false;
		}
	}

	void ResetParenthood()
	{
		if(transform.parent != null && !ThrowItemScript.Variables.timeStop)
		{
			transform.parent = null;
			rotating = false;
			DontFallOff();
			raycastDirection = transform.forward;
		}
	}

	void OnTriggerStay(Collider coll)
	{
		if(coll.CompareTag("RotatingBridge"))
		{
			if(coll.GetComponent<PlatformRotationScript>().rotating)
			{
				transform.parent = coll.transform;
				rotating = true;
			}
			else
			{
				ResetParenthood();
			}
		}

		if(coll.CompareTag("FlashBomb"))
		{
			float lifetimeLeft = coll.gameObject.GetComponent<FlashBombItemScript>().lifetime - 
				coll.gameObject.GetComponent<FlashBombItemScript>().timeElapsed;
			if(lifetimeLeft < 5.0f && lifetimeLeft > 1.0f) 
			{
				stunTimer = lifetimeLeft;
			}
		}
	}

	void OnTriggerExit(Collider coll)
	{
		ResetParenthood();
	}

	void CheckStairs()
	{
		//HT this function is called by "MoveFoward()"
		//HT there is an incoming stairs
	/*	if (hit.collider.gameObject.tag == "Stairs")
		{
			GameObject stairs = hit.collider.gameObject;
			//HT Checks for Stair's Type
			if (stairs.GetComponent<StairsScript>().stairsType == StairsScript.StairsType.UPWARDS)
			{
				//HT Checks for Stair's Height
				float stairsHeight = stairs.GetComponent<StairsScript>().height;
				if(speed == 0)
				{
					speed = baseSpeed;
					transform.Translate(Vector3.forward * stairsHeight); //! jumps accordimg to the height of the stairs
				}
			}
		} */
	}
}