using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWallAttractor : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        GameObjectGravity gravity = other.GetComponent<GameObjectGravity>();
        if (gravity)
        {
            gravity.EnterGravityWallZone();
        }
    }

    void OnTriggerExit(Collider other)
    {
        GameObjectGravity gravity = other.GetComponent<GameObjectGravity>();
        if (gravity)
        {
            gravity.ExitGravityWallZone();
        }
    }
}
