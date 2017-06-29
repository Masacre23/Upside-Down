using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectorByLayer : MonoBehaviour
{
    int m_enemyLayer;
    public List<Enemy> m_enemies;
    List<Enemy> m_temp;

    void Awake()
    {
        m_enemies = new List<Enemy>();
        m_temp = new List<Enemy>();
        m_enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    void Update()
    {
        //bool enemyKilled = false;
        //foreach (Enemy enemy in m_enemies)
        //{
        //    if (enemy.m_health < 0.0f)
        //    {
        //        enemyKilled = true;
        //        break;
        //    }
        //}

        //if (enemyKilled)
        //{
        //    foreach (Enemy enemy in m_enemies)
        //    {
        //        if (enemy.m_health > 0.0f)
        //            m_temp.Add(enemy);
        //    }

        //    m_enemies.Clear();
        //    m_enemies = m_temp;
        //    m_temp.Clear();
        //}
    }

    void OnDisable()
    {
        m_enemies.Clear();
    }

    void OnTriggerEnter(Collider col)
    {
        //Check if the collision collider layer is in m_targetLayers
        if (m_enemyLayer == col.gameObject.layer)
        {
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy && enemy.m_health > 0.0f && !m_enemies.Contains(enemy))
            {
                m_enemies.Add(enemy);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (m_enemyLayer == col.gameObject.layer)
        {
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy && m_enemies.Contains(enemy))
            {
                m_enemies.Remove(enemy);
            }
        }   
    }

}
