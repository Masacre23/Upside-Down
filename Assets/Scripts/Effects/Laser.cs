using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {
	public LineRenderer laser;
	public Transform target;
	public Transform firePoint;
	public ParticleSystem impactEffect;
	public bool dinamicLaser;
	public float particleRate = 0.05f; //20 particles per second
	public bool drawAlways = false;
	float particleTime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (target == null) 
		{
			laser.enabled = false;
			impactEffect.Stop ();
		}
		else 
		{
			if (dinamicLaser) 
			{
				RaycastHit hit;
				if (Physics.Raycast (transform.position, -transform.up, out hit, 4)) 
				{
					laser.enabled = true;
					if (particleTime >= particleRate) 
					{
                        impactEffect.Play();
						impactEffect.Emit (1);
						particleTime = 0;
					}
					laser.SetPosition (0, firePoint.position);
					target.position = hit.point;
					laser.SetPosition (1, target.position);
				} else 
				{
					if (drawAlways) {
						laser.enabled = true;
						particleTime += Time.deltaTime;
						if (particleTime >= particleRate) 
						{
							impactEffect.Emit (1);
							particleTime = 0;
						}
						laser.SetPosition (0, firePoint.position);
						laser.SetPosition (1, target.position);
					}
					else
						laser.enabled = false;
				//	impactEffect.Stop ();
				}
			}
			else 
			{
				laser.enabled = true;
				particleTime += Time.deltaTime;
				if (particleTime >= particleRate) 
				{
					impactEffect.Emit (1);
					particleTime = 0;
				}
				laser.SetPosition (0, firePoint.position);
				laser.SetPosition (1, target.position);
			}
		}
	}
}
