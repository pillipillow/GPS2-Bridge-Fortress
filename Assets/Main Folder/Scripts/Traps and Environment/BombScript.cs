using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour 
{
	public bool isActivated;
	public float standardForce;

	public List<GameObject> aoeHitList;

	public GameObject explosion;
	private Vector3 objectPosition;
	private Vector3 blastDirection;
	private SphereCollider thisSC;

	void Start () 
	{
		isActivated = false;
		standardForce = 4f;
		thisSC = GetComponent<SphereCollider>();
		thisSC.enabled = false;
	}

	void Update () 
	{
		if (isActivated && !ThrowItemScript.Variables.timeStop)
		{
			thisSC.enabled = true;

			if(aoeHitList.Count != 0)
			{
				ExplodeParticles();
				Explode();
			}

			isActivated = false;

			for (int i = 0; i < aoeHitList.Count; i++)
			{
				aoeHitList.Remove(aoeHitList[i]);
			}
		}
	}

	void GetDirection(int i)
	{
		objectPosition = aoeHitList[i].transform.position - transform.position;

		if (objectPosition.x > 0f && Mathf.Abs(objectPosition.x) > Mathf.Abs(objectPosition.z))
		{
			blastDirection = Vector3.right; //go right
		}

		if (objectPosition.x < 0f && Mathf.Abs(objectPosition.x) > Mathf.Abs(objectPosition.z))
		{
			blastDirection = Vector3.left; //go left
		}

		if (objectPosition.z > 0f && Mathf.Abs(objectPosition.z) > Mathf.Abs(objectPosition.x))
		{
			blastDirection = Vector3.forward; //go up
		}

		if (objectPosition.z < 0f && Mathf.Abs(objectPosition.z) > Mathf.Abs(objectPosition.x))
		{
			blastDirection = Vector3.back; //go down
		}


	}

	void Explode()
	{
		for (int i = 0; i < aoeHitList.Count; i++)
		{
			if (aoeHitList[i].CompareTag("Knight"))
			{
				aoeHitList[i].GetComponent<KnightMovementScript>().stunTimer = 2.0f;

				if(aoeHitList[i].GetComponent<KnightAttackScript>().carryingShield)
				{
					aoeHitList[i].GetComponent<KnightAttackScript>().carryingShield = false;
				}
				else
				{
					aoeHitList[i].GetComponent<KnightAttackScript>().currentStamina --;
				}

				GetDirection(i);
				aoeHitList[i].GetComponent<Rigidbody>().AddForce(blastDirection * standardForce, ForceMode.Impulse);
			}

			else if (aoeHitList[i].CompareTag("Enemy"))
			{
				aoeHitList[i].GetComponent<EnemyAIScript>().stunTimer = 2.0f;
				aoeHitList[i].GetComponent<EnemyAIScript>().health --;
				GetDirection(i);
				aoeHitList[i].GetComponent<Rigidbody>().AddForce(blastDirection * standardForce, ForceMode.Impulse);
			}
		}

		Camera.main.GetComponent<CameraTouchInputScript>().shakeDuration = 1.0f;
		this.gameObject.SetActive(false);
	}

	void ExplodeParticles()
	{
		GameObject bombParticles = (GameObject)Instantiate (explosion);
		bombParticles.transform.position = transform.position;

		SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_BOMBEXPLOSION);
	}

	void OnTriggerStay(Collider entity)
	{
		if(!aoeHitList.Contains(entity.gameObject) && entity.gameObject.tag != "Shield")
		{
			aoeHitList.Add(entity.gameObject);
		}
	}

	void OnCollisionStay(Collision entity)
	{
		if(entity.collider.CompareTag("Knight"))
		{
			if(!isActivated)
			{
				isActivated = true;
			}
		}
		else if(entity.collider.CompareTag("Enemy"))
		{
			if(!isActivated && entity.collider.GetComponent<EnemyAIScript>().EnemyType != EnemyAIScript.EnemyClass.Swarm)
			{
				isActivated = true;
			}
		}
	}

	void OnCollisionExit(Collision entity)
	{
		if(entity.collider.CompareTag("Knight") || entity.collider.CompareTag("Enemy"))
		{
			isActivated = false;
			aoeHitList.Clear();
		}
	}
}
