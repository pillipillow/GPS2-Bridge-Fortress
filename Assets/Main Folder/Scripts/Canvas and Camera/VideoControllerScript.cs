using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoControllerScript : MonoBehaviour {

	private GameObject video;
	private VideoPlayer videoPlayer;
	private GameObject skip;

	private float alpha;

	void Awake()
	{
		video = GameObject.Find("Video Player");
		video.SetActive(false);

		skip = GameObject.Find("Skip Button");
		skip.SetActive(false);

		alpha = 1;
	}

	// Use this for initialization
	void Start () 
	{
		videoPlayer = video.GetComponent<VideoPlayer>();
		StartCoroutine(Timer(31f));

		SoundManagerScript.Instance.StopBGM();
		//Somehow Video component doesn't detect anything DontDestroyOnLoad. So I made the Camera an AudioSource to play the audio instead
	}

	void Update()
	{
		videoPlayer.targetCameraAlpha = alpha;
	}

	//public so Skipbutton can use this
	public void LoadLevel1()
	{
		StartCoroutine(FadeOut());
	}

	IEnumerator Timer(float seconds)
	{
		yield return new WaitForSeconds(0.25f);
		video.SetActive(true);

		yield return new WaitForSeconds(2f);
		skip.SetActive(true);

		yield return new WaitForSeconds(seconds);
		LoadLevel1();

	}

	IEnumerator FadeOut()
	{
		while(alpha > 0)
		{
			alpha -= 0.75f * Time.deltaTime;
			yield return null;
		}
			
		if (alpha <= 0)
		{
			SceneManager.LoadScene("Level 1");
		}
	}
}
