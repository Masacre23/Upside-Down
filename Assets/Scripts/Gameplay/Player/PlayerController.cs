using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

//This class should yield with the player input. It returns the conclusions of the player input (jump, movement direction, etc), not the keys.
//Input for debug mode should be dealt in DebugMode class. Input for menus should be dealt in LevelManager class.
public class PlayerController : MonoBehaviour
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Method to be called in order to deal with input from player
	public void GetDirections (ref float axisHorizontal, ref float axisVertical, ref float camHorizontal, ref float camVertical)
    {
		axisHorizontal = Input.GetAxis("Horizontal");
        axisVertical = Input.GetAxis("Vertical");

        camHorizontal = CrossPlatformInputManager.GetAxis("Mouse X");
        camVertical = CrossPlatformInputManager.GetAxis("Mouse Y");
    }

    public void GetButtons(ref bool jump, ref bool changeGravity, ref bool throwObjects, ref bool returnCam)
    {
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
            jump = true;
        if (CrossPlatformInputManager.GetButton("Fire2"))
            changeGravity = true;
        if (CrossPlatformInputManager.GetButton("Fire1"))
            throwObjects = true;
        if (CrossPlatformInputManager.GetButtonDown("ReturnCam"))
            returnCam = true;
    }
}
