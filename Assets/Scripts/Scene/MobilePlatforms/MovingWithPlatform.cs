using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is to move objects or characters with moving platforms. 
//This is done by making them the platform's child.

//More tags should be added if needed (enemies, ...)
public class MovingWithPlatform : MonoBehaviour
{

    void OnCollisionEnter(Collision colInfo)
    {
        if (colInfo.collider.tag == "Player" || colInfo.collider.tag == "GravityAffected")
            colInfo.transform.parent = transform;
    }

    void OnCollisionExit(Collision colInfo)
    {
        if (colInfo.collider.tag == "Player" || colInfo.collider.tag == "GravityAffected")
            colInfo.transform.parent = null;
    }
}
