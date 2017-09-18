using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSceneManager : MonoBehaviour {
	public int phase = 0;
	GameObject boss;
	GameObject player;
	int []sizes = {10, 5, 1};
	bool scaling = false;
	public GameObject basePlatform;
	public GameObject pointReference;
	int[] points = { 6, 16, 16};
	public GameObject[] scenes;
	public bool activateCameras;
	public int numScene;

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
		} else 
		{
			player.GetComponent<Player> ().m_paused = false;
			scenes [numScene].SetActive (false);
		}
	}

	public IEnumerator ChangeBossScale(Laser laser)
	{
		if (!scaling) 
		{
			scaling = true;
			activateCameras = true;
			while (boss.transform.localScale.x > sizes [phase] || ((phase == 1)? pointReference.transform.position.y < points[phase] : 
				pointReference.transform.position.y > points[phase] )) 
			{
				if(phase > 1? laser.hitting : true)
				{
				if(boss.transform.localScale.x > sizes [phase])
					boss.transform.localScale -= new Vector3 (Time.deltaTime, Time.deltaTime, Time.deltaTime);
				if ((phase == 1)? pointReference.transform.position.y < points[phase] : 
					pointReference.transform.position.y > points[phase]) 
				{
					if(phase == 1)
						pointReference.transform.position += new Vector3 (0, Time.deltaTime, 0);
					else
						pointReference.transform.position -= new Vector3 (0, Time.deltaTime, 0);
					
					basePlatform.transform.position += new Vector3 (0, Time.deltaTime * (phase + 1) / 5.1f, 0);
				}
				}
				yield return 0;
			}
			laser.bossHitted = true;
			//laser.transform.GetChild(1).gameObject.SetActive(false);
			while (phase == 1 && basePlatform.transform.position.y < 9) 
			{
				basePlatform.transform.position += new Vector3 (0, Time.deltaTime * (phase + 1) / 5.1f, 0);
				yield return 0;
			}
			/*while (pointReference.transform.position.y > points[phase]) 
			{
				pointReference.transform.position -= new Vector3(0, Time.deltaTime, 0);
				basePlatform.transform.position += new Vector3 (0, Time.deltaTime * (phase+1)/5, 0);
				yield return 0;
			}*/
			scaling = false;
			phase++;
			boss.GetComponent<Boss> ().m_phase = phase;
			boss.GetComponent<Boss> ().m_animator.SetInteger ("Phase", phase);
			activateCameras = false;
		}
	}
}
