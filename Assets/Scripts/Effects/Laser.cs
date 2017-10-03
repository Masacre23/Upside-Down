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
    private LazerSoundEffects m_sound;
    private bool m_on = false;
    public float laserDistance = 4.0f;

	// Use this for initialization
	void Start () {
		bossSceneManager = GameObject.Find ("BossSceneManager");
        if (bossSceneManager)
            target.gameObject.SetActive(false);
        m_sound = GetComponentInParent<LazerSoundEffects>();
	}
	
	// Update is called once per frame
	void Update () {
        bool on = m_on;
        if (bossSceneManager && !transform.parent.GetComponent<ThrowableObject>().m_isCarring)
        {
            laser.enabled = false;
            transform.GetChild(1).gameObject.SetActive(false);
        }

        if (bossHitted && !transform.parent.GetComponent<ThrowableObject> ().m_isCarring)
        {
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
            if (screenPoint.z < 0 || screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1) //if is not viewing
            {
                //transform.parent.gameObject.SetActive(false);
                bossSceneManager.GetComponent<BossSceneManager>().laser = null;
                Destroy(transform.parent.gameObject);
                //transform.parent.gameObject.transform.parent = null;
                //transform.parent.gameObject.SetActive(false);
            }
        }
		if (target == null) 
		{
			laser.enabled = false;
			impactEffect.Stop ();
            on = false;
		}
		else 
		{
			if (dinamicLaser) 
			{
                time += Time.deltaTime;
				RaycastHit hit;
				if (Physics.Raycast (transform.position, -transform.up, out hit, bossSceneManager? 20 : laserDistance) && (time < maxTime || maxTime == 0) && !bossHitted) 
				{
					if (!bossSceneManager) 
					{
						Draw (hit.point);
                        on = true;
					} else if (bossSceneManager && hit.collider.tag == "Boss") 
					{
						if (!bossHitted && transform.parent.GetComponent<ThrowableObject> ().m_isCarring) {
                            target.gameObject.SetActive(true);
                            Draw (hit.point);
                            on = true;
							hitting = true;
                            bossSceneManager.GetComponent<BossSceneManager>().laser = this;
							StartCoroutine (bossSceneManager.GetComponent<BossSceneManager> ().ChangeBossScale ());
                        }else
                        {
                            on = false;
                        }
					}
				} else 
				{
					hitting = false;
                    if (drawAlways && (time < maxTime || maxTime == 0))
                    {
                        transform.GetChild(1).gameObject.SetActive(true);
						Draw ();
                        on = true;
                    }
                    else
                    {
                        laser.enabled = false;
                        transform.GetChild(1).gameObject.SetActive(false);
                        on = false;
                    }
				//	impactEffect.Stop ();
				}
                if (time > 2 * maxTime)
                    time = 0;
			}
			else 
			{
				Draw ();
                on = true;
			}
		}
        if(on != m_on)
        {
            if (on)
                m_sound.PlayLazer();
            else
                m_sound.StopLazer();
            m_on = on;
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
