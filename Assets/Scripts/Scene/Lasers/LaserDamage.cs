using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDamage : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        int player = LayerMask.NameToLayer("Player");
        if (other.gameObject.layer == player)
        {
            Player go_player = other.gameObject.GetComponent<Player>();
            if(go_player != null)
            {
                go_player.m_damage.m_damage = 20;
                go_player.m_damage.m_recive = true;
            }
        }
    }
}
