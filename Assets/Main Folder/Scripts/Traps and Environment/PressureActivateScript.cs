using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureActivateScript : MonoBehaviour {

	public enum ActivateTrap
	{
		FIRE,
	}

	public enum TrapSetting
	{
		NORMAL,
		REVERSE,
	}

	public ActivateTrap whatActivate;
	public TrapSetting activateSetting;

	public GameObject trapObject;
	public List<PressurePlateScript> pressurePlates;
	public bool allPressed;

	ParticleSystem particleFire;
	BoxCollider colliderObject;


	// Use this for initialization
	void Start () 
	{
		if(whatActivate == ActivateTrap.FIRE)
		{
			particleFire = trapObject.GetComponent<ParticleSystem>();
			colliderObject = trapObject.GetComponent<BoxCollider>();
		}	
	}
	
	// Update is called once per frame
	void Update () 
	{
		PressurePlate();
		if(whatActivate == ActivateTrap.FIRE)
		{
			FireTrap();
		}
	}

	void FireTrap()
	{
		if(allPressed)
		{
			if(activateSetting == TrapSetting.NORMAL)
			{
				DeActivateFire();
			}
			else if(activateSetting == TrapSetting.REVERSE)
			{
				ActivateFire();
			}
		}
		else
		{
			if(activateSetting == TrapSetting.NORMAL)
			{
				ActivateFire();

			}
			else if(activateSetting == TrapSetting.REVERSE)
			{
				DeActivateFire();
			}
		}
	}

	void ActivateFire()
	{
		particleFire.Play();
		colliderObject.enabled = true;

		//SoundManagerScript.Instance.PlayLoopingSFX(AudioClipID.SFX_FIREWALL_MAIN);  //Idk why there's a short separation in between :< //Giving a lot of problems. Disabled
	}

	void DeActivateFire()
	{
		particleFire.Stop();
		colliderObject.enabled = false;

		//SoundManagerScript.Instance.StopLoopingSFX(AudioClipID.SFX_FIREWALL_MAIN);
	}

	void PressurePlate() //idk what kind of reverse psychology is this but it works
	{
		if(!allPressed)
		{
			allPressed = true;
			for(int i = 0; i<pressurePlates.Count;i++)
			{
				if(!pressurePlates[i].isDown)
				{
					allPressed = false;
				}
			}
		}
		else
		{
			for(int i = 0; i<pressurePlates.Count;i++)
			{
				if(!pressurePlates[i].isDown)
				{
					allPressed = false;
				}
			}
		}
	}
}
