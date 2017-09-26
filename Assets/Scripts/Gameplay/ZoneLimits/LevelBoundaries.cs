using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBoundaries : MonoBehaviour
{
    public GameObject win;

    void OnTriggerExit(Collider other)
    {
		if (other.tag == "Player") 
		{
			Player m_player = other.GetComponent<Player> ();
			m_player.m_damageData.m_recive = true;
			m_player.m_damageData.m_damage = 20;
			m_player.m_damageData.m_respawn = true;
		} else if (other.tag == "Boss") 
		{
			other.transform.GetChild (0).gameObject.SetActive (true);
            win.SetActive(true);
		}
        else
        {
            int enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
            int throwLayer = 1 << LayerMask.NameToLayer("ThrowableObject");
            if (other.gameObject.layer == enemyLayer || other.gameObject.layer == throwLayer)
                other.gameObject.SetActive(false);
        }
    }
}
