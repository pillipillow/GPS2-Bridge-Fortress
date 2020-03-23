using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrankHighlightScript : MonoBehaviour {

	public GameObject crankBody;
	public GameObject relatedBridge;
	Renderer crankRend;
	public Renderer[] bridgeRend;
	Shader standardShader;
	public Shader outlineShader;

	/*public Material crankOriginal;
	public Material crankHighlight;*/


    void Start() 
    {
    	crankRend = crankBody.GetComponent<Renderer>();
    	bridgeRend = new Renderer[4];
    	bridgeRend = relatedBridge.GetComponentsInChildren<Renderer>();
		standardShader = Shader.Find("Standard");

       //crankOriginal =  crankRend.material;
      
    }


	void OnTriggerEnter(Collider other) 
    {
    	if(other.tag == "Squire")
    	{
			crankRend.material.shader = outlineShader;
			for(int i = 0; i< bridgeRend.Length; i++)
			{
				bridgeRend[i].material.shader = outlineShader;
			}
    	}
		
    }
   
	void OnTriggerExit(Collider other) 
    {
		if(other.tag == "Squire")
    	{
			crankRend.material.shader = standardShader;
			for(int i = 0; i< bridgeRend.Length; i++)
			{
				bridgeRend[i].material.shader = standardShader;
			}
		}
    }
}	