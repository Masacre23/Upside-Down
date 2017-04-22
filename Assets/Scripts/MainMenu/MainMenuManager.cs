using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
	public Canvas m_mainMenuCanvas;
	public Canvas m_helpCanvas;
	public Canvas m_creditsCanvas;

	public void PressPlayButton(){
        Scenes.LoadScene(Scenes.Level1);
	}

	public void PreesQuitButton(){
		Debug.Log("Quit");
		Application.Quit();
	}

}
