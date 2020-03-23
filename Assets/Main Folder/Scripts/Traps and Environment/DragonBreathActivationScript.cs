using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBreathActivationScript : MonoBehaviour {

	private KnightMovementScript knightMovement;
	private KnightCheckpointScript knightRespawn;
	private SquireMovementScript squireMovement;
	private MeshRenderer[] knightMR;
	private MeshRenderer[] squireMR;

	public GameObject breathParticles;
	public List<GameObject> victims;

	private float breathCycle = 30f;
	private float breathDuration = 6f;
	private float roarDuration;
	public float timer;
	public bool isFire = false;
	private bool isPlayedSFX = false;

	private CameraTouchInputScript theCamera;

	private bool pauseThem;
	public bool beginTilt;

	public Material frozenPlain;
	public Material frozenKnight;
	public Material frozenSquire;
	private ParticleSystem[] PS;


	//Icicles stuff
	List<GameObject> CubesWithIciclesList = new List <GameObject>();
	List<GameObject> IcicleRemovalList = new List <GameObject>();

	public GameObject icicle;
	public bool isFreezeCubes;

	private Vector3 fullyFrozenSize = new Vector3(1f, 1f, 2f);
	private Vector3 initSize = new Vector3(1f, 1f, 0f);




	// Use this for initialization
	void Start () 
	{
		knightMovement = GameObject.FindGameObjectWithTag("Knight").GetComponent<KnightMovementScript>();
		squireMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<SquireMovementScript>();
		knightRespawn = GameObject.FindGameObjectWithTag("Knight").GetComponent<KnightCheckpointScript>();
		knightMR = knightMovement.gameObject.GetComponentsInChildren<MeshRenderer>();
		squireMR = squireMovement.gameObject.GetComponentsInChildren<MeshRenderer>();

		breathParticles = transform.Find("Dragon Breath").gameObject;

		isFreezeCubes = false;

		theCamera = Camera.main.GetComponent<CameraTouchInputScript>();

		timer = 22.9f;
		isFire = false;
		isPlayedSFX = true;
		roarDuration = breathDuration + 1f;
		beginTilt = false;
		PS = transform.GetComponentsInChildren<ParticleSystem>();
	}

	void Update () 
	{
		FireBreath();
		FreezeCubes();
	}

	void FireBreath()
	{
		if(!ThrowItemScript.Variables.timeStop)
		{
			if(timer < breathCycle)
			{
				timer += Time.deltaTime;

				if (timer >= 0.5f && timer < breathCycle - 12f)
				{
					foreach(GameObject ice in CubesWithIciclesList)
					{
						ice.SetActive(false);
					}
				}

				// freeze cubes
				if (timer >= breathCycle - 12f && timer < breathCycle - roarDuration)
				{
					isFreezeCubes = true;
				}

				//before the roar
				if (timer >= breathCycle - 8f && timer < breathCycle - roarDuration)
				{
					isPlayedSFX = false;

					theCamera.shakeIntensity = 0.2f;
				}

				//the roar
				if (timer >= breathCycle - roarDuration && timer < breathCycle - breathDuration)
				{
					if (isPlayedSFX == false)
					{
						isPlayedSFX = true;

						SoundManagerScript.Instance.StopSFX();
						SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_DRAGONBREATH);
					}
				}

				//the Breath
				if ((timer >= breathCycle - breathDuration) && (timer < breathCycle))
				{
					if (!isFire)
					{
						theCamera.shakeIntensity = 0.4f;
						theCamera.shakeDuration = 5f;
					}

					isFire = true;
				}
			}

			if (timer >= breathCycle)
			{
				isFire = false;
				timer = 0f;
				beginTilt = true;
				isFreezeCubes = false;

				theCamera.shakeIntensity = 0.1f;
				isPlayedSFX = false;
			}

			if(isFire)
			{
				breathParticles.SetActive(true);
			}

			if(!isFire)
			{
				breathParticles.SetActive(false);

				if (theCamera.shakeDuration <= 0f)
				{
					theCamera.shakeIntensity = 1f; //reset back to 1 for camera
				}
			}

			if(pauseThem && isFire)
			{
				for(int i = 0; i < PS.Length; i ++)
				{
					PS[i].Play();
				}
				pauseThem = false;
			}
			MurderVictims();
		}
		else if(ThrowItemScript.Variables.timeStop && isFire)
		{
			if(!pauseThem)
			{
				for(int i = 0; i < PS.Length; i ++)
				{
					PS[i].Pause();
				}
				pauseThem = true;
			}
		}
	}

	void FreezeCubes()
	{
		if (isFreezeCubes)
		{
			foreach(GameObject ice in CubesWithIciclesList)
			{
				ice.SetActive(true);

				if ( ice.transform.localScale.z < fullyFrozenSize.z)
				{
					ice.transform.localScale += new Vector3(0f, 0f, 0.5f) * Time.deltaTime;
				}
			}
		}
		else
		{
			foreach(GameObject ice in CubesWithIciclesList)
			{
				if ( ice.transform.localScale.z > initSize.z)
				{
					ice.transform.localScale -= new Vector3(0f, 0f, 3f) * Time.deltaTime;
				}
			}
		}
	}

	void MurderVictims()
	{
		if(beginTilt)
		{
			beginTilt = false;

			for(int i = 0; i < victims.Count; i ++)
			{
				if(victims[i].CompareTag("Knight"))
				{
					knightMovement.tilt = true;
				}
				else if(victims[i].CompareTag("Player"))
				{
					squireMovement.tilt = true;
				}
				if(victims[i].CompareTag("Enemy") && victims[i].GetComponent<EnemyAIScript>().EnemyType != EnemyAIScript.EnemyClass.Swarm)
				{
					victims[i].GetComponent<EnemyAIScript>().tilt = true;
				}
			}
			victims.Clear();
		}
	}

	void OnTriggerEnter(Collider bricks)
	{
		if (bricks.CompareTag("FreezeableCube"))
		{
			GameObject ice = (GameObject)Instantiate(icicle, bricks.transform);
			ice.transform.localPosition = new Vector3(-0.25f, 0.4f, 0.75f); // offset
			CubesWithIciclesList.Add(ice);
		}
		if (bricks.CompareTag("FreezeableHalfCube"))
		{
			GameObject ice = (GameObject)Instantiate(icicle, bricks.transform);
			ice.transform.position = new Vector3(bricks.transform.position.x, bricks.transform.position.y, bricks.transform.position.z); // offset
			ice.transform.localPosition = new Vector3(-0.25f, 0.4f, 0.75f); // offset
			CubesWithIciclesList.Add(ice);
		}
	}

	void OnTriggerExit(Collider bricks)
	{
		if (bricks.CompareTag("FreezeableHalfCube"))
		{
			//====================   What I need to do:   =========================
			//1. create a list to store all detached children    //IcicleRemovalList.Add(bricks.transform.GetChild(0);  

			//2. remove the icicles from CubesWithIciclesList 

			// CubesWithIciclesList.Find(brick.gameObject);  //im trying to grab the object in the list to remove it, but doesn work 

			//bricks.transform.DetachChildren(); // if enabled now will have missing reference error because CubesWithIciclesList tries to get it but cant find it
		}
	}

	void OnTriggerStay(Collider coll)
	{
		if((coll.CompareTag("Enemy") || coll.CompareTag("Knight") || coll.CompareTag("Player")) && timer >= 26.0f && !victims.Contains(coll.gameObject))
		{
			if(coll.CompareTag("Enemy"))
			{
				if(coll.GetComponent<EnemyAIScript>().EnemyType != EnemyAIScript.EnemyClass.Swarm &&
					coll.GetComponent<EnemyAIScript>().EnemyType != EnemyAIScript.EnemyClass.Phantom)
				{
					victims.Add(coll.gameObject);
					coll.GetComponent<EnemyAIScript>().stunTimer = 500.0f;

					MeshRenderer[] enemyMR = coll.GetComponentsInChildren<MeshRenderer>();
					for(int i = 0; i < enemyMR.Length; i ++)
					{
						enemyMR[i].material = frozenPlain;
					}
				}
				else if(coll.GetComponent<EnemyAIScript>().EnemyType == EnemyAIScript.EnemyClass.Swarm)
				{
					coll.gameObject.SetActive(false);
				}
			}
			else if(coll.CompareTag("Knight"))
			{
				victims.Add(coll.gameObject);
				knightMovement.stunTimer = 500.0f;

				for(int i = 0; i < knightMR.Length; i ++)
				{
					knightMR[i].material = frozenKnight;
				}
				knightRespawn.frozen = true;
			}
			else if(coll.CompareTag("Player"))
			{
				victims.Add(coll.gameObject);
				for(int i = 0; i < squireMR.Length; i ++)
				{
					squireMR[i].material = frozenSquire;
				}
				squireMovement.frozen = true;
				knightRespawn.frozen = true;
			}
		}
	}
}
