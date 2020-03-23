using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateScript : MonoBehaviour {

	Animator animator;
	public bool isDown;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		if(isDown)
		{
			goDown();
		}
		else
		{
			goUp();
		}

	}

	void goDown()
	{
		animator.SetBool("pressureDown",true);
		isDown = true;

		 // I'm going to make a separate function to play something once and then play another audio when it finishes
	}

	void goUp()
	{
		animator.SetBool("pressureDown",false);
		isDown = false;
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Knight") || other.CompareTag("Enemy"))
		{
			if(isDown)
			{
				goUp();
				SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_FIREWALL_END);
			}
			else
			{
				goDown();
				SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_FIREWALL_START);
			}
		}
	}
}
