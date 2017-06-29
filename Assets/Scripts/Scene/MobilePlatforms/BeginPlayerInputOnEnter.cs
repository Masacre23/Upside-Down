using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginPlayerInputOnEnter : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            player.m_negatePlayerInput = false;
        }
    }
}
