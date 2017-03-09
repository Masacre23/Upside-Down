using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
	public Canvas m_mainMenuCanvas;
	public Canvas m_helpCanvas;
	public Canvas m_creditsCanvas;

	public void PressPlayButton(){
		Application.LoadLevel(0);
	}

	public void PressHelpButton(){
		m_mainMenuCanvas.gameObject.SetActive(false);
		m_helpCanvas.gameObject.SetActive(true);
	}

	public void PressCreditsButton(){
		m_mainMenuCanvas.gameObject.SetActive(false);
		m_creditsCanvas.gameObject.SetActive(true);
	}

	public void PreesQuitButton(){
		Debug.Log("Quit");
		Application.Quit();
	}

	public void BackMainMenu(){
		m_mainMenuCanvas.gameObject.SetActive(true);
		m_helpCanvas.gameObject.SetActive(false);
		m_creditsCanvas.gameObject.SetActive(false);
	}
}
