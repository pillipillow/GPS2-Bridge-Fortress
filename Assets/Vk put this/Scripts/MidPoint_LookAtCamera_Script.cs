using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidPoint_LookAtCamera_Script : MonoBehaviour {

	public GameObject mainCamera;
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.LookAt(mainCamera.transform);
	}
}
