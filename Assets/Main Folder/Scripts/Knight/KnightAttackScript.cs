using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KnightAttackScript : MonoBehaviour 
{
	private KnightMovementScript KnightMovement;
	private RagdollScript knightRagdoll;
	public int attack;
	private float attackRate;
	public bool isAttacking;
	public bool isEngaged;
	const float cooldownTimer = 1.0f;

	public float stamina;
	public float currentStamina;
	private RectTransform staminaBar;
	private float initLength;
	private float staminaToLengthRatio;
	public float staminaChangeSpeed;
	public bool drankPotion;
	public bool healthIncrease;

	private EnemyAIScript EnemyAI;
	private LayerMask enemyLayer;
	private LayerMask floorLayer;
	private Vector3 raycastDirection;
	private float raycastDistance;

	//Dania: Damage Feedback function
	public Renderer[] rend;
	Shader standardShader;
	public Shader damageShader;
	public int damageStamina;
	public float setStunTimer = 0.5f;
	float timer;

	//Jun: to play animations and the shield
	private Animation thisAni;
	public GameObject myShield;
	public bool carryingShield;

	private Vector3 startingRayPos;
	private Vector3 slightlyUp;

	void Start()
	{
		KnightMovement = gameObject.GetComponent<KnightMovementScript>();
		knightRagdoll = GetComponent<RagdollScript>();

		enemyLayer = LayerMask.GetMask("Enemy");
		floorLayer = LayerMask.GetMask("Walkable");
		stamina = 5f;
		attack = 1;
		attackRate = 0.0f;
		isAttacking = false;
		isEngaged = false;

		currentStamina = stamina;
		staminaBar = GameObject.Find("Stamina").GetComponent<RectTransform>();
		initLength = staminaBar.rect.xMax;
		staminaToLengthRatio = initLength / stamina;
		staminaChangeSpeed = 1f;
		drankPotion = false;
		healthIncrease = false;

		raycastDistance = 1.1f;

		//Dania: Damage Feedback function
		rend = new Renderer[8];
		rend = gameObject.GetComponentsInChildren<MeshRenderer>();
		standardShader =  Shader.Find("Standard");

		//damageStamina = currentStamina;
		timer = setStunTimer;

		carryingShield = false;
		thisAni = GetComponent<Animation>();

		slightlyUp = new Vector3(0, 1, 0); 
	}

	void Update()
	{
		KnightAttackBehaviour();
		InitiateAttack();
		KnightStaminaChange();
		DamagedFeedback();
		ShieldVisibility();
	}

	void InitiateAttack()
	{
		if (isAttacking)
		{
			RaycastHit hit;

			Vector3 leftPoint = transform.position - transform.right / 4;
			Vector3 rightPoint = transform.position + transform.right / 4;

			if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance, enemyLayer)
				|| Physics.Raycast(leftPoint, transform.forward, out hit, raycastDistance, enemyLayer)
				|| Physics.Raycast(rightPoint, transform.forward, out hit, raycastDistance, enemyLayer)) // 3D ray returns boolean
			{
				hit.collider.GetComponent<EnemyAIScript>().health--;

				SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_ATTACKING);
			}
		}
	}

	public void KnightStaminaChange()
	{
		if (drankPotion)
		{
			currentStamina = 5;
			healthIncrease = true;
			drankPotion = false;
		}

		if (healthIncrease == true)
		{
			staminaBar.sizeDelta += new Vector2(staminaChangeSpeed * 3f * staminaToLengthRatio, 0f) * Time.deltaTime;

			if (staminaBar.rect.width/initLength > currentStamina/stamina)
			{
				healthIncrease = false;
			}
		}

		if (staminaBar.rect.width/initLength > currentStamina/stamina)
		{
			staminaBar.sizeDelta -= new Vector2(staminaChangeSpeed * staminaToLengthRatio, 0f) * Time.deltaTime;
		}

		if (currentStamina < 1f && staminaBar.rect.width < 0.1f)
		{
			GetComponent<KnightCheckpointScript>().Return();
			currentStamina = 5f;
			staminaBar.sizeDelta = new Vector2(0f, 0f);
			GetComponent<KnightMovementScript>().stunTimer = 1.0f;
			GetComponent<KnightMovementScript>().proceedMovement = false;
		}
	}

	void KnightAttackBehaviour()
	{
		Vector3 leftPoint = transform.position - transform.right / 4;
		Vector3 rightPoint = transform.position + transform.right / 4;

		RaycastHit hit;

		Debug.DrawRay((transform.position + transform.up) - transform.forward, Vector3.down, Color.red);

		//Detects if theres enemy, stops if there is
		if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance, enemyLayer)
			|| Physics.Raycast(leftPoint, transform.forward, out hit, raycastDistance, enemyLayer)
			|| Physics.Raycast(rightPoint, transform.forward, out hit, raycastDistance, enemyLayer))
		{
			if(hit.transform.GetComponent<EnemyAIScript>().EnemyType != EnemyAIScript.EnemyClass.Phantom && 
				hit.transform.GetComponent<EnemyAIScript>().EnemyType != EnemyAIScript.EnemyClass.Swarm)
			{
				KnightMovement.MoveState = KnightMovementScript.MovementState.StopMove;
				isEngaged = true;

				if(!thisAni.IsPlaying("KnightAttackAnimation"))
				{
					thisAni.Stop();
					thisAni.Play(animation:"KnightAttackAnimation");
				}
			}
			else if(hit.transform.GetComponent<EnemyAIScript>().EnemyType == EnemyAIScript.EnemyClass.Phantom)
			{
				bool back = false;
				bool right = false;
				bool left = false;

				if(Physics.Raycast((transform.position + transform.up) - transform.forward, Vector3.down , 5.0f, floorLayer))
				{
					back = true;
				}
				if(Physics.Raycast((transform.position + transform.up) + transform.right, Vector3.down , 5.0f, floorLayer))
				{
					right = true;
				}
				if(Physics.Raycast((transform.position + transform.up) - transform.right, Vector3.down , 5.0f, floorLayer))
				{
					left = true;
				}

				if(back || right || left)
				{
					KnightMovement.isMoving = false;
					KnightMovement.raycastCheck = 2;
				}
				else
				{
					Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), hit.transform.GetComponent<CapsuleCollider>());
				}
			}
			else if(hit.transform.GetComponent<EnemyAIScript>().EnemyType == EnemyAIScript.EnemyClass.Swarm)
			{
				Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), hit.transform.GetComponent<CapsuleCollider>());
			}
		}
		else if(KnightMovement.MoveState != KnightMovementScript.MovementState.Stunned)
		{
			KnightMovement.MoveState = KnightMovementScript.MovementState.CanMove;
			isEngaged = false;
		}

		if (isEngaged == true)
		{
			attackRate += Time.deltaTime;
		}
		else
		{
			attackRate = 0.0f;
		}

		if (attackRate >= cooldownTimer)
		{
			attackRate = 0.0f;
			isAttacking = true;
			InitiateAttack();
			isAttacking = false;
		}
	}

	void DamagedFeedback()
	{
		if((int)currentStamina < damageStamina)
		{
			for(int i = 0; i < rend.Length; i ++)
			{
				rend[i].material.shader = damageShader;
			}

			timer -= Time.deltaTime;
			if(timer <= 0)
			{
				damageStamina = (int)currentStamina;

				for(int i = 0; i < rend.Length; i ++)
				{
					rend[i].material.shader = standardShader;
				}

				timer = setStunTimer;
			}
		}
		else if(currentStamina == stamina)
		{
			damageStamina = (int)currentStamina;
		}
	}

	void ShieldVisibility()
	{
		if(carryingShield && !knightRagdoll.flip)
		{
			myShield.SetActive(true);
		}
		else
		{
			myShield.SetActive(false);
		}
	}
}