using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeActivateScript : MonoBehaviour {

	public SpikeScript relatedTrap;
	SpikeActivateScript self;
	public float setStunTimer;
	public bool canTrigger;

	// Use this for initialization
	void Start () 
	{
		canTrigger = true;
		setStunTimer = 2.2f;
		self = GetComponent<SpikeActivateScript>();
	}

	void OnCollisionEnter(Collision coll)
	{
		Collider other = coll.collider;
		if(!ThrowItemScript.Variables.timeStop && canTrigger && self.enabled)
		{
			if(other.CompareTag("Knight"))
			{
				relatedTrap.ActivateTrap();

				SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_SPIKETRAP);

			}
			else if(other.CompareTag("Enemy") && other.GetComponent<EnemyAIScript>().EnemyType != EnemyAIScript.EnemyClass.Swarm)
			{
				relatedTrap.ActivateTrap();

				SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_SPIKETRAP);
			}
		}
	}

	void OnCollisionExit(Collision coll)
	{
		if(!canTrigger)
		{
			canTrigger = true;
		}
	}
}
