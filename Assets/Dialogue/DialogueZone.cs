using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueZone : MonoBehaviour {

    bool playerInside = false;
    public GameObject dialogue;
    Player playerManager;
    bool b = false;

    void Awake()
    {
        playerManager = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Return) && playerInside && !b)
        {
            dialogue.SetActive(true);
            playerManager.m_negatePlayerInput = true;
            b = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            playerInside = false;
    }
}
