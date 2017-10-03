using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArea : MonoBehaviour
{
	List<Enemy> enemies;
    int layerEnemy;
	public GameObject iceBlocks;
	public int numEnemies;

	void Awake()
	{
		enemies = new List<Enemy>();
        layerEnemy = LayerMask.NameToLayer("EnemySnailIce");
		//ScanForItems();
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player") 
		{
			foreach(Enemy enemy in enemies)
			{
                enemy.player = col.gameObject;
				enemy.m_enemyArea = this.gameObject;
             }
			if (iceBlocks)
				iceBlocks.SetActive (true);
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
