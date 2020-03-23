using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticleIndicatorScript : MonoBehaviour {

	float screenX;
	float screenY;

	Vector3 alteredCameraPosition;
	Vector3 screenPos;
	Vector3 newTransform;

	public GameObject particleIndicator;

	// Use this for initialization
	void Start () 
	{
		screenX = Screen.width * 84/100f;
		screenY = Screen.height * 78/100f;

		particleIndicator = GameObject.Find("Particle Indicator");

		//particleIndicator.transform.position = new Vector3(0, 0, 0);

		screenPos = Camera.main.WorldToScreenPoint(particleIndicator.transform.position);
		Debug.Log(screenPos);




		alteredCameraPosition = Camera.main.ScreenToWorldPoint(particleIndicator.transform.position);
		Debug.Log(alteredCameraPosition);

		newTransform = new Vector3(screenPos.x * screenX /alteredCameraPosition.x, 
			screenPos.y  / alteredCameraPosition.y,
			screenPos.z * screenY / alteredCameraPosition.z);

		particleIndicator.transform.position = newTransform;
	}

	void Update()
	{
		
	}
}
