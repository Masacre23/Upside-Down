using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArea : MonoBehaviour
{
	List<GameObject> enemies;
	SphereCollider area;

	void Awake()
	{
		enemies = new List<GameObject> ();
		area = GetComponent<SphereCollider> ();
		//ScanForItems();
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player") 
		{
			foreach(GameObject go in enemies)
			{
				Enemy enemy = go.GetComponent<Enemy> ();
				enemy.player = col.gameObject;
				enemy.m_currentState.OnExit ();
				enemy.m_currentState = enemy.m_Following;
				enemy.m_currentState.OnEnter ();
			}
		}

        if (col.tag == "EnemySnail" && col.name != "CenterSpineFather" || col.tag == "Enemy")
        {
            enemies.Add(col.gameObject);
        }
    }

	void OnTriggerExit(Collider col)
	{
		if (col.tag == "Player") 
		{
			foreach(GameObject go in enemies)
			{
				Enemy enemy = go.GetComponent<Enemy>();
				enemy.m_currentState.OnExit ();
				enemy.m_currentState = enemy.m_Idle;
				enemy.m_currentState.OnEnter ();
			}
		}

	}

	//void ScanForItems()
	//{
	//	Vector3 center = area.transform.position + area.center;
	//	float radius = area.radius;

	//	Collider[] allOverlappingColliders = Physics.OverlapSphere (center, radius);
	//	foreach (Collider col in allOverlappingColliders) 
	//	{
	//		if (col.tag == "EnemySnail" && col.name != "CenterSpineFather" || col.tag == "Enemy")
	//			enemies.Add (col.gameObject);
	//	}
	//}
}
