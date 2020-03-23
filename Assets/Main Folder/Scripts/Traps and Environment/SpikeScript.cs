using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour {

	Animator animator;
	public SpikeActivateScript relatedScript;
	private KnightMovementScript KnightMovement;
	private KnightAttackScript knightScript;

	void Start ()
	{
		animator = GetComponent<Animator>();
		KnightMovement = GameObject.FindGameObjectWithTag("Knight").GetComponent<KnightMovementScript>();
		knightScript = GameObject.FindGameObjectWithTag("Knight").GetComponent<KnightAttackScript>();
	}

	void Update () 
	{
		
	}

	public void ActivateTrap()
	{
		animator.SetTrigger("activateTrap");
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Knight") && relatedScript.canTrigger)
		{
			relatedScript.canTrigger = false;

			if(knightScript.carryingShield)
			{
				knightScript.carryingShield = false;
			}
			else
			{
				knightScript.currentStamina--;
				KnightMovement.stunTimer = relatedScript.setStunTimer;
			}
		}
		else if(other.CompareTag("Enemy") && relatedScript.canTrigger)
		{
			
			if(other.gameObject.GetComponent<EnemyAIScript>().health > 1)
			{
				relatedScript.canTrigger = false;
			}

			other.gameObject.GetComponent<EnemyAIScript>().health--;
			other.gameObject.GetComponent<EnemyAIScript>().stunTimer = 2.2f;
		}
	}
}
