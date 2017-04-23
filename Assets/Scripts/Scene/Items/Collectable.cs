using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {
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
