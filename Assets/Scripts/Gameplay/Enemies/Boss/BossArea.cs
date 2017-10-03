using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArea : MonoBehaviour {

    GameObject player;
    GameObject boss;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        boss = GameObject.Find("Boss");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            boss.GetComponent<Boss>().m_canChase = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            boss.GetComponent<Boss>().m_canChase = false;
    }
}
