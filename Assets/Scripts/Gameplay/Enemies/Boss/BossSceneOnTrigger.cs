using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSceneOnTrigger : MonoBehaviour {
    public GameObject prefab;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("YOLOOOOOO");
        Instantiate(prefab, transform.position, Quaternion.identity);
    }
}
