using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is to move objects or characters with moving platforms. 
//This is done by making them the platform's child.

//More tags should be added if needed (enemies, ...)
public class MovingWithPlatform : MonoBehaviour
{

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player" || col.tag == "GravityAffected")
            col.transform.parent = transform;
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player" || col.tag == "GravityAffected")
            col.transform.parent = null;
    }
}
