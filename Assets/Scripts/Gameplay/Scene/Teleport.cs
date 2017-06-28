using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Teleport m_destiny;
    public Transform m_thisLocationSpawning;

    int playerLayer;

	void Start ()
    {
        playerLayer = LayerMask.NameToLayer("Player");
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            Transform destinyPosition = m_destiny.m_thisLocationSpawning;
            Player player = other.gameObject.GetComponent<Player>();
            player.m_negatePlayerInput = true;

            if(player.m_playerRespawn)
                player.m_playerRespawn.ReSpawn(destinyPosition);
        }
    }
}
