using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Knight"))
		{
			KnightAttackScript knightScript = other.gameObject.GetComponent<KnightAttackScript>();

			if(knightScript.carryingShield)
			{
				knightScript.carryingShield = false;
			}
			else
			{
				knightScript.currentStamina--;
			}
		}
		else if(other.CompareTag("Enemy"))
		{
			other.gameObject.GetComponent<EnemyAIScript>().health--;
		}
	}
}
