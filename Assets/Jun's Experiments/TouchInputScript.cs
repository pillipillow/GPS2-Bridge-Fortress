using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputScript : MonoBehaviour
{
	public List<GameObject> touchList = new List<GameObject>();
	public GameObject[] oldTouches;
	private RaycastHit hit;

	void Update ()
	{
		if(Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if(Physics.Raycast(ray, out hit))
			{
				GameObject touchedObject = hit.transform.gameObject;

				if(touchedObject.CompareTag("Crank"))
				{
					
				}
			}
		}

		if(Input.touchCount == 1)
		{
			oldTouches = new GameObject[touchList.Count];
			touchList.CopyTo(oldTouches);
			touchList.Clear();

			foreach(Touch touch in Input.touches)
			{
				Ray ray = Camera.main.ScreenPointToRay(touch.position);

				if(Physics.Raycast(ray, out hit))
				{
					GameObject touchedObject = hit.transform.gameObject;
					touchList.Add(touchedObject);

					if(touchedObject.CompareTag("Crank") && touch.phase == TouchPhase.Began)
					{
						
					}
				}
			}
			foreach(GameObject g in oldTouches)
			{
				if(!touchList.Contains(g))
				{
					// what should happened to the previously touched object
				}
			}
		}

		/*
		 	==== Regarding swipe inputs ====
		 	
		    On the touch start phase, record the position and the time and set a 
		    boolean value to say that a swipe might be happening.

			Each frame during the drag phase, check that the finger doesn't stray too far 
			from a straight line (horizontal or vertical, depending on the swipe type). 
			If it does stray, cancel the boolean flag to denote it isn't a swipe.

			On the end phase, again, record the time and position and subtract the initial time 
			and position. If the time is greater than the maximum swipe time or the distance is 
			less than the minimum swipe distance, or the boolean flag is false then it's not a swipe. Otherwise, it is.

			^ You may also use this knowledge to create drag inputs

			================================

		e.g 

		float startTime;
		Vector2 startPos;
		boolean couldBeSwipe;
		float exitZone;
		float minSwipeDistance;
		float maxSwipeTime;

		if (Input.touchCount > 0)
		{
	      	oldTouches = new GameObject[touchList.Count];
			touchList.CopyTo(oldTouches);
			touchList.Clear();
	       
	        switch (touch.phase)
	        {
	            case TouchPhase.Began:
	                couldBeSwipe = true;
	                startPos = touch.position;
	                startTime = Time.time;
	                break;
	           
	            case TouchPhase.Moved:
	                if(Mathf.Abs(touch.position.y - startPos.y) > exitZone)
	                {
	                    couldBeSwipe = false;
	                }
	                break;
	           
	            case TouchPhase.Stationary:
	                couldBeSwipe = false;
	                break;
	           
	            case TouchPhase.Ended:
	                float swipeTime = Time.time - startTime;
	                float minSwipeDistance = (touch.position - startPos).magnitude;
	               
	                if(couldBeSwipe && (swipeTime < maxSwipeTime) && (swipeDist > minSwipeDist))
	                {
	                    // It's a swiiiiiiiiiiiipe!
	                    float swipeDirection = Mathf.Sign(touch.position.y - startPos.y);
	                }
	                break;
	        }
    	}
		*/

	}
}
