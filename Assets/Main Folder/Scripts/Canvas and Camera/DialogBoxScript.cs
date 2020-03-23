using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogBoxScript : MonoBehaviour
{
	public int level;
	int pageNumber;
	//HT To store the current dialog
	string[] dialog;
	public Text charater;
	public Text textBox;
	public Image characterSprite;
	public List <Sprite> characterSpriteList = new List<Sprite>();

	public Button shieldButton;
	public Button flashButton;

	private bool hasRoared;

	public GameObject pauseUI;

	void Awake ()
	{
		pageNumber = 0;
		hasRoared = false;
		shieldButton = GameObject.Find("ShieldUseButton").GetComponent<Button>();
		flashButton = GameObject.Find("FlashUseButton").GetComponent<Button>();
		//HT Extrating Dialog from databas

		if(level <= DialogDataScript.levelDialogs.Count && level != 0)
		{
			Time.timeScale = 0.0f;
			shieldButton.interactable = false;
			flashButton.interactable = false;
			shieldButton.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
			flashButton.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
			dialog = DialogDataScript.levelDialogs[level - 1];
		} 
		else
		{
			Time.timeScale = 1.0f;
			shieldButton.interactable = true;
			flashButton.interactable = true;
			shieldButton.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			flashButton.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			gameObject.SetActive(false);

			if (!SoundManagerScript.Instance.bgmAudioSource.isPlaying)
			{
				SoundManagerScript.Instance.PlayBGM(AudioClipID.BGM_IN_GAME);
			}
		}

		if (!SoundManagerScript.Instance.bgmAudioSource.isPlaying)
		{
			SoundManagerScript.Instance.PlayBGM(AudioClipID.BGM_IN_GAME);
		}
	}

	void Start()
	{
		if (level == 14 && pageNumber == 0 && hasRoared == false) //level might change
		{
			SoundManagerScript.Instance.PlayRandomAlternating(AudioClipID.SBGM_RandomGrunt1, AudioClipID.SBGM_RandomGrunt2, AudioClipID.SBGM_RandomGrunt3);
			hasRoared = true;
		}

		if (!SoundManagerScript.Instance.bgmAudioSource.isPlaying)
		{
			SoundManagerScript.Instance.PlayBGM(AudioClipID.BGM_IN_GAME);
		}
	}

	void Update ()
	{
		if(pauseUI.activeSelf)
		{
			return;
		}

		if (Input.GetMouseButtonDown(0) ) //! && Alpha == 0)
		{
			if( pageNumber < dialog.Length - 2)
			{
				pageNumber += 2;
			}
			else
			{
				DeactivateTextBox();
			}
		}

		charater.text = dialog[pageNumber];
		textBox.text = dialog[pageNumber + 1];

		switch(charater.text)
		{
		case "Squire":
			characterSprite.sprite = characterSpriteList[0];
			break;
		case "Knight":
			characterSprite.sprite = characterSpriteList[1];
			break;
		}
	}

	public void DeactivateTextBox()
	{
		Time.timeScale = 1.0f;

		if(level != 1)
		{
			shieldButton.interactable = true;
			flashButton.interactable = true;
			shieldButton.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			flashButton.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		}

		gameObject.SetActive(false);

		if (!SoundManagerScript.Instance.bgmAudioSource.isPlaying)
		{
			SoundManagerScript.Instance.PlayBGM(AudioClipID.BGM_IN_GAME);
		}
	}
}
