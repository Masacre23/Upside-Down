using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
    int numStaticCameras;
    int actualCamera = 0;
    GameObject staticCameras;
    GameObject godModeCamera;
    GameObject player;

	void Start () {
        staticCameras = this.gameObject.transform.GetChild(0).gameObject;
        godModeCamera = this.gameObject.transform.GetChild(2).gameObject;
        numStaticCameras = staticCameras.transform.childCount;
    }
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space) && staticCameras.activeInHierarchy)
        {
            staticCameras.transform.GetChild(actualCamera).gameObject.SetActive(false);
            actualCamera++;
            if(actualCamera >= numStaticCameras)
            {
                actualCamera = 0;
            }
            staticCameras.transform.GetChild(actualCamera).gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            staticCameras.SetActive(true);
            godModeCamera.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            staticCameras.SetActive(false);
            godModeCamera.SetActive(true);
        }
    }

    private void OnEnable()
    {
        player = GameObject.Find("Player");
        player.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        player.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
