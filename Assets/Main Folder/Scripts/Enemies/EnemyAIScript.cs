using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIScript : MonoBehaviour
{
	public enum MovementState
	{
		CanMove = 0,
		StopMove
	}

	public enum BehaviourState
	{
		Patrolling = 0,
		Chase,
		Bumped
	}

	public enum EnemyClass
	{
		Normal = 0,
		Armored,
		Phantom,
		Swarm
	}

	public enum ChangeBehavior
	{
		ItChanges = 0,
		DoesNot
	}

	public MovementState MoveState;
	public MovementState PrevState;
	public BehaviourState BehaveState;
	public BehaviourState PrevBehaviour;
	public EnemyClass EnemyType;
	public ChangeBehavior ChangeableState;

	public int raycastCheck;
	public int lostCount;
	public int patrolCheck;
	private int randomDirection;

	public float bumpTurn;
	private float raycastDistance;
	private float originalRotation;
	private float timer;
	public float bumpTimer;
	private float speed;
	public float health;
	public float stunTimer;
	private float swarmDeathTimer;
	private float swarmDistanceX;
	private float swarmDistanceZ;

	public bool tilt;
	public bool dontMove;

	private Vector3 raycastDirection;

	private LayerMask floorLayer;
	private LayerMask wallLayer;
	private LayerMask knightLayer;
	private LayerMask enemyLayer;
	private LayerMask stairsLayer;
	private LayerMask flashLayer;

	public bool isMoving;
	private bool rotating;
	private bool checkedSides;
	private bool hasTurned;
	private bool swarmDeath;
	private bool swarmGetTarget;

	public Transform[] patrolPoints;
	private CapsuleCollider thisCC;

	public Transform targetPos;
	public Transform parent;
	private Rigidbody thisRB;

	public List<Material> materialList;
	public GameObject shieldItem;
	public GameObject swordItem;
	public KnightAttackScript KnightHealth;
	public KnightMovementScript KnightScript;
	public Animation thisAni;
	public EnemySwarmScript[] swarmScripts;

	private MeshRenderer[] thisMR;

	void Start ()
	{
		thisRB = GetComponent<Rigidbody>();

		parent = transform.parent;

		PrevState = MovementState.CanMove;
		MoveState = MovementState.StopMove;

		PrevBehaviour = BehaviourState.Patrolling;

		floorLayer = LayerMask.GetMask("Walkable");
		wallLayer = LayerMask.GetMask("Wall");
		knightLayer = LayerMask.GetMask("Knight");
		enemyLayer = LayerMask.GetMask("Enemy");
		flashLayer = LayerMask.GetMask("Flash");

		KnightHealth = GameObject.FindGameObjectWithTag("Knight").GetComponent<KnightAttackScript>();
		KnightScript = GameObject.FindGameObjectWithTag("Knight").GetComponent<KnightMovementScript>();

		thisCC = GetComponent<CapsuleCollider>();
		randomDirection = Random.Range(0, 11);

		thisMR = GetComponentsInChildren<MeshRenderer>();

		if(EnemyType == EnemyClass.Normal)
		{
			health = 1;
			thisAni = transform.GetChild(0).GetComponent<Animation>();
			for(int i = 0; i < thisMR.Length; i++)
			{
				if(thisMR[i].name != shieldItem.name && thisMR[i].name != swordItem.name)
				{
					thisMR[i].material = materialList[0];
				}
			}

			shieldItem.SetActive(false);
		}
		else if(EnemyType == EnemyClass.Armored)
		{
			health = 2;
			thisAni = transform.GetChild(0).GetComponent<Animation>();
			for(int i = 0; i < thisMR.Length; i++)
			{
				if(thisMR[i].name != shieldItem.name && thisMR[i].name != swordItem.name)
				{
					thisMR[i].material = materialList[1];
				}
			}
		}
		else if(EnemyType == EnemyClass.Phantom)
		{
			health = 999;
			transform.GetChild(0).gameObject.SetActive(false);
			transform.GetChild(1).gameObject.SetActive(true);
			thisAni = transform.GetChild(1).GetComponent<Animation>();
			for(int i = 0; i < thisMR.Length; i++)
			{
				if(thisMR[i].name != shieldItem.name && thisMR[i].name != swordItem.name)
				{
					thisMR[i].material = materialList[2];
				}
			}
		}
		else if(EnemyType == EnemyClass.Swarm)
		{
			health = 999;
			transform.GetChild(0).gameObject.SetActive(false);
			transform.GetChild(1).gameObject.SetActive(false);
			transform.GetChild(2).gameObject.SetActive(true);
			swarmScripts = GetComponentsInChildren<EnemySwarmScript>();
			thisCC.center = new Vector3(0.0f, -0.05f, 0.02f);
			thisCC.height = 0.25f;
			thisCC.radius = 0.03f;
			thisRB.useGravity = false;
			patrolCheck = 0;
			targetPos = patrolPoints[0];
			swarmGetTarget = false;

			swarmDistanceX = Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(targetPos.transform.position.x));
			swarmDistanceZ = Mathf.Abs(Mathf.Abs(transform.position.z) - Mathf.Abs(targetPos.transform.position.z));
		}

		originalRotation = Mathf.RoundToInt(transform.eulerAngles.y);
		raycastDirection = transform.forward;
		hasTurned = false;
		checkedSides = false;
		isMoving = false;
		rotating = false;
		swarmDeath = false;
		raycastDistance = 3;
		speed = 3;

		if(EnemyType == EnemyClass.Phantom)
		{
			speed = 2;
		}

		raycastCheck = 1;
		lostCount = 0;
		stunTimer = -1.0f;
		timer = -1.0f;
		swarmDeathTimer = 0.0f;
	}

	void Update ()
	{
		if(!ThrowItemScript.Variables.timeStop && !tilt)
		{
			if(EnemyType != EnemyClass.Swarm)
			{
				if(BehaveState == BehaviourState.Chase)
				{
					targetPos = GameObject.FindGameObjectWithTag("Knight").transform;
				}
				else if(BehaveState == BehaviourState.Patrolling)
				{
					if(patrolPoints.Length == 0 || (patrolPoints[0] == null && patrolPoints[1] == null))
					{
						lostCount = 5;
					}
					else if(patrolCheck == 0 && patrolPoints[1] != null)
					{
						targetPos = patrolPoints[1];
					}
					else if(patrolCheck == 1 && patrolPoints[0] != null)
					{
						targetPos = patrolPoints[0];
					}
				}

				if(MoveState == MovementState.CanMove && BehaveState != BehaviourState.Bumped)
				{
					if(!dontMove)
					{
						if(!isMoving)
						{
							CheckNextDirection();
						}
						else
						{
							MoveForward();
						}
					}

				}
				else if(MoveState == MovementState.CanMove && BehaveState == BehaviourState.Bumped)
				{
					BumpedIntoFriend();
				}

				if(timer > 0.0f)
				{
					timer -= Time.deltaTime * 1.0f;
				}

				RaycastHit hit;

				if(Physics.Raycast(transform.position, transform.forward, out hit, 5.0f, flashLayer))
				{
					float lifetimeLeft = hit.collider.gameObject.GetComponent<FlashBombItemScript>().lifetime - 
						hit.collider.gameObject.GetComponent<FlashBombItemScript>().timeElapsed;
					if(lifetimeLeft < 5.0f && lifetimeLeft > 1.0f) 
					{
						stunTimer = lifetimeLeft;
					}
				}

				if(stunTimer > 0.2f && EnemyType != EnemyClass.Phantom)
				{
					if(thisAni.isPlaying)
					{
						thisAni.Stop();
					}

					stunTimer -= 1.0f * Time.deltaTime;
					MoveState = MovementState.StopMove;
				}
				else if(stunTimer > 0.0f && EnemyType != EnemyClass.Phantom)
				{
					stunTimer -= 1.0f * Time.deltaTime;
					raycastCheck = 1;
					PrevState = MoveState;
					MoveState = MovementState.CanMove;

					if(!thisAni.isPlaying)
					{
						thisAni.Play(animation:"ArmoredWalk");
					}
				}
				else if(EnemyType == EnemyClass.Phantom)
				{
					if(!thisAni.isPlaying)
					{
						thisAni.Play(animation:"ArmoredWalk");
					}
				}
			}
			else
			{
				RaycastHit hit;
				if(Physics.Raycast(transform.position, transform.forward, out hit, 1.5f, flashLayer))
				{
					gameObject.SetActive(false);
				}
				if(EnemyType == EnemyClass.Swarm)
				{
					timer += Time.deltaTime * 1.0f;

					if(Input.GetKeyDown(KeyCode.C))
					{
						timer = 359.5f;
					}

					if(timer > 360.0f)
					{
						for(int i = 0; i < swarmScripts.Length; i++)
						{
							swarmScripts[i].colorPhase += 1;
						}
						timer = 0.0f;
					}
				}

				if(Physics.Raycast(transform.position, raycastDirection, out hit, 2.5f, wallLayer))
				{
					if(!swarmDeath)
					{
						swarmDeath = true;
						Physics.IgnoreCollision(thisCC, hit.collider);

						for(int i = 0; i < swarmScripts.Length; i ++)
						{
							swarmScripts[i].inEffect = false;
						}
					}

				}

				SwarmMovement();
			}

			CheckPatrol();
			PauseAtPlayer();
			HealthCheck();
		}
		else if(!ThrowItemScript.Variables.timeStop && tilt)
		{
			if(thisAni != null && thisAni.isPlaying)
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

			thisCC.radius = 0.05f;
			thisCC.height = 0.1f;
			thisCC.center = new Vector3(0.0f, -0.1f, thisCC.center.z);
			thisRB.AddForce(randomRotX, randomRotY, randomRotZ);

			if(thisRB.velocity.y < -15f)
			{
				gameObject.SetActive(false);
			}
		}
		else
		{
			if(thisAni != null && thisAni.isPlaying)
			{
				thisAni.Stop();
			}
		}
	}

	void HealthCheck()
	{
		if(health <= 0)
		{
			gameObject.SetActive(false);
		}

		if(EnemyType != EnemyClass.Phantom && health == 1 && shieldItem.activeSelf)
		{
			shieldItem.SetActive(false);
		}
		else if(EnemyType != EnemyClass.Phantom && health > 1 && !shieldItem.activeSelf)
		{
			shieldItem.SetActive(true);
		}

		if(thisRB.velocity.y < -20f)
		{
			gameObject.SetActive(false);
		}

		if(swarmDeath)
		{
			swarmDeathTimer += Time.deltaTime;

			if(swarmDeathTimer >= 2.1f)
			{
				gameObject.SetActive(false);
			}
		}
	}

	void PauseAtPlayer()
	{
		if(EnemyType != EnemyClass.Phantom && EnemyType != EnemyClass.Swarm)
		{
			if(Physics.Raycast(transform.position, raycastDirection, 1.3f, knightLayer))
			{
				MoveState = MovementState.StopMove;
				PrevState = MovementState.CanMove;

				if(timer <= 0.0f)
				{
					timer = 3.0f;
					if(!KnightHealth.carryingShield)
					{
						KnightHealth.currentStamina -= 1;
					}
					else
					{
						KnightHealth.carryingShield = false;
					}
				}
			}
			else if(PrevState == MovementState.CanMove && !Physics.Raycast(transform.position, raycastDirection, 1.3f, knightLayer))
			{
				MoveState = MovementState.CanMove;
				PrevState = MovementState.StopMove;
			}
		}
		else if(EnemyType == EnemyClass.Phantom)
		{
			MoveState = MovementState.CanMove;
			PrevState = MovementState.StopMove;

			if(Physics.Raycast(transform.position, raycastDirection, 1.3f, knightLayer))
			{
				if(timer <= 0.0f)
				{
					timer = 3.0f;
					KnightHealth.currentStamina -= 1;
				}
			}
		}
		else if(EnemyType == EnemyClass.Swarm)
		{
			MoveState = MovementState.CanMove;
			PrevState = MovementState.StopMove;

			if(Physics.Raycast(transform.position, raycastDirection, 1.5f, knightLayer) ||
				Physics.Raycast(transform.position, raycastDirection, 1.0f, knightLayer) ||
				Physics.Raycast(transform.position, raycastDirection, 0.5f, knightLayer))
			{
				KnightScript.stunTimer = 4.0f;
			}

			RaycastHit enemy;

			if(Physics.Raycast(transform.position, raycastDirection, out enemy, 1.5f, enemyLayer) ||
				Physics.Raycast(transform.position, raycastDirection, out enemy, 1.0f, enemyLayer) ||
				Physics.Raycast(transform.position, raycastDirection, out enemy, 0.5f, enemyLayer))
			{
				enemy.collider.GetComponent<EnemyAIScript>().stunTimer = 4.0f;
				Physics.IgnoreCollision(thisCC, enemy.collider);
			}
		}
	}

	void SwarmMovement()
	{
		transform.Translate(Vector3.forward * speed * Time.deltaTime);

		float xDistance = Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(targetPos.transform.position.x));
		float zDistance = Mathf.Abs(Mathf.Abs(transform.position.z) - Mathf.Abs(Mathf.Abs(targetPos.transform.position.z) + 0.55f));

		if(swarmDistanceX > swarmDistanceZ)
		{
			if(transform.position.x > targetPos.position.x + 0.01f)
			{
				transform.localEulerAngles = new Vector3(0.0f, 270.0f, 0.0f);
			}
			else if(transform.position.x < targetPos.position.x - 0.01f)
			{
				transform.localEulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
			}
			else if(transform.position.z > targetPos.position.z + 0.55f)
			{
				transform.localEulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
			}
			else if(transform.position.z < targetPos.position.z + 0.35f)
			{
				transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
			}
		}
		else
		{
			if(transform.position.z > targetPos.position.z + 0.55f)
			{
				transform.localEulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
			}
			else if(transform.position.z < targetPos.position.z + 0.35f)
			{
				transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
			}
			else if(transform.position.x > targetPos.position.x + 0.02f)
			{
				transform.localEulerAngles = new Vector3(0.0f, 270.0f, 0.0f);
			}
			else if(transform.position.x < targetPos.position.x - 0.02f)
			{
				transform.localEulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
			}
		}

	}

	void BumpedIntoFriend()
	{
		if(bumpTimer < 0.05f)
		{
			speed = 7;
			bumpTimer += 1.0f * Time.deltaTime;
			transform.Translate(Vector3.up * speed * Time.deltaTime);
		}
		else if(bumpTimer <= 0.3f)
		{
			speed = 1;
			bumpTimer += 1.0f * Time.deltaTime;
			transform.Translate(Vector3.up * speed * Time.deltaTime);
		}
		else if(bumpTimer <= 1.2f)
		{
			bumpTimer += 1.0f * Time.deltaTime;
		}
		else if(bumpTimer <= 2.8f)
		{
			bumpTimer += 1.0f * Time.deltaTime;

			float originalX = transform.eulerAngles.x;
			float originalY = transform.eulerAngles.y;
			float originalZ = transform.eulerAngles.z;

			bumpTurn += 2.4f * Time.deltaTime;

			if(Time.timeScale != 0)
			{
				transform.eulerAngles = new Vector3(originalX, originalY + bumpTurn, originalZ);
			}
		}
		else
		{
			DontFallOff();
			bumpTurn = 0.0f;
			speed = 3;

			if(EnemyType == EnemyClass.Phantom)
			{
				speed = 2;
			}

			raycastDirection = transform.forward;
			bumpTimer = 0.0f;
			BehaveState = PrevBehaviour;
			PrevBehaviour = BehaviourState.Bumped;
		}
	}

	void CheckPatrol()
	{
		if(patrolPoints.Length != 0 && patrolPoints[0] != null && EnemyType != EnemyClass.Swarm)
		{
			for(int i = 0; i < patrolPoints.Length; i ++)
			{
				if(transform.position.x > patrolPoints[i].position.x - 1 && transform.position.x < patrolPoints[i].position.x + 1 &&
					transform.position.z > patrolPoints[i].position.z - 1 && transform.position.z < patrolPoints[i].position.z + 1)
				{
					if(patrolCheck != i)
					{
						patrolCheck = i;

						if(BehaveState == BehaviourState.Patrolling)
						{
							if(lostCount >= 0)
							{
								lostCount = -1;
							}

							raycastCheck = 7;
							isMoving = false;
						}
					}
				}
			}
		}
		else if(patrolPoints.Length > 0 && patrolPoints[0] != null && EnemyType == EnemyClass.Swarm)
		{
			if(transform.position.x < targetPos.position.x + 0.05f && transform.position.x > targetPos.position.x - 0.05f &&
				transform.position.z < targetPos.position.z + 0.55f && transform.position.z > targetPos.position.z - 0.55f)
			{
				if(!swarmGetTarget)
				{
					swarmGetTarget = true;
					patrolCheck += 1;

					if(patrolCheck >= patrolPoints.Length)
					{
						patrolCheck = 0;
					}

					targetPos = patrolPoints[patrolCheck];

					swarmDistanceX = Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(targetPos.transform.position.x));
					swarmDistanceZ = Mathf.Abs(Mathf.Abs(transform.position.z) - Mathf.Abs(targetPos.transform.position.z));
				}
			}
			else
			{
				swarmGetTarget = false;
			}
		}
	}

	void WalkOnceMore()
	{
		isMoving = true;

		float newAngle = Mathf.RoundToInt(transform.eulerAngles.y) - originalRotation;

		if((Mathf.Abs(newAngle) == 180 || Mathf.Abs(newAngle) == 360) && lostCount < 1 && BehaveState == BehaviourState.Patrolling)
		{
			lostCount ++;
		}

		originalRotation = Mathf.RoundToInt(transform.eulerAngles.y);
	}

	void CheckRotate()
	{
		if(raycastCheck == 1 || raycastCheck == 5)
		{
			transform.RotateAround(transform.position, Vector3.up, 0);
			hasTurned = true;
		}
		else if(raycastCheck == 2 || raycastCheck == 6)
		{
			transform.RotateAround(transform.position, Vector3.up, -90);
			hasTurned = true;
		}
		else if(raycastCheck == 3 || raycastCheck == 7)
		{
			transform.RotateAround(transform.position, Vector3.up, 180);
			hasTurned = true;
		}
		else if(raycastCheck == 4 || raycastCheck == 8)
		{
			transform.RotateAround(transform.position, Vector3.up, 90);
			hasTurned = true;
		}

		if(raycastCheck == 9)
		{
			if(Physics.Raycast(transform.position + transform.forward, Vector3.down , raycastDistance, floorLayer))
			{
				transform.RotateAround(transform.position, Vector3.up, 0);
				raycastDirection = transform.forward;
				WalkOnceMore();
			}
			else if(Physics.Raycast(transform.position - transform.right, Vector3.down , raycastDistance, floorLayer))
			{
				transform.RotateAround(transform.position, Vector3.up, -90);
				raycastDirection = transform.forward;
				WalkOnceMore();
			}
			else if(Physics.Raycast(transform.position + transform.right, Vector3.down , raycastDistance, floorLayer))
			{
				transform.RotateAround(transform.position, Vector3.up, 90);
				raycastDirection = transform.forward;
				WalkOnceMore();
			}
			else if(Physics.Raycast(transform.position - transform.forward, Vector3.down , raycastDistance, floorLayer))
			{
				transform.RotateAround(transform.position, Vector3.up, 180);
				raycastDirection = transform.forward;
				WalkOnceMore();
			}
		}
		else if(raycastCheck == 10)
		{
			if(Physics.Raycast(transform.position - transform.right, Vector3.down , raycastDistance, floorLayer))
			{
				transform.RotateAround(transform.position, Vector3.up, -90);
				raycastDirection = transform.forward;
				WalkOnceMore();
			}
			else if(Physics.Raycast(transform.position + transform.right, Vector3.down , raycastDistance, floorLayer))
			{
				transform.RotateAround(transform.position, Vector3.up, 90);
				raycastDirection = transform.forward;
				WalkOnceMore();
			}
			else if(Physics.Raycast(transform.position - transform.forward, Vector3.down , raycastDistance, floorLayer))
			{
				transform.RotateAround(transform.position, Vector3.up, 180);
				raycastDirection = transform.forward;
				WalkOnceMore();
			}
		}
		else if(raycastCheck < 9)
		{
			raycastCheck++;
		}
	}

	void GoToTarget()
	{
		float originZ = transform.eulerAngles.z;
		float originX = transform.eulerAngles.x;
		int angle = Mathf.RoundToInt(transform.eulerAngles.y);

		if(targetPos.position.x > transform.position.x + 1f && raycastCheck == 1 && angle != 270)
		{
			transform.eulerAngles = new Vector3(originX, 90f, originZ);
			hasTurned = true;
		}
		else if(targetPos.position.x < transform.position.x - 1f && raycastCheck == 2 && angle != 90f)
		{
			transform.eulerAngles = new Vector3(originX, 270, originZ);
			hasTurned = true;
		}
		else if(targetPos.position.z > transform.position.z + 1f && raycastCheck == 3 && angle != 180f)
		{
			transform.eulerAngles = new Vector3(originX, 0, originZ);
			hasTurned = true;
		}
		else if(targetPos.position.z < transform.position.z - 1f && raycastCheck == 4 && angle != 0)
		{
			transform.eulerAngles = new Vector3(originX, 180f, originZ);
			hasTurned = true;
		}

		if(raycastCheck == 5)
		{
			transform.eulerAngles = new Vector3(originX, originalRotation, originZ);
		}

		if(raycastCheck >= 6)
		{
			CheckRotate();
		}
		else
		{
			raycastCheck++;
		}
	}

	void DontFallOff()
	{
		int angle = Mathf.RoundToInt (transform.localEulerAngles.y);
		float originalX = transform.eulerAngles.x;
		float originalZ = transform.eulerAngles.z;

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
		if(lostCount >= 1 && BehaveState == BehaviourState.Patrolling)
		{
			CheckRotate();
		}
		else
		{
			GoToTarget();
		}

		if(rotating)
		{
			DontFallOff();
		}

		raycastDirection = transform.forward;

		if(Physics.Raycast(transform.position + raycastDirection, Vector3.down , raycastDistance, floorLayer))
		{
			WalkOnceMore();
		}
	}

	void CheckSides()
	{
		int angle = Mathf.RoundToInt(transform.eulerAngles.y);
		Vector3 backside = transform.position - transform.forward / 4.0f;
		Vector3 frontside = transform.position + transform.forward / 4.0f;

		if(angle != 90 && angle != 270)
		{
			if(Physics.Raycast(transform.position + Vector3.right, Vector3.down, raycastDistance, floorLayer)
				&& Physics.Raycast(backside + Vector3.right, Vector3.down, raycastDistance, floorLayer)
				&& Physics.Raycast(frontside + Vector3.right, Vector3.down, raycastDistance, floorLayer)
				&& targetPos.position.x > transform.position.x + 1.15f)
			{
				if(!checkedSides)
				{
					checkedSides = true;
					raycastCheck = 1;
				}

				if(!hasTurned)
				{
					CheckNextDirection();
				}
			}
			else if(Physics.Raycast(transform.position + Vector3.left, Vector3.down, raycastDistance, floorLayer)
				&& Physics.Raycast(backside + Vector3.left, Vector3.down, raycastDistance, floorLayer)
				&& Physics.Raycast(frontside + Vector3.left, Vector3.down, raycastDistance, floorLayer)
				&& targetPos.position.x < transform.position.x - 1.15f)
			{
				if(!checkedSides)
				{
					checkedSides = true;
					raycastCheck = 1;
				}

				if(!hasTurned)
				{
					CheckNextDirection();
				}
			}
			else
			{
				hasTurned = false;
				checkedSides = false;
			}
		}
		else
		{
			if(Physics.Raycast(transform.position + Vector3.forward, Vector3.down, raycastDistance, floorLayer)
				&& Physics.Raycast(backside + Vector3.forward, Vector3.down, raycastDistance, floorLayer)
				&& Physics.Raycast(frontside + Vector3.forward, Vector3.down, raycastDistance, floorLayer)
				&& targetPos.position.z > transform.position.z + 1.15f)
			{
				if(!checkedSides)
				{
					checkedSides = true;
					raycastCheck = 1;
				}

				if(!hasTurned)
				{
					CheckNextDirection();
				}
			}
			else if(Physics.Raycast(transform.position + Vector3.back, Vector3.down, raycastDistance, floorLayer)
				&& Physics.Raycast(backside + Vector3.back, Vector3.down, raycastDistance, floorLayer)
				&& Physics.Raycast(frontside + Vector3.back, Vector3.down, raycastDistance, floorLayer)
				&& targetPos.position.z < transform.position.z - 1.15f)
			{
				if(!checkedSides)
				{
					checkedSides = true;
					raycastCheck = 1;
				}

				if(!hasTurned)
				{
					CheckNextDirection();
				}
			}
			else
			{
				hasTurned = false;
				checkedSides = false;
			}
		}
	}

	void MoveForward()
	{
		RaycastHit target;

		if(Physics.Raycast(transform.position, raycastDirection, 1f, wallLayer) && EnemyType != EnemyClass.Swarm)
		{
			raycastCheck = 10;
			isMoving = false;
		}

		if(Physics.Raycast(transform.position, raycastDirection, out target, 2.0f, enemyLayer) && EnemyType != EnemyClass.Phantom)
		{
			if(target.collider.GetComponent<EnemyAIScript>().EnemyType != EnemyClass.Swarm && EnemyType != EnemyClass.Phantom)
			{
				PrevBehaviour = BehaveState;
				BehaveState = BehaviourState.Bumped;
			}
		}
		else if(Physics.Raycast(transform.position + raycastDirection / 2, Vector3.down, raycastDistance, floorLayer) || 
			Physics.Raycast(transform.position + raycastDirection, Vector3.down, raycastDistance, floorLayer))
		{
			transform.Translate(Vector3.forward * speed * Time.deltaTime);

			if(targetPos != null)
			{
				CheckSides();
			}
		}
		else
		{
			raycastCheck = 1;
			isMoving = false;
		}
	}

	void ResetParenthood()
	{
		if(transform.parent != parent)
		{
			transform.parent = parent;
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
			if(EnemyType != EnemyClass.Phantom && EnemyType != EnemyClass.Swarm)
			{
				float lifetimeLeft = coll.gameObject.GetComponent<FlashBombItemScript>().lifetime - 
					coll.gameObject.GetComponent<FlashBombItemScript>().timeElapsed;
				if(lifetimeLeft < 5.0f && lifetimeLeft > 1.0f) 
				{
					stunTimer = lifetimeLeft;
				}
			}
			else if(EnemyType == EnemyClass.Swarm)
			{
				gameObject.SetActive(false);
			}
		}
	}

	void OnTriggerExit(Collider coll)
	{
		if(coll.CompareTag("RotatingBridge"))
		{
			ResetParenthood();
		}

	}
}