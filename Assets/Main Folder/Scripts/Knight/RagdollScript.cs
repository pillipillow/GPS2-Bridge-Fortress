using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollScript : MonoBehaviour {

	// Head -> body -> right arm -> left arm -> right leg -> left leg -> entire body
	public GameObject[] bodyParts;
	public Quaternion[] bodyOriginalPos;

	public GameObject sword;

	public bool flip;
	public bool readyReset;

	public bool rotationSide1;
	public bool rotationSide2;
	public bool rotationSide3;
	public bool rotationSide4;

	public float head;
	public float originalHead;

	public float rightArm;
	public float originalRightArm;
	public float leftArm;
	public float originalLeftArm;
	public float rightLeg;
	public float originalRightLeg;
	public float leftLeg;
	public float originalLeftLeg;

	public float eBody;
	public float originalEBody;

	void Start()
	{
		bodyOriginalPos = new Quaternion[7];

		for(int i = 0; i < bodyOriginalPos.Length; i ++)
		{
			bodyOriginalPos[i] = bodyParts[i].transform.localRotation;
		}

		rotationSide1 = true;
		rotationSide2 = false;
		rotationSide3 = true;
		rotationSide4 = false;

		eBody = bodyParts[6].transform.localEulerAngles.z;
		originalEBody = eBody;

		head = bodyParts[0].transform.localEulerAngles.z;
		originalHead = head;

		rightArm = bodyParts[2].transform.localEulerAngles.z;
		originalRightArm = rightArm;

		leftArm = bodyParts[3].transform.localEulerAngles.z;
		originalLeftArm = leftArm;

		rightLeg = bodyParts[4].transform.localEulerAngles.z;
		originalRightLeg = rightLeg;

		leftLeg = bodyParts[5].transform.localEulerAngles.z;
		originalLeftLeg = leftLeg;
	}

	void Update()
	{
		if(flip)
		{
			FlipOut();
		}
		else
		{
			ResetAll();
		}
	}

	void FlipOut()
	{
		if(sword != null)
		{
			sword.SetActive(false);
		}

		readyReset = true;

		if(head > -40.0f)
		{
			head -= Time.deltaTime * 50.0f;
		}

		if(rightArm > 70.0f && rotationSide1)
		{
			rightArm -= Time.deltaTime * 500.0f;
		}
		else if(rightArm <= 70.0f && rotationSide1)
		{
			rotationSide1 = false;
		}

		if(rightArm < 260.0f && !rotationSide1)
		{
			rightArm += Time.deltaTime * 500.0f;
		}
		else if(rightArm >= 260.0f && !rotationSide1)
		{
			rotationSide1 = true;
		}

		if(leftArm > 70.0f && rotationSide2)
		{
			leftArm -= Time.deltaTime * 500.0f;
		}
		else if(leftArm <= 70.0f && rotationSide2)
		{
			rotationSide2 = false;
		}

		if(leftArm < 260.0f && !rotationSide2)
		{
			leftArm += Time.deltaTime * 500.0f;
		}
		else if(leftArm >= 260.0f && !rotationSide2)
		{
			rotationSide2 = true;
		}

		if(rightLeg > -60.0f && rotationSide3)
		{
			rightLeg -= Time.deltaTime * 400.0f;
		}
		else if(rightLeg <= -60.0f && rotationSide3)
		{
			rotationSide3 = false;
		}

		if(rightLeg < 60.0f && !rotationSide3)
		{
			rightLeg += Time.deltaTime * 400.0f;
		}
		else if(rightLeg >= 60.0f && !rotationSide3)
		{
			rotationSide3 = true;
		}

		if(leftLeg > -60.0f && rotationSide4)
		{
			leftLeg -= Time.deltaTime * 400.0f;
		}
		else if(leftLeg <= -60.0f && rotationSide4)
		{
			rotationSide4 = false;
		}

		if(leftLeg < 60.0f && !rotationSide4)
		{
			leftLeg += Time.deltaTime * 400.0f;
		}
		else if(leftLeg >= 60.0f && !rotationSide4)
		{
			rotationSide4 = true;
		}

		bodyParts[0].transform.localEulerAngles = new Vector3(bodyParts[0].transform.localEulerAngles.x, bodyParts[0].transform.localEulerAngles.y, head);
		bodyParts[2].transform.localEulerAngles = new Vector3(bodyParts[2].transform.localEulerAngles.x, bodyParts[2].transform.localEulerAngles.y, rightArm);
		bodyParts[3].transform.localEulerAngles = new Vector3(bodyParts[3].transform.localEulerAngles.x, bodyParts[3].transform.localEulerAngles.y, leftArm);
		bodyParts[4].transform.localEulerAngles = new Vector3(bodyParts[4].transform.localEulerAngles.x, bodyParts[4].transform.localEulerAngles.y, rightLeg);
		bodyParts[5].transform.localEulerAngles = new Vector3(bodyParts[5].transform.localEulerAngles.x, bodyParts[5].transform.localEulerAngles.y, leftLeg);

		eBody += Time.deltaTime * 200.0f;
		bodyParts[6].transform.localEulerAngles = new Vector3(bodyParts[6].transform.localEulerAngles.x, bodyParts[6].transform.localEulerAngles.y, eBody);
	}

	void ResetAll()
	{
		if(readyReset)
		{
			readyReset = false;

			if(sword != null)
			{
				sword.SetActive(true);
			}

			rotationSide1 = true;
			rotationSide2 = false;
			rotationSide3 = true;
			rotationSide4 = false;

			for(int i = 0; i < bodyOriginalPos.Length; i ++)
			{
				bodyParts[i].transform.localRotation = bodyOriginalPos[i];
			}

			eBody = originalEBody; 
			head = originalHead;
			rightArm = originalRightArm;
			leftArm = originalLeftArm;
			rightLeg = originalRightLeg;
			leftLeg = originalLeftLeg;
		}
	}
}
