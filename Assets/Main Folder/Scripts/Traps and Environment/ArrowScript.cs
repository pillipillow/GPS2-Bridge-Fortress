using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour {

	public float projectileSpeed = 10f;
	public float distance = 6.0f;
	Vector3 originSpawn;

	void Start () 
	{
		this.transform.Rotate(90,90,90); //D - This may need to be change depending on the model
		originSpawn = transform.position;	
	}

	void Update () 
	{	//D - Vector3 may need to be change depending on the model		
		transform.Translate(Vector3.down * Time.deltaTime * projectileSpeed);
		if(transform.position.z < originSpawn.z + -distance || transform.position.z > originSpawn.z + distance 
		|| transform.position.x < originSpawn.x + -distance || transform.position.x > originSpawn.x + distance)
		{
			Destroy(this.gameObject);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		//HT changes made to detect knight
		if(other.CompareTag("Knight"))
		{
			//other.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(3,0,3);

			if(!other.GetComponent<KnightAttackScript>().carryingShield)
			{
				other.gameObject.GetComponent<KnightAttackScript>().currentStamina--;
			}

			Destroy(this.gameObject);
		}
		else if(other.CompareTag("Enemy"))
		{
			if(other.gameObject.GetComponent<EnemyAIScript>().EnemyType != EnemyAIScript.EnemyClass.Swarm)
			{
				if(other.gameObject.GetComponent<EnemyAIScript>().health < 2)
				{
					other.gameObject.GetComponent<EnemyAIScript>().health--;
				}

				Destroy(this.gameObject);
			}
		}
	}
}
