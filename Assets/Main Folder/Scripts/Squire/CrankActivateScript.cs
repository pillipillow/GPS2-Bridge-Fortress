using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrankActivateScript : MonoBehaviour
{
	public List<PlatformRotationScript> relatedBridge;

	public bool activate;

	private float i;
	private float originalX;
	private float originalY;
	public float speed;

	void Start()
	{
		activate = false;
		i = 0;
		originalX = transform.eulerAngles.x;
		originalY = transform.eulerAngles.y;
		speed = 70f;
	}

	void Update()
	{
		Activation();
		RotationCheck();
	}

	void Activation()
	{
		if(activate)
		{
			SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_TURNINGCRANK);

			for (int j = 0; j < relatedBridge.Count; j++)
			{
				if(!relatedBridge[j].rotating)
				{
					relatedBridge[j].rotating = true;
				}
			}
			activate = false;
		}
	}

	void RotationCheck()
	{
		if(relatedBridge[0] != null)
		{
			if(relatedBridge[0].rotating)
			{
				i = i + speed * Time.deltaTime;
				transform.eulerAngles = new Vector3(originalX,originalY,i);
			}
		}

		if(i >= 360)
		{
			i = 0;
		}
	}
}
