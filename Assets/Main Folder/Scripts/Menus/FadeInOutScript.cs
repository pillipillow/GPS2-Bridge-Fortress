using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOutScript : MonoBehaviour {

	private static FadeInOutScript mInstance;

	public static FadeInOutScript Instance
	{
		get
		{
			if (mInstance == null) 
			{
				GameObject tempObject = GameObject.FindWithTag("FadeInOutManager");

				if(tempObject == null)
				{
					GameObject obj = new GameObject("FadeInOutManager");
					mInstance = obj.AddComponent<FadeInOutScript>();
					obj.tag = "FadeInOutManager";
				}
				else
				{
					mInstance = tempObject.GetComponent<FadeInOutScript>();
				}
				DontDestroyOnLoad(mInstance.gameObject);
			}
			return mInstance;
		}
	}

	public static bool CheckInstanceExist()
	{
		return mInstance;
	}

	void Awake () 
	{
		if(FadeInOutScript.CheckInstanceExist())
		{
			Destroy(this.gameObject);
		}
	}


	public Image blackScreen;
	private float alpha;
	float fadeSpeed = 0.6f;

	public bool isFadeToScene;
	public bool isFullyFadedToScene;

	public bool isFadeToBlack;
	public bool isFullyFadedToBlack;

	public bool isFadeEnded;
	public bool isNextLevelLoaded;

	void Start()
	{
		blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();


		isFadeToBlack = false;
     	isFullyFadedToBlack = false;

		isFadeToScene = false;
		isFullyFadedToScene = false;

		isFadeEnded = true;
		isNextLevelLoaded = true;
	}

	public void FadeToScene()
	{
		alpha = 1;
		isFullyFadedToScene = false;

		StartCoroutine(FadeToScene(fadeSpeed));
	}

	IEnumerator FadeToScene(float fadeSpeed)
	{


		while( alpha >= 0)
		{
			alpha -= fadeSpeed * Time.unscaledDeltaTime / 1f;

			blackScreen.color = new Color(0.0f, 0.0f, 0.0f, alpha);
			yield return null;

			if (alpha <= 0.1f)
			{
				isFullyFadedToScene = true;
				isFadeToScene = false;
			}

			if (alpha <= 0.6f && isFadeToScene)
			{
				if (!SoundManagerScript.Instance.bgmAudioSource.isPlaying)
				{
					SoundManagerScript.Instance.PlayBGM(AudioClipID.BGM_IN_GAME);
				}
			}
		}
	}

	public  void FadeToBlack()
	{
		alpha = 0;
		isFullyFadedToBlack = false;

		StartCoroutine(FadeToBlack(fadeSpeed));
	}

	IEnumerator FadeToBlack(float fadeSpeed)
	{
		while( alpha <= 0.9f)
		{
			alpha += fadeSpeed * Time.unscaledDeltaTime / 1f;

			blackScreen.color = new Color(0.0f, 0.0f, 0.0f, alpha);
			yield return null;

			if (alpha >= 0.9)
			{
				isFullyFadedToBlack = true;
				isFadeToBlack = false;
			}
		}
	}
}
