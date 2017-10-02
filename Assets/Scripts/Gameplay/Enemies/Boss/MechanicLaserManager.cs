using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicLaserManager : MonoBehaviour {
    public GameObject laser;
    public GameObject bossSceneManager;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        laser.transform.GetChild(0).GetComponent<Laser>().bossSceneManager = null;
    }

    private void OnDisable()
    {
        laser.transform.GetChild(0).GetComponent<Laser>().laser.enabled = false;
        laser.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
        laser.transform.GetChild(0).GetComponent<Laser>().bossSceneManager = bossSceneManager;
        laser.GetComponent<GameObjectGravity>().enabled = true;
    }
}
