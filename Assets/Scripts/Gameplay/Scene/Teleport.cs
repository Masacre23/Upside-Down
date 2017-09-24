using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Teleport m_destiny;
    public Transform m_thisLocationSpawning;

    private int m_playerLayer;
    private int m_terrainLayer;
    private int m_generalLayer;
    private int m_watterLayer;
    private SoundEffects sounds;

	void Start ()
    {
        m_playerLayer = LayerMask.NameToLayer("Player");
        m_terrainLayer = LayerMask.NameToLayer("Terrain");
        m_watterLayer = LayerMask.NameToLayer("HarmfulTerrain");
        sounds = GetComponent<SoundEffects>();
        sounds.PlaySoundLoop("Idle");
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == m_playerLayer)
        {
            Transform destinyPosition = m_destiny.m_thisLocationSpawning;
            Player player = other.gameObject.GetComponent<Player>();
            player.m_negatePlayerInput = true;
            player.m_currentZone = null;
            player.m_camController.CameraFollowingOff();
            Physics.IgnoreLayerCollision(m_playerLayer, m_watterLayer, true);
            Physics.IgnoreLayerCollision(m_playerLayer, m_terrainLayer, true);
            

            if (player.m_playerRespawn)
            {
                player.m_playerRespawn.ReSpawn(destinyPosition);
                sounds.PlaySound("Pass");
            }
        }
    }
}
