using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoor : MonoBehaviour {
	//GameObject[] buttons;
	public int numButtons;
	int numActivatedButtons;
	bool active = false;

	void Start () {
		//numButtons = buttons.Length;
	}

	void Update () {
		
	}

	public void Activate()
	{
		++numActivatedButtons;
		if (numActivatedButtons >= numButtons)
			DoAction ();
	}

	void DoAction()
	{
		this.gameObject.SetActive (false);
		GameObject.Find ("SceneManager").GetComponent <SceneManager> ().GameOver ();
	}
}
