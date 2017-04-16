using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        if (col.name == "Player")
        {
            Player player = col.gameObject.GetComponent<Player>();
            if (player != null)
                player.m_checkPoint = gameObject.transform;
        }
    }
}
