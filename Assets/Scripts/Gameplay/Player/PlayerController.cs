using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class should yield with the player input. It returns the conclusions of the player input (jump, movement direction, etc), not the keys.
//Input for debug mode should be dealt in DebugMode class. Input for menus should be dealt in LevelManager class.
public class PlayerController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Method to be called in order to deal with input from player
	public void GetDirections (ref float axisHorizontal, ref float axisVertical)
    {
		axisHorizontal = Input.GetAxis("Horizontal");
        axisVertical = Input.GetAxis("Vertical");
    }

    public void GetButtons(ref bool jump, ref bool changeGravity, ref bool throwObjects)
    {
        if (Input.GetButtonDown("Jump"))
            jump = true;
        if (Input.GetButton("Fire2"))
            changeGravity = true;
        if (Input.GetButton("Fire1"))
            throwObjects = true;
    }
}
