using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreenScript : MonoBehaviour {

	public Image splashImage;
	public Image splashImage2;

	// Use this for initialization
	IEnumerator Start () 
	{
		splashImage.canvasRenderer.SetAlpha(0.0f);
		splashImage2.canvasRenderer.SetAlpha(0.0f);	

		FadeIn(splashImage);
		yield return new WaitForSeconds(2f);
		FadeOut(splashImage);
		yield return new WaitForSeconds(3f);
		FadeIn(splashImage2);
		yield return new WaitForSeconds(2f);
		FadeOut(splashImage2);
		yield return new WaitForSeconds(3f);

		SceneManagerScript.LoadMainMenuScene();
	}

	void FadeIn(Image image)
	{
		image.CrossFadeAlpha(1.0f,1.5f,false);

	}

	void FadeOut(Image image)
	{
		image.CrossFadeAlpha(0.0f,3f,false);
	}


}
