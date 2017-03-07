using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
    int numStaticCameras;
    int actualCamera = 0;
    GameObject staticCameras;

	void Start () {
        staticCameras = this.gameObject.transform.GetChild(0).gameObject;
        numStaticCameras = staticCameras.transform.childCount;
	}
	
	void Update () {
		if(Input.GetKeyDown("v"))
        {
            staticCameras.transform.GetChild(actualCamera).gameObject.SetActive(false);
            actualCamera++;
            if(actualCamera >= numStaticCameras)
            {
                actualCamera = 0;
            }
            staticCameras.transform.GetChild(actualCamera).gameObject.SetActive(true);
        }
	}

    private void OnEnable()
    {
        GameObject.Find("Player").SetActive(false);
    }

    private void OnDisable()
    {
        GameObject.Find("Player").SetActive(true);
    }
}
