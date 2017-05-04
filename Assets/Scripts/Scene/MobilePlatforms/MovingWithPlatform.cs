using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is to move objects or characters with moving platforms. 
//This is done by making them the platform's child.

//More tags should be added if needed (enemies, ...)
public class MovingWithPlatform : MonoBehaviour
{
    private Vector3 position;
    private Player player = null;

    private void Update()
    {
        if (player != null)
        {
            Vector3 offset = transform.position - position;
            position = transform.position;
            player.m_offset += offset;
        }
    }

    void OnTriggerEnter(Collider col)
    {

        if (col.tag == "Player")
        {
            position = transform.position;
            player = col.gameObject.GetComponent<Player>();
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player" )
            player = null;
    }
}
