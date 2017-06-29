using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArea : MonoBehaviour
{
	List<Enemy> enemies;
    int layerEnemy;

	void Awake()
	{
		enemies = new List<Enemy>();
        layerEnemy = LayerMask.NameToLayer("Enemy");
		//ScanForItems();
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player") 
		{
			foreach(Enemy enemy in enemies)
			{
                enemy.player = col.gameObject;
                if (!enemy.m_isSleeping)
                {
                    enemy.m_currentState.OnExit();
                    enemy.m_currentState = enemy.m_Following;
                    enemy.m_currentState.OnEnter();
                }
             }
		}

        if (col.tag == "EnemySnail" && col.name != "CenterSpineFather")
        {
            Enemy enemy = col.GetComponent<Enemy>();
            enemies.Add(enemy);
        }
    }

	void OnTriggerExit(Collider col)
	{
		if (col.tag == "Player") 
		{
			foreach (Enemy enemy in enemies)
			{
                enemy.player = null;
                enemy.m_currentState.OnExit();
                enemy.m_currentState = enemy.m_Idle;
                enemy.m_currentState.OnEnter();
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
