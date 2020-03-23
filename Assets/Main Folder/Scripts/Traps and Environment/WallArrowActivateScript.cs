using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallArrowActivateScript : MonoBehaviour {

	public GameObject arrows;
	public float setTimer = 1.0f;
	float timer;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay(Collider other)
	{
		if(!ThrowItemScript.Variables.timeStop)
		{
			if(other.CompareTag("Knight"))
			{
				timer -= Time.deltaTime;
				if(timer <= 0)
				{
					Instantiate (arrows, transform.position, transform.localRotation);
					timer = setTimer;

					SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_ARROWTRAP);
				}
			}
			else if(other.CompareTag("Enemy") && other.GetComponent<EnemyAIScript>().EnemyType != EnemyAIScript.EnemyClass.Swarm)
			{
				timer -= Time.deltaTime;
				if(timer <= 0)
				{
					Instantiate (arrows, transform.position, transform.localRotation);
					timer = setTimer;

					SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_ARROWTRAP);
				}
			}
		}
	}

}
