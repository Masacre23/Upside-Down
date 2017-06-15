using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

    public Transform destiny;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerRespawn respawn = other.gameObject.GetComponent<PlayerRespawn>();
            if(respawn != null)
            {
                respawn.ReSpawn(destiny);
            }
        }
    }
}
