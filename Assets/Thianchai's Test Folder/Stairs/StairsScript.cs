using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsScript : MonoBehaviour
{
	public enum StairsType
	{
		UPWARDS,
		DOWNWARDS,
		TOTAL
	};

	public StairsType stairsType;
	public float height;
}
