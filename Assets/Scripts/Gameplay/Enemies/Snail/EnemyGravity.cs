using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGravity : MonoBehaviour {

	Enemy m_enemy;
	GameObjectGravity m_enemyGravity;

	// Use this for initialization
	void Start () {
		m_enemy = GetComponent<Enemy> ();
		m_enemyGravity = GetComponent<GameObjectGravity> ();
	}

	void FixedUpdate () {
		
	}
}
