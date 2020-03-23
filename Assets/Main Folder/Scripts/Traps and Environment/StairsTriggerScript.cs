using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsTriggerScript : MonoBehaviour
{
	public GameObject relatedStairs;
	private GameObject knight;
	private KnightCheckpointScript knightCheckpoint;
	Vector3 offset;

	void Start()
	{
		knight = GameObject.FindGameObjectWithTag("Knight");
		offset = new Vector3(0.0f, 0.5f, 0.0f);
	}

	void OnTriggerEnter(Collider coll)
	{
		if(coll.CompareTag("Knight"))
		{
			if(relatedStairs != null)
			{
				knight.transform.position = relatedStairs.transform.position + offset;
				knight.GetComponent<KnightCheckpointScript>().Reset();

				if(transform.parent.name == "Knight Layer")
				{
					knight.GetComponent<KnightCheckpointScript>().checkPointNum = 1;
				}
				else if(transform.parent.name == "Knight Layer 1")
				{
					knight.GetComponent<KnightCheckpointScript>().checkPointNum = 2;
				}
			}
		}
	}

	//HT THis is for knight respawn
	void OnCollisionStay(Collision coll)
	{
		if(coll.gameObject.tag == "Knight")
		{
			coll.gameObject.GetComponent<KnightMovementScript>().proceedMovement = true;
		}
	}

	void OnTriggerExit(Collider coll)
	{
		if(coll.CompareTag("Knight"))
		{
		//	gameObject.layer = 10;
		//	for (int i = 0; i < transform.childCount; i++)
		//	{
		//		transform.GetChild(i).gameObject.layer = 10;
		//	}
		}

	}
}
