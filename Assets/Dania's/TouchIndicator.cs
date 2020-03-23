using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchIndicator : MonoBehaviour {

	LayerMask squireLayer;
	public GameObject indicator;
	Vector3 offset;

	ParticleSystem particleIndicator;

	// Use this for initialization
	void Start () 
	{
		squireLayer = LayerMask.GetMask("SquireLayer");
		particleIndicator = indicator.GetComponent<ParticleSystem>();

		particleIndicator.Stop();

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit,Mathf.Infinity,squireLayer))
		{
			offset = this.transform.position - hit.point;
		}

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetMouseButton(0) && Application.platform == RuntimePlatform.WindowsEditor)
		{
			GetMouseInteraction();
		}
		else if(Application.platform == RuntimePlatform.Android)
		{
			GetTouchInteraction();
		}
	}

	void GetMouseInteraction()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit,Mathf.Infinity,squireLayer))
		{
			//Debug.Log ("Raycast hit at" +hit.point);
			particleIndicator.Play();
			this.transform.position = hit.point;
		}
	}

	void GetTouchInteraction()
	{
		if(Input.touches.Length>0)
		{
			if(Input.GetTouch(0).phase == TouchPhase.Began)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
				RaycastHit hit;
				if(Physics.Raycast(ray, out hit,Mathf.Infinity,squireLayer))
				{
					//Debug.Log ("Raycast hit at" +hit.point);
					particleIndicator.Play();
					this.transform.position = hit.point;
				}
			}
		}
	}

	void OnTriggerStay(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			particleIndicator.Stop();
		}
	}

}
