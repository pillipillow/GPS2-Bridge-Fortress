using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class KnightCheckpointScript : MonoBehaviour
{
	public Rigidbody thisRB;
	private KnightMovementScript knightScript;
	public KnightAttackScript knightShield;
	private GameObject squire;
	private Vector3 restartPoint;
	private float directionZ;
	private float directionY;
	private float directionX;
	public bool heDead;

	public int deadPhase;

	public float timer;

	public int checkPointNum;

	private GameObject[] stairs;
	public bool frozen;

	private GameObject[] knightLayers;
	private GameObject[] enemies;
	private GameObject[] rotationPlatforms;
	private GameObject[] cranks;
	private GameObject[] bombs;
	private GameObject[] pressurePlates;
	public GameObject enemy;
	public GameObject reviveBoom;
	public GameObject body;
	public GameObject shieldPickUp;
	public GameObject bombObj;
	public Image fader;

	private Vector3 squireLocation;
	private NavMeshAgent squireAgent;
	private SquireMovementScript squireMovement;

	private RagdollScript ragdoll;

	public List<Vector3> enemyPos;
	public List<Quaternion> enemyRot;
	public List<Vector3> bombPos;
	public List<Quaternion> platformRotations;
	public List<Quaternion> crankRotations;

	private CapsuleCollider thisCC;
	private DragonBreathActivationScript dragonBreath;
	private MeshRenderer[] thisMR;
	private MeshRenderer[] squireMR;
	public Material squireMaterial;
	public Material thisMaterial;
	private Button shieldButton;

	void Start()
	{
		thisRB = GetComponent<Rigidbody>();
		deadPhase = 0;
		heDead = false;
		knightLayers = new GameObject[3];
		knightLayers[0] = GameObject.Find("Knight Layer");
		knightLayers[1] = GameObject.Find("Knight Layer 1");
		knightLayers[2] = GameObject.Find("Knight Layer 2");
		fader = GameObject.Find("Fader").GetComponent<Image>();

		GameObject dragon = GameObject.Find("Dragon Layer");
		if(dragon != null)
		{
			dragonBreath = GameObject.Find("Dragon Layer").GetComponent<DragonBreathActivationScript>();
		}

		body = gameObject.transform.GetChild(0).gameObject;
		shieldButton = GameObject.Find("ShieldUseButton").GetComponent<Button>();
		reviveBoom = transform.Find("Revive").gameObject;
		reviveBoom.SetActive(false);
		squire = GameObject.FindGameObjectWithTag("Player");
		squireMR = squire.GetComponentsInChildren<MeshRenderer>();
		squireMovement = squire.GetComponent<SquireMovementScript>();
		squireAgent = squire.GetComponent<NavMeshAgent>();
		squireLocation = squire.transform.position;

		Reset();
		knightScript = GetComponent<KnightMovementScript>();
		knightShield = GetComponent<KnightAttackScript>();
		ragdoll = GetComponent<RagdollScript>();
		thisCC = GetComponent<CapsuleCollider>();

		thisMR = GetComponentsInChildren<MeshRenderer>();

		cranks = GameObject.FindGameObjectsWithTag("Crank");
		bombs = GameObject.FindGameObjectsWithTag("Bomb");
		enemies = GameObject.FindGameObjectsWithTag("Enemy");
		rotationPlatforms = GameObject.FindGameObjectsWithTag("RotatingBridge");
		pressurePlates = GameObject.FindGameObjectsWithTag("PressurePlate");

		for(int i = 0; i < enemies.Length; i ++)
		{
			enemyPos.Add(enemies[i].transform.localPosition);
			enemyRot.Add(enemies[i].transform.localRotation);
		}

		for(int i = 0; i < bombs.Length; i ++)
		{
			bombPos.Add(bombs[i].transform.position);
		}

		for(int i = 0; i < rotationPlatforms.Length; i ++)
		{
			platformRotations.Add(rotationPlatforms[i].transform.localRotation);
		}

		for(int i = 0; i < cranks.Length; i ++)
		{
			crankRotations.Add(cranks[i].transform.localRotation);
		}
	}

	void Update()
	{
		if(thisRB.velocity.y < -15f)
		{
			Return();
		}

		if(heDead)
		{
			Revival();
		}

		if(deadPhase == 3)
		{
			if(timer < -1.5f)
			{
				ThrowItemScript.Variables.timeStop = false;
				reviveBoom.SetActive(false);
				timer = 0.0f;
				deadPhase = 0;
			}
			else
			{
				timer -= Time.deltaTime;
			}
		}
	}

	void Revival()
	{
		ragdoll.flip = false;
		knightScript.stunTimer = -1.0f;

		if(timer < 1.5f && deadPhase != 2)
		{
			body.SetActive(false);
			timer += Time.deltaTime;
		}
		else if(timer >= 1.5f && deadPhase != 2)
		{
			deadPhase = 2;
		}
		else if(timer >= 0.0f && deadPhase == 2)
		{
			timer -= Time.deltaTime;
		}

		if(frozen)
		{
			fader.color = new Color(0.0f, 1.0f, 1.0f, timer);
		}
		else
		{
			fader.color = new Color(0.0f, 0.0f, 0.0f, timer);
		}

		if(timer > 1.0f && deadPhase == 0)
		{
			ThrowItemScript.Variables.timeStop = true;
			deadPhase = 1;
			stairs = GameObject.FindGameObjectsWithTag("Stairs");

			if(squireMovement.frozen)
			{
				squire.transform.position = squireLocation;

				squireMovement.tilt = false;
				squireMovement.frozen = false;

				squireAgent.enabled = true;

				squireMovement.thisRB.freezeRotation = true;
				squireMovement.thisRB.velocity = Vector3.zero;
				squireMovement.thisCC.center = new Vector3(-0.27f, 1.46f, -0.1f);
				squireMovement.thisCC.radius = 0.84f;
				squireMovement.thisCC.height = 3.0f;

				for(int i = 0; i < squireMR.Length; i ++)
				{
					squireMR[i].material = squireMaterial;
				}
			}

			squireAgent.destination = squireLocation;
			ThrowItemScript.Variables.shieldAmount = 1;

			shieldButton.interactable = false;
			if(ThrowItemScript.Variables.isAiming)
			{
				ThrowItemScript.Variables.BeginShieldAim();
			}

			ThrowItemScript.Variables.flashAmount = 2;
			ThrowItemScript.Variables.buttonsNeeded[2].interactable = true;
			ThrowItemScript.Variables.buttonsNeeded[3].interactable = true;

			shieldPickUp = GameObject.FindGameObjectWithTag("Shield");
			if(shieldPickUp != null)
			{
				shieldPickUp.GetComponent<ShieldItemScript>().pickedUp = true;
				shieldPickUp.GetComponent<ShieldItemScript>().destructTimer = 1.0f;
			}

			for(int i = 0; i < stairs.Length; i ++)
			{
				stairs[i].layer = 8;
				for (int j = 0; j < stairs[i].transform.childCount; j++)
				{
					stairs[i].transform.GetChild(j).gameObject.layer = 8;
				}
			}

			for(int i = 0; i < enemies.Length; i ++)
			{
				if(checkPointNum == 0 && (enemies[i].transform.parent.name == "Knight Layer" ||
					enemies[i].transform.parent.name == "Knight Layer 1" || enemies[i].transform.parent.name == "Knight Layer 2"))
				{
					GameObject resetEnemy = Instantiate(enemy);

					resetEnemy.GetComponent<EnemyAIScript>().EnemyType = enemies[i].GetComponent<EnemyAIScript>().EnemyType;

					if(enemies[i].transform.parent.name == "Knight Layer")
					{
						resetEnemy.transform.parent = knightLayers[0].transform;
					}
					else if(enemies[i].transform.parent.name == "Knight Layer 1")
					{
						resetEnemy.transform.parent = knightLayers[1].transform;
					}
					else if(enemies[i].transform.parent.name == "Knight Layer 2")
					{
						resetEnemy.transform.parent = knightLayers[2].transform;
					}

					resetEnemy.GetComponent<EnemyAIScript>().patrolPoints = enemies[i].GetComponent<EnemyAIScript>().patrolPoints;

					resetEnemy.transform.localPosition = enemyPos[i];
					resetEnemy.transform.localRotation = enemyRot[i];
					Destroy(enemies[i]);
					enemies[i] = resetEnemy;
				}
				else if(checkPointNum == 1 && (enemies[i].transform.parent.name == "Knight Layer 1" || enemies[i].transform.parent.name == "Knight Layer 2"))
				{
					GameObject resetEnemy = Instantiate(enemy);

					resetEnemy.GetComponent<EnemyAIScript>().EnemyType = enemies[i].GetComponent<EnemyAIScript>().EnemyType;

					if(enemies[i].transform.parent.name == "Knight Layer 1")
					{
						resetEnemy.transform.parent = knightLayers[1].transform;
					}
					else if(enemies[i].transform.parent.name == "Knight Layer 2")
					{
						resetEnemy.transform.parent = knightLayers[2].transform;
					}

					resetEnemy.GetComponent<EnemyAIScript>().patrolPoints = enemies[i].GetComponent<EnemyAIScript>().patrolPoints;

					resetEnemy.transform.localPosition = enemyPos[i];
					resetEnemy.transform.localRotation = enemyRot[i];
					Destroy(enemies[i]);
					enemies[i] = resetEnemy;
				}
				else if(checkPointNum == 2 && enemies[i].transform.parent.name == "Knight Layer 2")
				{
					GameObject resetEnemy = Instantiate(enemy);
					resetEnemy.GetComponent<EnemyAIScript>().EnemyType = enemies[i].GetComponent<EnemyAIScript>().EnemyType;

					resetEnemy.GetComponent<EnemyAIScript>().patrolPoints = enemies[i].GetComponent<EnemyAIScript>().patrolPoints;

					resetEnemy.transform.parent = knightLayers[2].transform;
					resetEnemy.transform.localPosition = enemyPos[i];
					resetEnemy.transform.localRotation = enemyRot[i];
					Destroy(enemies[i]);
					enemies[i] = resetEnemy;
				}
			}
			for(int i = 0; i < cranks.Length; i ++)
			{
				cranks[i].GetComponent<CrankActivateScript>().activate = false;
				cranks[i].transform.rotation = crankRotations[i];
			}
			for(int i = 0; i < rotationPlatforms.Length; i ++)
			{
				rotationPlatforms[i].transform.localRotation = platformRotations[i];

				PlatformRotationScript thisBridge = rotationPlatforms[i].GetComponent<PlatformRotationScript>();

				thisBridge.rotating = false;
				rotationPlatforms[i].transform.rotation = thisBridge.startRot;
				thisBridge.forwardDirection = thisBridge.startDirection;
				thisBridge.i = thisBridge.startI;
				thisBridge.rotateTimes = thisBridge.startTimes;
			}

			for(int i = 0; i < pressurePlates.Length; i++)
			{
				pressurePlates[i].GetComponent<PressurePlateScript>().isDown = false;
			}

			for(int i = 0; i < bombs.Length; i ++)
			{
				if(checkPointNum == 0 && (bombs[i].transform.parent.name == "Knight Layer" || bombs[i].transform.parent.name == "Knight Layer 1" ||
					bombs[i].transform.parent.name == "Knight Layer 2"))
				{
					GameObject resetBomb = Instantiate(bombObj);

					if(bombs[i].transform.parent.name == "Knight Layer")
					{
						bombs[i].transform.parent = knightLayers[0].transform;
					}
					else if(bombs[i].transform.parent.name == "Knight Layer 1")
					{
						bombs[i].transform.parent = knightLayers[1].transform;
					}
					else if(bombs[i].transform.parent.name == "Knight Layer 2")
					{
						bombs[i].transform.parent = knightLayers[2].transform;
					}

					resetBomb.transform.position = bombPos[i];
					Destroy(bombs[i]);
					bombs[i] = resetBomb;
				}
				else if(checkPointNum == 1 && (bombs[i].transform.parent.name == "Knight Layer 1" || bombs[i].transform.parent.name == "Knight Layer 2"))
				{
					GameObject resetBomb = Instantiate(bombObj);

					if(bombs[i].transform.parent.name == "Knight Layer 1")
					{
						bombs[i].transform.parent = knightLayers[1].transform;
					}
					else if(bombs[i].transform.parent.name == "Knight Layer 2")
					{
						bombs[i].transform.parent = knightLayers[2].transform;
					}
					resetBomb.transform.position = bombPos[i];
					Destroy(bombs[i]);
					bombs[i] = resetBomb;
				}
				else if(checkPointNum == 2 && bombs[i].transform.parent.name == "Knight Layer 2")
				{
					Destroy(bombs[i]);
					GameObject resetBomb = Instantiate(bombObj);
					resetBomb.transform.position = bombPos[i];
					bombs[i] = resetBomb;
					bombs[i].transform.parent = knightLayers[2].transform;
				}
			}
		}

		if(timer < 0.0f && deadPhase == 2)
		{
			if(knightScript.tilt)
			{
				for(int i = 0; i < thisMR.Length; i ++)
				{
					thisMR[i].material = thisMaterial;
				}
				knightScript.tilt = false;
				thisCC.center = new Vector3(thisCC.center.x, 0.4237527f, thisCC.center.z);
				thisCC.height = 0.9120386f;
				thisRB.freezeRotation = true;
			}

			if(dragonBreath != null)
			{
				dragonBreath.timer = -1.0f;
			}

			heDead = false;
			deadPhase = 3;
			knightShield.carryingShield = false;
			knightScript.stunTimer = -1.0f;
			transform.position = new Vector3(restartPoint.x, restartPoint.y, restartPoint.z);
			transform.eulerAngles = new Vector3(directionX, directionY, directionZ);
			shieldButton.interactable = true;
			thisRB.velocity = new Vector3(0, 0, 0);
			frozen = false;
			knightScript.raycastDirection = knightScript.gameObject.transform.forward;
			reviveBoom.SetActive(true);
			body.SetActive(true);
		}
	}

	public void Reset()
	{
		directionX = transform.eulerAngles.x;
		directionY = transform.eulerAngles.y;
		directionZ = transform.eulerAngles.z;
		restartPoint = transform.position;
	}

	public void Return()
	{
		heDead = true;
		knightScript.stunTimer = 100.0f;
	}
}
