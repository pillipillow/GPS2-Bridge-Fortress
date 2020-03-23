using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySightScript : MonoBehaviour
{
	private EnemyAIScript EnemyAI;

	void Start()
	{
		EnemyAI = transform.parent.GetComponent<EnemyAIScript>();
	}

	void OnTriggerStay(Collider coll)
	{
		if(coll.CompareTag("Knight"))
		{
			if(EnemyAI.ChangeableState == EnemyAIScript.ChangeBehavior.ItChanges)
			{
				EnemyAI.PrevBehaviour = EnemyAI.BehaveState;
				EnemyAI.BehaveState = EnemyAIScript.BehaviourState.Chase;
			}
		}
	}

	void OnTriggerExit(Collider coll)
	{
		if(coll.CompareTag("Knight"))
		{
			if(EnemyAI.ChangeableState == EnemyAIScript.ChangeBehavior.ItChanges)
			{
				EnemyAI.PrevBehaviour = EnemyAI.BehaveState;
				EnemyAI.BehaveState = EnemyAIScript.BehaviourState.Patrolling;
				EnemyAI.lostCount = -1;
			}
		}
	}
}
