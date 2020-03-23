using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//HT This code is not completed yet, and will resume after VS to avoid creating new bugs
public class PitfallTileScript : MonoBehaviour {

	//HT values that shake the tile on the x, y, z axis inseted into a sin or cos graph
	float xMovement;
	float zMovement;

	public float amplitute;	//HT The degree of the maximum point and minimum point of shaking
	public float speed;		//HT speed of how fast it shakes

	bool isFalling;
	float time; //HT Countdown timer for pitfall to SetActive state to false after drop

	void Start ()
	{
		xMovement = 0.0f;
		zMovement = 0.0f;
		isFalling = false;
	}

	void Update ()
	{
		if(isFalling)
		{
			time += Time.deltaTime;
			if (time >= 1.0f)
			{
				DeactivateBlock();
			}
		}
		else
		{
			xMovement += 1.0f * speed;
			zMovement += 1.0f * speed;

			transform.position = new Vector3 (transform.position.x + (Mathf.Sin(xMovement) * amplitute), transform.position.y, transform.position.z + (Mathf.Cos(zMovement) * amplitute));
		}
	}

	void DeactivateBlock()
	{
		gameObject.SetActive(false);
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "Knight")
		{
			GetComponent<Rigidbody>().isKinematic = false;
			GetComponent<Rigidbody>().useGravity = true;
			col.gameObject.GetComponent<KnightMovementScript>().stunTimer = 3.0f;
			isFalling = true;
		}
		if(col.gameObject.tag == "Enemy")
		{
			if (col.gameObject.GetComponent<EnemyAIScript>().EnemyType == EnemyAIScript.EnemyClass.Normal || 
				col.gameObject.GetComponent<EnemyAIScript>().EnemyType == EnemyAIScript.EnemyClass.Armored)
			{
				GetComponent<Rigidbody>().isKinematic = false;
				GetComponent<Rigidbody>().useGravity = true;
				col.GetComponent<EnemyAIScript>().stunTimer = 3.0f;
				isFalling = true;
			}
		}
	}
}
