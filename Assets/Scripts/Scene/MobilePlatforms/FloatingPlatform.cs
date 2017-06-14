using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour {

    public float speed;

    private void OnCollisionStay(Collision collision)
    {
        
        if (collision.collider.tag == "Player")
        {
            Vector3 distance = this.transform.InverseTransformPoint( collision.contacts[0].point);
            Debug.Log(distance);
            this.transform.Rotate(new Vector3(-distance.y * speed * Time.deltaTime, distance.x * speed * Time.deltaTime, 0.0f));
        }
    }
}
