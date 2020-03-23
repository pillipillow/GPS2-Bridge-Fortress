using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SquireMovementScript : MonoBehaviour {

	public enum MovementState
	{
		STOP = 0,
		MOVING
	};

	public enum InteractionState
	{
		NONE = 0,
		CRANK,
		FLOORPAD,
		ITEM
	};
		
	public NavMeshAgent playerAgent;
	private RaycastHit interactionInfo;
	private GameObject interactedObject;
	private KnightCheckpointScript respawning;

	public MovementState MoveState;
	public InteractionState InteractState;
	public float speed;

	public bool frozen;
	public bool tilt;
	public Rigidbody thisRB;
	public CapsuleCollider thisCC;
	public int randomDirection;

	private LayerMask squireLayer;
	Animator animator;

	// Use this for initialization
	void Start () 
	{
		playerAgent = GetComponent<NavMeshAgent>();	
		squireLayer = LayerMask.GetMask("SquireLayer");
		speed = 10f;
		animator = GetComponentInChildren<Animator>();

		thisRB = GetComponent<Rigidbody>();
		thisCC = GetComponent<CapsuleCollider>();
		randomDirection = Random.Range(0, 11);
		respawning = GameObject.FindGameObjectWithTag("Knight").GetComponent<KnightCheckpointScript>();

		frozen = false;
		tilt = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(tilt)
		{
			animator.speed = 0;

			thisCC.center = new Vector3(-0.27f, 0.9f, -0.1f);
			thisCC.height = 1.68f;

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

			thisRB.AddForce(randomRotX, randomRotY, randomRotZ);

			if(thisRB.velocity.y < -20f)
			{
				respawning.heDead = true;
			}
		}
		else if(frozen)
		{
			playerAgent.enabled = false;
			animator.speed = 0;
		}
		else if(!ThrowItemScript.Variables.timeStop)
		{
			ObjectInteraction();

			if(Input.GetMouseButton(0) && Application.platform == RuntimePlatform.WindowsEditor)
			{
				GetMouseInteraction();
			}
			else if(Application.platform == RuntimePlatform.Android)
			{
				GetTouchInteraction();
			}

			animator.speed = 1;
			AnimationMovement();
		}
		else if(ThrowItemScript.Variables.targetLocked)
		{
			animator.SetTrigger("squireThrow");
			animator.speed = 1;
		}
		else
		{
			animator.speed = 0;
		}
	}

	void GetMouseInteraction()
	{
		Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		if(Physics.Raycast(interactionRay,out interactionInfo, Mathf.Infinity, squireLayer))
		{
			interactedObject = interactionInfo.transform.gameObject;

			MoveState = MovementState.MOVING;
			playerAgent.destination = interactionInfo.point;

			if(interactedObject.tag == "Crank")
			{
				InteractState = InteractionState.CRANK;
			}
			else
			{
				InteractState = InteractionState.NONE;
			}
		}
	}

	void TouchInteraction()
	{
		playerAgent.destination = interactionInfo.point;

		if(interactedObject.CompareTag("Crank"))
		{
			InteractState = InteractionState.CRANK;
		}
		else
		{
			InteractState = InteractionState.NONE;
		}
	}

	void GetTouchInteraction()
	{
		if (Input.touchCount == 1)
		{
			Ray ray;

			foreach(Touch touch in Input.touches)
			{
				switch (touch.phase)
				{
				case TouchPhase.Began:

					ray = Camera.main.ScreenPointToRay(touch.position);

					if(Physics.Raycast(ray, out interactionInfo, Mathf.Infinity, squireLayer))
					{
						interactedObject = interactionInfo.transform.gameObject;
						TouchInteraction();
					}
					break;

				case TouchPhase.Moved:

					ray = Camera.main.ScreenPointToRay(touch.position);

					if(Physics.Raycast(ray, out interactionInfo, Mathf.Infinity, squireLayer))
					{
						interactedObject = interactionInfo.transform.gameObject;
						TouchInteraction();
					}
					break;

				case TouchPhase.Stationary:
					
					ray = Camera.main.ScreenPointToRay(touch.position);

					if(Physics.Raycast(ray, out interactionInfo, Mathf.Infinity, squireLayer))
					{
						interactedObject = interactionInfo.transform.gameObject;
						TouchInteraction();
					}
					break;
				}
			}
		}
	}

	void ObjectInteraction()
	{
		if(playerAgent.pathStatus == NavMeshPathStatus.PathComplete && playerAgent.remainingDistance == 0)
		{
			if(InteractState == InteractionState.CRANK)
			{
				if(!interactedObject.GetComponent<CrankActivateScript>().relatedBridge[0].rotating)
				{
					interactedObject.GetComponent<CrankActivateScript>().activate = true;
					InteractState = InteractionState.NONE;
					interactedObject = null;
				//	SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_TURNINGCRANK);
				}
				else
				{
					InteractState = InteractionState.NONE;
					interactedObject = null;
				}
			}
		}
	}

	//Animation states
	void AnimationMovement()
	{
		if(MoveState== MovementState.MOVING)
		{
			animator.SetBool("squireWalk",true);
		}
		else if(MoveState == MovementState.STOP)
		{
			animator.SetBool("squireWalk",false);
		}
	}
}
