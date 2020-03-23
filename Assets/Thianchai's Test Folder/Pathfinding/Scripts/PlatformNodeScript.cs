using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//HT This script is the mode basic class of each node on the grid
public class PlatformNodeScript
{
	//HT This boloen determines whether or not the platform is walakable
	public bool walkable;
	//HT this stores the position of the particular node in the world
	public Vector3 worldPosition;

	//HT This constructer takes in the 2 variables below to create each node
	public PlatformNodeScript (bool _walkable, Vector3 _worldPosition)
	{
		//HT The variables are assigned locally
		walkable = _walkable;
		worldPosition = _worldPosition;
	}
}
