using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingOnWater : MonoBehaviour
{
    public GameObject m_prefabEffect;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("OnWater"))
        {
            Vector3 direction = other.transform.position - transform.position;

            EffectsManager.Instance.GetEffect(m_prefabEffect, other.transform.position, direction.normalized, transform);

            SoundEffects sound = other.gameObject.GetComponent<SoundEffects>();
            if(sound != null)
            {
                sound.PlaySound("SplashWater");
            }

            if (other.tag == "Player")
            {
                Player player = other.GetComponent<Player>();
                if (player.m_currentState == player.m_onAir)
                {
                    player.m_negatePlayerInput = true;
                }
            }
        } 
    }
}
