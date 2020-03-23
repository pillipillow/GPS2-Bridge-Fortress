using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTheKnight : MonoBehaviour {

	public KnightAttackScript knightlyDeath;

	// Use this for initialization
	void Start () {
		knightlyDeath = GameObject.FindGameObjectWithTag("Knight").GetComponent<KnightAttackScript>();
	}

	// Update is called once per frame
	void Update ()
	{
		if(Input.touchCount == 5 || Input.GetKeyDown(KeyCode.Alpha0))
		{
			knightlyDeath.currentStamina = 0;
		}
	}
}
