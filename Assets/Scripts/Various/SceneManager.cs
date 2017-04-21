using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {
    [SerializeField] GameObject cameras;
	[SerializeField] GameObject gameoverPanel;
    bool b = false;

	void Update () {
		if(Input.GetKeyDown(KeyCode.V))
        {
            b = !b;
            cameras.SetActive(b);
        }
	}

	public void GameOver()
	{
		gameoverPanel.SetActive (true);
	}

	public void BackToMenu()
	{
		Application.LoadLevel(0);
	}
}
