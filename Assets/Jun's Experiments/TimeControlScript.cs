using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeControlScript : MonoBehaviour {

	public enum TimeControlType
	{
		Manager = 0,
		Slider
	}

	public TimeControlType TimeType;
	public Slider theSlider;

	void OnEnable()
	{
		if(TimeType == TimeControlType.Manager)
		{
			Time.timeScale = 1;
			theSlider.gameObject.SetActive(false);
		}
		else
		{
			theSlider = GetComponent<Slider>();
			theSlider.value = 0.3f;
		}
	}

	void Update()
	{
		if(TimeType == TimeControlType.Manager)
		{
			
		}
	}

	void ActivateSlider()
	{
		if(Input.GetKeyDown(KeyCode.Q))
		{
			
		}
	}
}
