using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaPotionScript : MonoBehaviour {

	[System.Serializable]
	public enum PotionType		//To make it easier for Designers to choose potion type and caters new potions
	{
		STAMINA

	}

	private KnightAttackScript knightAttackScript;
	public PotionType potionType = PotionType.STAMINA;


	// Use this for initialization
	void Start () 
	{
		knightAttackScript = GameObject.FindGameObjectWithTag("Knight").GetComponent<KnightAttackScript>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.collider.CompareTag("Knight"))
		{
			if (potionType == PotionType.STAMINA)
			{
				knightAttackScript.drankPotion = true;
				this.gameObject.SetActive(false);
					
				SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_POTIONDRINK);
			}
		}
		else 
		{
			Physics.IgnoreCollision(gameObject.GetComponent<BoxCollider>(), other.collider);
		}
	}
}
