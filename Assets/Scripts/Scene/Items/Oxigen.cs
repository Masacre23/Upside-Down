using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oxigen : MonoBehaviour {
    public float m_oxigen = 60;

    private Player m_player = null;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_player != null)
        {
            m_player.m_oxigen += m_oxigen;
            if (m_player.m_oxigen > m_player.m_maxOxigen)
                m_player.m_oxigen = m_player.m_maxOxigen;
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
