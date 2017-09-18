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
    public float maxTime = 0;
    public  float time;
	public GameObject bossSceneManager;
	public bool bossHitted = false;
	public bool hitting = false;

	// Use this for initialization
	void Start () {
		bossSceneManager = GameObject.Find ("BossSceneManager");
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
                time += Time.deltaTime;
				RaycastHit hit;
				if (Physics.Raycast (transform.position, -transform.up, out hit, bossSceneManager? 20 : 4) && (time < maxTime || maxTime == 0)) 
				{
					if (!bossSceneManager) 
					{
						Draw (hit.point);
					} else if (bossSceneManager && hit.collider.tag == "Boss") 
					{
						if (!bossHitted) {
							Draw (hit.point);
							hitting = true;
							StartCoroutine (bossSceneManager.GetComponent<BossSceneManager> ().ChangeBossScale (this));
						}
					} 
				} else 
				{
					hitting = false;
                    if (drawAlways && (time < maxTime || maxTime == 0))
                    {
                        transform.GetChild(1).gameObject.SetActive(true);
						Draw ();
                    }
                    else
                    {
                        laser.enabled = false;
                        transform.GetChild(1).gameObject.SetActive(false);
                    }
				//	impactEffect.Stop ();
				}
                if (time > 2 * maxTime)
                    time = 0;
			}
			else 
			{
				Draw ();
			}
		}
	}

	void Draw()
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

	void Draw(Vector3 hitPoint)
	{
		laser.enabled = true;
		transform.GetChild (1).gameObject.SetActive (true);
		if (particleTime >= particleRate) {
			impactEffect.Play ();
			impactEffect.Emit (1);
			particleTime = 0;
		}
		laser.SetPosition (0, firePoint.position);
		target.position = hitPoint;
		laser.SetPosition (1, target.position);
	}
}
