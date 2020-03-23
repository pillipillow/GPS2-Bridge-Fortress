using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//HT This script manages all the nodes in a grid
public class PlatformGridScript : MonoBehaviour 
{
	//HT Creates a layer of mask that is walkable;
	public LayerMask walkableMask;
	//HT The size of the entire grid
	public Vector2 gridWorldSize;
	//HT The radius each node has
	public float nodeRadius;
	//HT This is an array of nodes that make a grid
	PlatformNodeScript[,] grid;

	float nodeDiameter;
	//HT The amount of nodes that can be fitted into the grid;
	int nodeAmountX, nodeAmountY;

	void Start ()
	{
		nodeDiameter = nodeRadius*2;
		//HT Calculating amount of nodes that can be fitted into the grid;
		nodeAmountX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
		nodeAmountY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
		CreateGrid ();
	}


	//HT Creating the Grid
	void CreateGrid ()
	{
		//HT creates a new data to store the nodes
		grid = new PlatformNodeScript[nodeAmountX, nodeAmountY];
		//HT getting the left edge of the world
		Vector3 worldBottomLeft = transform.position + (Vector3.left * (nodeAmountX/2)) + (Vector3.back * (nodeAmountY/2)) + new Vector3(-nodeRadius, 0, -nodeRadius/2);
		Debug.Log(worldBottomLeft);

		for (int i = 0; i < nodeAmountX; i++)
		{
			for (int j = 0; j < nodeAmountY; j++)
			{
				//HT moving along the grid
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.forward * (j * nodeDiameter + nodeRadius);
				//HT checking for collision, off setted radius to reduce collision detection error
				bool walkable = Physics.CheckSphere(worldPoint, nodeRadius - 0.1f, walkableMask);
				grid[i,j] = new PlatformNodeScript(walkable, worldPoint);
			}
		}

	}

	//HT This gives a visualization of the grid
	void OnDrawGizmos ()
	{
		//HT The top view is treated as an (x,y) grid
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x,1,gridWorldSize.y));

		//HT Drawing every node
		if(grid != null)
		{
			foreach(PlatformNodeScript n in grid)
			{
				Gizmos.color = (n.walkable)?Color.green:Color.red;
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
			}
		}
	
	}

}
