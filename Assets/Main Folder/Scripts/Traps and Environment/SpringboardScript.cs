using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringboardScript : MonoBehaviour {

	public List<GameObject> victimList;
	public Transform targetLocation;

	private RaycastHit hit;
	private LayerMask enemyLayer;
	private LayerMask knightLayer;

	private bool solidTarget;
	private bool isPlayedSpring;

	void Start()
	{
		knightLayer = LayerMask.GetMask("Knight");
		enemyLayer = LayerMask.GetMask("Enemy");

		isPlayedSpring = false;

		MeshRenderer targetBody = targetLocation.gameObject.GetComponent<MeshRenderer>();

		if(targetBody != null)
		{
			solidTarget = true;
		}
		else
		{
			solidTarget = false;
		}
	}

	void Update()
	{
		if(!ThrowItemScript.Variables.timeStop)
		{
			DetectVictim();
		}

		LandedVictim();
	}

	void DetectVictim()
	{
		Debug.DrawRay(transform.position, Vector3.up * 1.0f);

		if(Physics.Raycast(transform.position, Vector3.up, out hit, 1.0f, knightLayer) || 
			Physics.Raycast(transform.position, Vector3.up, out hit, 1.0f, enemyLayer))
		{
			GameObject victim = hit.collider.gameObject;

			if(victim.transform.position.y < transform.position.y + 4.0f)
			{
				bool isSwarm = false;

				if(victim.CompareTag("Enemy"))
				{
					if(victim.GetComponent<EnemyAIScript>().EnemyType != EnemyAIScript.EnemyClass.Swarm)
					{
						victim.GetComponent<EnemyAIScript>().MoveState = EnemyAIScript.MovementState.StopMove;
					}
					else
					{
						isSwarm = true;
					}

					if (!isPlayedSpring)
					{
						SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_SPRINGTRAP);
						isPlayedSpring = true;
					}
				}
				else if(victim.CompareTag("Knight"))
				{
					victim.GetComponent<KnightMovementScript>().stunTimer = 100.0f;
					victim.GetComponent<RagdollScript>().flip = true;

					if (!isPlayedSpring)
					{
						SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_SPRINGTRAP);
						isPlayedSpring = true;
					}
				}

				if(!isSwarm)
				{
					if(!victimList.Contains(victim))
					{
						victimList.Add(victim);
					}

					Launch(victim);
				}
			}
		}
	}

	void Launch(GameObject victim)
	{
		Vector3 pos = transform.position;
		Vector3 target = targetLocation.position;

		float dist = Vector3.Distance(pos, target);

		float Vi = Mathf.Sqrt(dist * -Physics.gravity.y / (Mathf.Sin(Mathf.Deg2Rad * 60 * 2)));
		float Vy, Vz;

		Vy = Vi * Mathf.Sin(Mathf.Deg2Rad * 60);
		Vz = Vi * Mathf.Cos(Mathf.Deg2Rad * 60);

		Vector3 launchVelocity;

		if(pos.x - 1.0f > target.x)
		{
			launchVelocity = new Vector3(-Vz, Vy, 0.0f);
			victim.GetComponent<Rigidbody>().velocity = new Vector3(launchVelocity.x, launchVelocity.y, launchVelocity.z);
		}
		else if(pos.x + 1.0f < target.x)
		{
			launchVelocity = new Vector3(Vz, Vy, 0.0f);
			victim.GetComponent<Rigidbody>().velocity = new Vector3(launchVelocity.x, launchVelocity.y, launchVelocity.z);
		}
		else if(pos.z - 1.0f > target.z)
		{
			launchVelocity = new Vector3(0.0f, Vy, -Vz);
			victim.GetComponent<Rigidbody>().velocity = new Vector3(launchVelocity.x, launchVelocity.y, launchVelocity.z);
		}
		else if(pos.z + 1.0f < target.z)
		{
			launchVelocity = new Vector3(0.0f, Vy, Vz);
			victim.GetComponent<Rigidbody>().velocity = new Vector3(launchVelocity.x, launchVelocity.y, launchVelocity.z);
		}
	}

	void LandedVictim()
	{
		if(victimList.Count > 0)
		{
			for(int i = 0; i < victimList.Count; i ++)
			{
				if(victimList[i] != null)
				{
					if((victimList[i].transform.position.x > targetLocation.position.x - 0.5f && victimList[i].transform.position.x < targetLocation.position.x + 0.5f &&
						victimList[i].transform.position.z > targetLocation.position.z - 0.5f && victimList[i].transform.position.z < targetLocation.position.z + 0.5f) ||
						victimList[i].GetComponent<Rigidbody>().velocity.y == 0)
					{
						victimList[i].GetComponent<Rigidbody>().velocity = Vector3.zero;

						if(victimList[i].CompareTag("Enemy"))
						{
							victimList[i].GetComponent<EnemyAIScript>().stunTimer = 1.2f;
						}
						else if(victimList[i].CompareTag("Knight"))
						{
							if(solidTarget)
							{
								victimList[i].GetComponent<KnightMovementScript>().stunTimer = 1.2f;
								victimList[i].GetComponent<RagdollScript>().flip = false;
							}
						}

						victimList.Remove(victimList[i]);
						isPlayedSpring = false;
					}
				}
			}
		}
	}
}
