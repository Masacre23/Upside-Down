using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBoundaries : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player m_player = other.GetComponent<Player>();
            m_player.m_damage.m_recive = true;
            m_player.m_damage.m_damage = 20;
            m_player.m_damage.m_respawn = true;
        }
    }
}
