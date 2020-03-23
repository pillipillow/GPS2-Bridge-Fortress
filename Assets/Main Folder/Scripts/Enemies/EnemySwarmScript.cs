using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwarmScript : MonoBehaviour
{
	public int colorPhase;
	private int whichColor;

	private float r;
	private float g;
	private float b;
	private float timer;

	public bool inEffect;

	void Start()
	{
		colorPhase = 0;
		whichColor = 0;
		r = 1.0f;
		g = 1.0f;
		b = 1.0f;
		inEffect = true;
	}

	void Update()
	{
		if(inEffect)
		{
			timer += Time.deltaTime;

			if(timer > 60.0f && colorPhase == 1)
			{
				colorPhase = 2;
				timer = 0.0f;
			}

			if(colorPhase == 1)
			{
				if(whichColor == 0)
				{
					g -= Time.deltaTime / 2.0f;
					b -= Time.deltaTime / 2.0f;

					if(g <= 0.0f && b <= 0.0f)
					{
						whichColor += 1;
					}
				}
				else if(whichColor == 1)
				{
					g += Time.deltaTime / 2.0f;
					r -= Time.deltaTime / 2.0f;

					if(g >= 1.0f && r <= 0.0f)
					{
						whichColor += 1;
					}
				}
				else if(whichColor == 2)
				{
					b += Time.deltaTime / 2.0f;

					if(b >= 1.0f)
					{
						whichColor += 1;
					}
				}
				else if(whichColor == 3)
				{
					if(b > 0.86f)
					{
						b -= Time.deltaTime / 2.0f;
					}
					g -= Time.deltaTime / 2.0f;
					r += Time.deltaTime / 2.0f;

					if(r >= 1.0f)
					{
						whichColor += 1;
					}
				}
				else if(whichColor == 4)
				{
					if(b > 0.0f)
					{
						b -= Time.deltaTime / 2.0f;
					}
					g += Time.deltaTime / 2.0f;

					if(g >= 1.0f)
					{
						whichColor = 0;
					}
				}
			}

			ParticleSystem.Particle[] particles = new ParticleSystem.Particle[GetComponent<ParticleSystem>().particleCount];
			var particleCount = GetComponent<ParticleSystem>().GetParticles(particles);
			int i = 0;

			while (i < particleCount)
			{
				if(colorPhase != 1)
				{
					particles[i].startColor = Color.yellow;
				}
				else
				{
					particles[i].startColor = new Color(r, g, b);
				}

				i++;
			}
			GetComponent<ParticleSystem>().SetParticles(particles, particleCount);
		}
		else
		{
			GetComponent<ParticleSystem>().Stop();
		}
	}
}
