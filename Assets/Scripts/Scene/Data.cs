using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour {
	public bool spanish;

	void Awake()
	{
		DontDestroyOnLoad (this);
	}

	public void ToggleDialogues()
	{
		spanish = !spanish;
        GameObject[] dialogues = GameObject.FindGameObjectsWithTag("Dialogue");

        foreach (GameObject go in dialogues)
        {
            go.GetComponent<Dialogue>().spanish = spanish;
        }
	}
}
