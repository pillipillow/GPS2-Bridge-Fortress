using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBombItemScript : MonoBehaviour {

	//HT To store the light particle effect
	GameObject particleEffect;
	//HT To store the sphere before explosion
	GameObject sphere;
	//HT Initial and final sizes for the trigger collider
	float initialSize;
	float finalSize;
	//HT Lifetime of the flash bomb
	public float lifetime;
	float sizeIncreamentPerSecond;
	public float timeElapsed;
	//HT If collision is detected
	bool isHit;

	//Jun to fix weird glitch
	bool hasBlown;

	void Awake()
	{
		sphere = transform.GetChild(0).gameObject;
		particleEffect = transform.GetChild(1).gameObject;
		particleEffect.SetActive(false);
		initialSize = 0.5f;
		finalSize = 4.0f;
		lifetime = 5.0f;
		sizeIncreamentPerSecond = (finalSize - initialSize) / (lifetime - 3.0f);
		timeElapsed = 0.0f;
		isHit = false;
		hasBlown = false;
	}

	void Update()
	{
		if(isHit)
		{
			CollisionIncreament();
		}

		if (timeElapsed >= lifetime)
		{
			//HT deactivate after the lifetime of the flash bomb is over
			gameObject.SetActive(false);
		}
	}

	//Jun changed to Trigger rather than collision due to bug which causes enemies to fly upon impact
	void OnTriggerStay(Collider col)
	{
		if(!hasBlown)
		{
			hasBlown = true;
			GetComponent<Rigidbody>().freezeRotation = true;

			//HT When Flash Bomb hits something
			particleEffect.SetActive(true);
			sphere.SetActive(false);
			isHit = true;
		}
	}

	void  CollisionIncreament()
	{
		//HT Trigger Collison Box Increament;
		if (GetComponent<SphereCollider>().radius < 4.0f)
		{
			GetComponent<SphereCollider>().radius += (sizeIncreamentPerSecond * Time.deltaTime);
		}
		timeElapsed += Time.deltaTime;
	}
}
