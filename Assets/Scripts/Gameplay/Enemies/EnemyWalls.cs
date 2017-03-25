using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalls : MonoBehaviour {

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "EnemySnail") 
		{
			Enemy enemy = col.gameObject.GetComponent<Enemy> ();
			enemy.m_currentState.OnExit ();
			enemy.m_currentState = enemy.m_Changing;
			enemy.m_currentState.OnEnter ();
		}
	}
}
