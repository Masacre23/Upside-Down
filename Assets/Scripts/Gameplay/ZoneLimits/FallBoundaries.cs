using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallBoundaries : MonoBehaviour
{
    private bool m_inThePlanet = true;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !m_inThePlanet)
        {
            Player m_player = other.GetComponent<Player>();
            m_player.m_damageData.m_recive = true;
            m_player.m_damageData.m_damage = 0;
            m_player.m_damageData.m_respawn = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" )
        {
            m_inThePlanet = false;
        }
    }
}
