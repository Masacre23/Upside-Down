using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableModel : MonoBehaviour
{
    GameObject m_playerModel;

	// Use this for initialization
	void Start ()
    {
        GameObject player = GameObject.Find("Player");
        if (player)
            m_playerModel = player.transform.FindChild("Model").gameObject;
	}
	
	void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            m_playerModel.SetActive(false);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            m_playerModel.SetActive(true);
    }
}
