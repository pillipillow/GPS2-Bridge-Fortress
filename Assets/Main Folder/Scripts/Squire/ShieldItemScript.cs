using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldItemScript : MonoBehaviour
{
	private SpikeActivateScript spikeTrap;
	private SpringboardScript springTrap;
	private BombScript bombTrap;
	private bool fireWallGet;
	private ParticleSystem fireWall;
	private LayerMask walkLayer;

	private MeshFilter thisMF;
	public bool pickedUp;
	public float destructTimer;

	void Start()
	{
		thisMF = GetComponent<MeshFilter>();
		walkLayer = LayerMask.NameToLayer("Walkable");
		destructTimer = 2.5f;
	}

	void Update()
	{
		if(pickedUp)
		{
			thisMF.mesh = null;
			destructTimer -= Time.deltaTime * 1;

			if(destructTimer < 0.0f)
			{
				if(spikeTrap != null)
				{
					spikeTrap.enabled = true;
				}
				if(springTrap != null)
				{
					springTrap.enabled = true;
				}
				if(bombTrap != null)
				{
					bombTrap.enabled = true;
				}
				Destroy(gameObject);
			}
		}

		if(fireWall != null)
		{
			if(fireWall.isPlaying)
			{
				Destroy(gameObject);
			}
		}
	}

	void OnTriggerStay(Collider coll)
	{
		if(transform.parent == null)
		{
			if(coll.gameObject.layer == walkLayer)
			{
				transform.parent = coll.transform;
			}
		}

		if(coll.CompareTag("Spike"))
		{
			if(coll.GetComponent<SpikeActivateScript>().enabled && !pickedUp)
			{
				spikeTrap = coll.GetComponent<SpikeActivateScript>();
				spikeTrap.enabled = false;
			}
		}
		else if(coll.CompareTag("Spring"))
		{
			if(coll.GetComponent<SpringboardScript>().enabled && !pickedUp)
			{
				springTrap = coll.GetComponent<SpringboardScript>();
				springTrap.enabled = false;
			}
		}
		else if(coll.CompareTag("Bomb"))
		{
			if(coll.GetComponent<BombScript>().enabled && !pickedUp)
			{
				bombTrap = coll.GetComponent<BombScript>();
				bombTrap.enabled = false;
			}
		}
		else if(coll.CompareTag("FireWall"))
		{
			if(!fireWallGet)
			{
				fireWallGet = true;
				fireWall = coll.transform.parent.GetComponentInChildren<ParticleSystem>();
			}
		}

		if(coll.CompareTag("Knight"))
		{
			pickedUp = true;
			coll.GetComponent<KnightAttackScript>().carryingShield = true;
		}
		else if(coll.CompareTag("Enemy") && coll.GetComponent<EnemyAIScript>().EnemyType != EnemyAIScript.EnemyClass.Phantom
			&& coll.GetComponent<EnemyAIScript>().EnemyType != EnemyAIScript.EnemyClass.Swarm)
		{
			pickedUp = true;
			if(coll.GetComponent<EnemyAIScript>().health == 1)
			{
				coll.GetComponent<EnemyAIScript>().health = 2;
			}
		}
	}
}
