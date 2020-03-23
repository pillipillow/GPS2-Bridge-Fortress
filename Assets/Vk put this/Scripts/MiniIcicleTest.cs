using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniIcicleTest : MonoBehaviour {

	//list <> Cubes with Icicles List
	List<GameObject> CubesWithIciclesList = new List <GameObject>();

	public GameObject icicle;
	public bool isActivated;

	private Vector3 fullyFrozenSize = new Vector3(1f, 1f, 2f);
	private Vector3 initSize = new Vector3(1f, 1f, 0f);

	void OnTriggerEnter(Collider bricks)
	{
		if (bricks.CompareTag("Freezeable"))
		{
		///	GameObject ice = (GameObject)Instantiate(icicle, bricks.transform);
		///	ice.transform.localPosition = new Vector3(-0.25f, 0.4f, 0.75f);
		///	CubesWithIciclesList.Add(ice);

			GameObject ice = (GameObject)Instantiate(icicle, bricks.transform);
			ice.transform.localPosition = new Vector3(Mathf.Abs(bricks.transform.localPosition.x) - Mathf.Abs(bricks.transform.position.x) + -0.25f, 0.4f,
														Mathf.Abs(bricks.transform.localPosition.z) - Mathf.Abs(bricks.transform.position.z) + 0.75f); // offset
			CubesWithIciclesList.Add(ice);
		}
	}



	// Use this for initialization
	void Start () 
	{
		isActivated = false; //set to true and boom
	}

	void FreezeCubes()
	{
		if (isActivated)
		{
			foreach(GameObject ice in CubesWithIciclesList)
			{
				if ( ice.transform.localScale.z < fullyFrozenSize.z)
				{
					ice.transform.localScale += new Vector3(0f, 0f, 1f) * Time.deltaTime;
				}
			}
		}
		else
		{
			foreach(GameObject ice in CubesWithIciclesList)
			{
				if ( ice.transform.localScale.z > initSize.z)
				{
					ice.transform.localScale -= new Vector3(0f, 0f, 1f) * Time.deltaTime;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		FreezeCubes();
	}


}
