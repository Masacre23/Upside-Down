using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopOnEnter : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
