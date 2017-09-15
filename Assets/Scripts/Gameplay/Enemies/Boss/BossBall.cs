using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBall : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player") 
		{
			col.gameObject.GetComponent<Player> ().m_damageData.m_recive = true;
			col.gameObject.GetComponent<Player> ().m_damageData.m_damage = 20;
		}

	}
}
