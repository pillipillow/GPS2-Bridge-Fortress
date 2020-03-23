using UnityEngine;
using System.Collections.Generic;

public enum ColorType
{
	RED,
	GREEN,
	BLUE,
	YELLOW,
	PURPLE
}

public class GlowObjectCmd : MonoBehaviour
{
	public Color GlowColor;
	public float LerpFactor = 10;
	public ColorType colorType = ColorType.RED;
	public List<GameObject> relatedBridge; 

	public Renderer[] Renderers
	{
		get;
		private set;
	}

	public Color CurrentColor
	{
		get { return _currentColor; }
	}

	private Color _currentColor;
	private Color _targetColor;

	void Start()
	{
		Renderers = GetComponentsInChildren<Renderer>();
		GlowController.RegisterObject(this);

		relatedBridge.AddRange(GameObject.FindGameObjectsWithTag("RotatingBridge"));
	}

	public void Lit()
	{
		_targetColor = GlowColor;
		enabled = true;
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Squire")
		{
			Lit();

			for ( int i = 0 ;i < relatedBridge.Count;i++)
			{
				if( relatedBridge[i].GetComponent<GlowObjectCmd>().colorType == this.colorType)
				{
					if ( relatedBridge[i] != this.gameObject )
					{
						if ( relatedBridge[i] != this.gameObject )relatedBridge[i].GetComponent<GlowObjectCmd>().Lit();
					}
				}
			}
		}
	}

	public void UnLit()
	{
		_targetColor = Color.black;
		enabled = true;
	} 

	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Squire")
		{
			UnLit();

			for ( int i = 0 ;i < relatedBridge.Count;i++){
				if( relatedBridge[i].GetComponent<GlowObjectCmd>().colorType == this.colorType)
				{
					if ( relatedBridge[i] != this.gameObject )relatedBridge[i].GetComponent<GlowObjectCmd>().UnLit();
				}
			}
		}
	}

	/// <summary>
	/// Update color, disable self if we reach our target color.
	/// </summary>
	private void Update()
	{
		_currentColor = Color.Lerp(_currentColor, _targetColor, Time.deltaTime * LerpFactor);

		if (_currentColor.Equals(_targetColor))
		{
			enabled = false;
		}
	}
}
;