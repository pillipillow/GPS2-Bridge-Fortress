using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepScript : MonoBehaviour
{
	void OnTriggerEnter(Collider coll)
	{
		if(coll.gameObject.CompareTag("Knight"))
		{
			coll.gameObject.GetComponent<KnightMovementScript>().speed = 0;
		}
	}
}
