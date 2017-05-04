using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {
    public float m_health = 20;

    private Player m_player = null;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(m_player != null)
        {
            m_player.m_health += m_health;
            if (m_player.m_health > m_player.m_maxHealth)
                m_player.m_health = m_player.m_maxHealth;
            gameObject.SetActive(false);
        }
	}

    private void OnTriggerEnter(Collider col)
    {
        int player = LayerMask.NameToLayer("Player");
        if (col.gameObject.layer == player)
            m_player = col.gameObject.GetComponent<Player>();
    }
}
