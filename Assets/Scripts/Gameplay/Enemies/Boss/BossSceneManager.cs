using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSceneManager : MonoBehaviour {
	public int phase = 0;
	GameObject boss;
	int []sizes = {10, 5, 1};
	bool scaling = false;

	// Use this for initialization
	void Start () {
		boss = GameObject.Find ("Boss");
	}
	
	// Update is called once per frame
	void Update () {

	}

	public IEnumerator ChangeBossScale(GameObject laser)
	{
		if (!scaling) 
		{
			scaling = true;
			while (boss.transform.localScale.x > sizes [phase]) 
			{
				boss.transform.localScale -= new Vector3 (Time.deltaTime, Time.deltaTime, Time.deltaTime);
				yield return 0;
			}
			scaling = false;
			//laser.GetComponent<Laser> ().enabled = false;
			laser.transform.GetChild(1).gameObject.SetActive(false);
			phase++;
		}
	}
}
