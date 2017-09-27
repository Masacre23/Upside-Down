using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSceneManager : MonoBehaviour {
	public int phase = 0;
	GameObject boss;
	GameObject player;
	float []sizes = {10, 5, 1, 0.5f};
	bool scaling = false;
	public GameObject basePlatform;
	public GameObject pointReference;
	int[] points = { 6, 16, 16, 16};
	public GameObject[] scenes;
	public bool activateCameras;
	public int numScene;
    public GameObject asteroids;
    public GameObject[] platforms;
    bool lastCorrutineHasEnded;

	// Use this for initialization
	void Start () {
		boss = GameObject.Find ("Boss");
		player = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (activateCameras && phase < 2) 
		{
			player.GetComponent<Player> ().m_paused = true;
			scenes [numScene].SetActive (true);
            player.transform.parent = platforms[numScene].transform;
		} else 
		{
			player.GetComponent<Player> ().m_paused = false;
			scenes [numScene].SetActive (false);
            player.transform.parent = null;

        }
	}

	public IEnumerator ChangeBossScale(Laser laser)
	{
		if (!scaling) 
		{
            lastCorrutineHasEnded = false;
            scaling = true;
			activateCameras = true;

			if (phase < 2) 
			{
				boss.GetComponent<Boss> ().Stun ();
				player.transform.GetChild (0).GetChild (0).GetComponent<LookAtBoss> ().enabled = true;

            }

            StartCoroutine(Platforms(phase, laser));
            while (boss.transform.localScale.x > sizes[phase])
            {
                if (phase > 1 ? laser.hitting : true)
                    boss.transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime);
                yield return 0;
            }

                //if(phase != 2)
                laser.bossHitted = true;
			activateCameras = false;
            player.transform.GetChild (0).GetChild (0).GetComponent<LookAtBoss> ().enabled = false;
			switch (phase) 
			{
			case 1:
				player.transform.GetChild (0).GetChild (0).transform.localRotation = Quaternion.Euler (0, 0, 0);
				break;
			case 2:
				player.transform.GetChild (0).GetChild (0).transform.localRotation = Quaternion.Euler (5, 0, 0);
				break;
			default:
				player.transform.GetChild (0).GetChild (0).transform.localRotation = Quaternion.Euler (-15, 0, 0);
				break;
			}

            scaling = false;
            if (phase < 2)
                boss.GetComponent<Boss>().Stun();

            StartCoroutine(Asteroids(phase));
            StartCoroutine(BigPlatform(phase));

            phase++;
            boss.GetComponent<Boss>().m_phase = phase;
            boss.GetComponent<Boss>().m_animator.SetInteger("Phase", phase);

            while (!lastCorrutineHasEnded)
                yield return 0;

            
		}
	}

    private IEnumerator Platforms(int p, Laser laser)
    {
        while (((p == 1) ? pointReference.transform.position.y < points[p] : pointReference.transform.position.y > points[p]))
        {
            if (p > 1 ? laser.hitting : true)
            {
                if (p == 1)
                    pointReference.transform.position += new Vector3(0, Time.deltaTime, 0);
                else
                    pointReference.transform.position -= new Vector3(0, Time.deltaTime, 0);

                basePlatform.transform.position += new Vector3(0, Time.deltaTime * (p + 1) / 5.1f, 0);
            }
            yield return 0;
        }
    }
    
    private IEnumerator Asteroids(int p)
    {
        while (p < 2 && p == 0 ? asteroids.transform.localPosition.y < 2 : asteroids.transform.localPosition.y > 0)
        {
            asteroids.transform.localPosition += new Vector3(0, p == 0 ? Time.deltaTime : -Time.deltaTime, 0);
            asteroids.transform.GetChild(0).GetComponent<SgtSimpleOrbit>().Radius += p == 0 ? -Time.deltaTime / 2 : Time.deltaTime / 2;
            asteroids.transform.GetChild(1).GetComponent<SgtSimpleOrbit>().Radius += p == 0 ? -Time.deltaTime / 2 : Time.deltaTime / 2;
            asteroids.transform.GetChild(2).GetComponent<SgtSimpleOrbit>().Radius += p == 0 ? -Time.deltaTime / 2 : Time.deltaTime / 2;
            yield return 0;
        }
    }

    private IEnumerator BigPlatform(int p)
    {
        while (p == 1 && basePlatform.transform.position.y < 9)
        {
            basePlatform.transform.position += new Vector3(0, Time.deltaTime * (p + 1) / 5.1f, 0);
            yield return 0;
        }
        lastCorrutineHasEnded = true;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
            boss.GetComponent<Boss>().m_animator.SetBool("BattleHasStarted", true);
    }
}
