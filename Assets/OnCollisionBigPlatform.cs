using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionBigPlatform : MonoBehaviour {
    public GameObject lasers;

    private void OnCollisionEnter(Collision collision)
    {
        // if(collision.gameObject.tag == "BigPlatform")
        if (collision.gameObject.tag == "BigPlatform")
        {
            //Destroy(other.gameObject);
            //  transform.parent = null;
            for (int i = 0; i < 4; i++)
            {
                if (lasers.GetComponent<LaserSpawner>().lasers[i] != null && collision.gameObject.transform.position.x == lasers.GetComponent<LaserSpawner>().lasers[i].gameObject.transform.position.x)
                {
                    GameObject.Find("Lasers").GetComponent<LaserSpawner>().lasers[i] = null;
                    this.transform.parent = null;
                }
            }
        }
    }
}
