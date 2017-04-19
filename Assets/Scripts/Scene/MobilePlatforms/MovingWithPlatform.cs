using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is to move objects or characters with moving platforms. 
//This is done by making them the platform's child.

//More tags should be added if needed (enemies, ...)
public class MovingWithPlatform : MonoBehaviour
{
    private Vector3 position;
    private Transform child = null;

    private void Update()
    {
        if (child != null)
        {
            Vector3 offset = transform.position - position;
            position = transform.position;
            child.position += offset;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player" || col.tag == "GravityAffected")
        {
            position = transform.position;
            child = col.transform;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player" || col.tag == "GravityAffected")
            child = null;
    }
}
