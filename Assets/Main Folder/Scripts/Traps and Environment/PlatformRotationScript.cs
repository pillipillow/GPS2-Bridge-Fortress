using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotateExtent
{
	CLOCKWISE = 0,
	ANTICLOCK,
	RETURNFORWARD,
	RETURNBACKWARD,
	DOUBLEFORWARD,
	DOUBLEBACKWARD
};

public class PlatformRotationScript : MonoBehaviour
{
	public RotateExtent RotateStyle;

	private bool newRotation;
	public bool forwardDirection;
	public bool rotating;

	private int endDegrees;
	public int rotateLimit;
	public int rotateTimes;
	public int rotateDegrees;

	public Quaternion startRot;
	public float startI;
	public int startTimes;
	public bool startDirection;
	public float i;
	public float speed;

	void Start()
	{
		forwardDirection = true;

		if(RotateStyle == RotateExtent.RETURNBACKWARD || RotateStyle == RotateExtent.DOUBLEBACKWARD)
		{
			forwardDirection = false;
		}

		newRotation = true;
		rotating = false;

		startDirection = forwardDirection;
		startRot = transform.rotation;
		startTimes = rotateTimes;
		startI = transform.localEulerAngles.y;
		i = transform.localEulerAngles.y;

		if(RotateStyle == RotateExtent.ANTICLOCK)
		{
			i -= 360;
		}

		speed = 70f;

		if(RotateStyle == RotateExtent.RETURNBACKWARD || RotateStyle == RotateExtent.RETURNFORWARD)
		{
			rotateLimit = 1;
		}
		else
		{
			rotateLimit = 2;
		}
	}

	void Update()
	{
		if(rotating)
		{
			if(RotateStyle == RotateExtent.CLOCKWISE)
			{
				ClockWiseRotation();
			}
			else if(RotateStyle == RotateExtent.ANTICLOCK)
			{
				AntiClockWiseRotation();
			}
			else
			{
				Returning();
			}
		}
	}

	void ClockWiseRotation()
	{
		if(i >= 360)
		{
			i = 0;
			transform.localEulerAngles = new Vector3(0,0,0);
		}

		if(newRotation)
		{
			newRotation = false;
			endDegrees = Mathf.RoundToInt(transform.localEulerAngles.y + rotateDegrees);
		}

		i = i + speed * Time.deltaTime;
		transform.localEulerAngles = new Vector3(0,i,0);

		if(i >= endDegrees)
		{
			rotating = false;
			newRotation = true;
			FixRotation();
		}
	}

	void AntiClockWiseRotation()
	{
		if(newRotation)
		{
			newRotation = false;
			float newDegrees = transform.localEulerAngles.y;

			if(i <= -360)
			{
				i = 0;
			}

			if(newDegrees != 0)
			{
				newDegrees -= 360;
			}

			endDegrees = Mathf.RoundToInt(newDegrees - rotateDegrees);

			Debug.Log(endDegrees);
		}

		i = i - speed * Time.deltaTime;

		transform.localEulerAngles = new Vector3(0,i,0);

		if(i <= endDegrees)
		{
			rotating = false;
			newRotation = true;
			EndAntiCheck();
			FixRotation();
		}
	}

	void Returning()
	{
		if(i >= 360)
		{
			i = 0;
			transform.localEulerAngles = new Vector3(0,0,0);
		}

		if(newRotation)
		{
			newRotation = false;
			float newDegrees = transform.localEulerAngles.y;

			if(i < -45)
			{
				i += 360;
			}

			if(forwardDirection)
			{
				endDegrees = Mathf.RoundToInt(newDegrees + rotateDegrees);
			}
			else
			{
				endDegrees = Mathf.RoundToInt(newDegrees - rotateDegrees);
			}
		}

		if(forwardDirection)
		{
			i = i + speed * Time.deltaTime;
			transform.localEulerAngles = new Vector3(0,i,0);

			if(i >= endDegrees)
			{
				rotating = false;
				newRotation = true;
				EndAntiCheck();
				FixRotation();

				rotateTimes += 1;

				if(rotateTimes >= rotateLimit)
				{
					rotateTimes = 0;
					forwardDirection = false;
				}
			}
		}
		else
		{
			i = i - speed * Time.deltaTime;
			transform.localEulerAngles = new Vector3(0,i,0);

			if(i <= endDegrees)
			{
				rotating = false;
				newRotation = true;
				EndAntiCheck();
				FixRotation();

				rotateTimes += 1;

				if(rotateTimes >= rotateLimit)
				{
					rotateTimes = 0;
					forwardDirection = true;
				}
			}
		}
	}

	void EndAntiCheck()
	{
		if(i <= -360)
		{
			i = 0;
			transform.localEulerAngles = new Vector3(0,0,0);
		}
	}

	void FixRotation()
	{
		float currAngle = Mathf.RoundToInt(transform.localEulerAngles.y);

		if(currAngle < 90 + 3 && currAngle > 90 - 3)
		{
			transform.localEulerAngles = new Vector3(0,90.0f,0);
		}
		else if(currAngle < 180 + 3 && currAngle > 180 - 3)
		{
			transform.localEulerAngles = new Vector3(0,180.0f,0);
		}
		else if(currAngle < 270 + 3 && currAngle > 270 - 3)
		{
			transform.localEulerAngles = new Vector3(0,270.0f,0);
		}
		else if(currAngle < 360 + 3 && currAngle > 360 - 3)
		{
			transform.localEulerAngles = new Vector3(0,360.0f,0);
		}
		else if(currAngle < 0 + 3 && currAngle > 0 - 3)
		{
			transform.localEulerAngles = new Vector3(0,0.0f,0);
		}
	}
}
