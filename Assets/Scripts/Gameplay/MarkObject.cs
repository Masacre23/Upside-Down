using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkObject : HighlighterController
{

    public Color flashingStartColor = Color.blue;
    public Color flashingEndColor = Color.cyan;
    public float flashingDelay = 2.5f;
    public float flashingFrequency = 2f;

    protected override void Start ()
    {
        base.Start();
    }
	
    public void BeginMarking()
    {
        h.FlashingOn(flashingStartColor, flashingEndColor, flashingDelay);
    }

    public void StopMarking()
    {
        h.FlashingOff();
    }

}
