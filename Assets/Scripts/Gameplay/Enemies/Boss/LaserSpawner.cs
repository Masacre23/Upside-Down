using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSpawner : MonoBehaviour {
    public GameObject[] lasers;
    public GameObject LaserPrefab;
    public Transform[] origPos;
    public float spawnRate = 3600.0f;
    public float time;
	// Use this for initialization
	void Start () {
        lasers[0] = transform.GetChild(0).gameObject;
        lasers[1] = transform.GetChild(1).gameObject;
        lasers[2] = transform.GetChild(2).gameObject;
        lasers[3] = transform.GetChild(3).gameObject;

        /*origPos[0] = lasers[0].transform;
        origPos[1] = lasers[1].transform;
        origPos[2] = lasers[2].transform;
        origPos[3] = lasers[3].transform;*/
    }
	
	// Update is called once per frame
	void Update () {
		if(transform.childCount < 4)
        {
            for(int i = 0; i < 4; ++i)
            {
               // if (lasers[i] != transform.GetChild(i))
               //     lasers[i] = null;

               // if(lasers[i] == null)
               if(!lasers[i] || lasers[i] == null || System.Object.Equals(lasers[i], null))
                {
                    time += Time.deltaTime;
                    if (time > spawnRate)
                    {
                        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
                        if (screenPoint.z < 0 || screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1) //if is not viewing
                        {
                            lasers[i] = (GameObject)Instantiate(LaserPrefab, origPos[i].position, Quaternion.identity, transform);
                            lasers[i].GetComponent<OnCollisionBigPlatform>().lasers = gameObject;
                            time = 0;
                        }
                    }
                    break;
                }
            }
        }
	}
}
