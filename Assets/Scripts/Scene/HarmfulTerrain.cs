using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmfulTerrain : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        int playerLayer = LayerMask.NameToLayer("Player");
        if (other.gameObject.layer == playerLayer)
        {
            Player player = other.gameObject.GetComponent<Player>();
            if(player != null)
            {
                player.m_damageData.m_recive = true;
                player.m_damageData.m_damage = 20;
                player.m_damageData.m_respawn = true;
                player.m_negatePlayerInput = true;
            }
        }
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        if (other.gameObject.layer == enemyLayer)
        {
            ThrowableObject throwableObject = other.gameObject.GetComponent<ThrowableObject>();
            if (throwableObject != null)
            {
                throwableObject.StopMovingObject(true);
            }
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.m_damageData.m_recive = true;
                enemy.m_damageData.m_damage = (int)(enemy.m_health + 1);
            }
        }
        int throwableLayer = LayerMask.NameToLayer("ThrowableObject");
        if (other.gameObject.layer == throwableLayer)
        {
            ThrowableObject throwableObject = other.gameObject.GetComponent<ThrowableObject>();
            if(throwableObject != null)
            {
                throwableObject.StopMovingObject(true);
            }
        }
    }
}
