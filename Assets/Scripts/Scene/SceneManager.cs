using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {
    [SerializeField] GameObject cameras;
    bool b = false;

	void Update () {
		if(Input.GetKeyDown(KeyCode.V))
        {
            b = !b;
            cameras.SetActive(b);
        }
	}
}
