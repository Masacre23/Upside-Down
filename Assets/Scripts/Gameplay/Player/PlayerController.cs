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

    public void GetButtons(ref bool jump, ref bool aimGravity, ref bool changeGravity, ref bool aimObjects, ref bool throwObjects, ref bool returnCam)
    {
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
            jump = true;
        if (CrossPlatformInputManager.GetButtonDown("AimGravity"))
            aimGravity = true;
        if (CrossPlatformInputManager.GetButtonDown("AimObjects"))
            aimObjects = true;
        if (CrossPlatformInputManager.GetButtonDown("ReturnCam"))
            returnCam = true;

        float rightTrigger = CrossPlatformInputManager.GetAxis("ChangeGravity");
        changeGravity = rightTrigger > 0 ? true : false;
        if (!changeGravity && CrossPlatformInputManager.GetButton("ChangeGravity"))
            changeGravity = true;

        float leftTrigger = CrossPlatformInputManager.GetAxis("ThrowObjects");
        throwObjects = leftTrigger > 0 ? true : false;
        if (!throwObjects && CrossPlatformInputManager.GetButton("ThrowObjects"))
            throwObjects = true;
    }
}
