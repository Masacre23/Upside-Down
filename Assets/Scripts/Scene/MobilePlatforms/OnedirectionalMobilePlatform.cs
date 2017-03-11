using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnedirectionalMobilePlatform : MonoBehaviour {

    public float mSpeed = 0.2f;
    public float mDistance = 2.0f;
    public float mWaitTime = 0.5f;
    public Vector3 mDirection = new Vector3(1.0f, 0.0f, 0.0f);
    public bool mBoomerang = true;

    private float mDistanceTraveled = 0.0f;
    private float mTimeWaited = 0.0f;
    private int mSense = 1;
    private bool mWaiting = false;
	
    // Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (!mWaiting)
        {
            float distanceToMove = mDistance - mDistanceTraveled;
            if (mSpeed * Time.deltaTime <= distanceToMove)
                distanceToMove = mSpeed * Time.deltaTime;
            mDistanceTraveled += distanceToMove;
            transform.Translate(mSense * mDirection * distanceToMove);
            if (mDistanceTraveled >= mDistance && mBoomerang)
            {
                mDistanceTraveled = 0;
                mSense = mSense == 1 ? -1 : 1;
                mWaiting = true;
            }
        }
        else
        {
            mTimeWaited += Time.deltaTime;
            if (mTimeWaited >= mWaitTime)
            {
                mWaiting = false;
                mTimeWaited = 0.0f;
            }
        }
        
	}
}
