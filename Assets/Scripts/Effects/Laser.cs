using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {
	public LineRenderer laser;
	public Transform target;
	public Transform firePoint;
	public ParticleSystem impactEffect;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (target == null) 
		{
			laser.enabled = false;
			//impactEffect.Stop ();
		}
		else 
		{
			laser.enabled = true;
			//impactEffect.Play ();
			laser.SetPosition (0, firePoint.position);
			laser.SetPosition (1, target.position);
		}
	}
}
