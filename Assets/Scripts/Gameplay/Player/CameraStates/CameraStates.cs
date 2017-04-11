using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStates : MonoBehaviour {

    public enum States
    {
        BACK,
        AIMING,
        TRANSIT
    }

    public States m_type;
    protected VariableCam m_variableCam;

    // Use this for initialization
    public virtual void Start()
    {
        m_variableCam = GetComponent<VariableCam>();
    }

    //Main player update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public virtual bool OnUpdate(float axisHorizontal, float axisVertical, float timeStep)
    {
        return false;
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnExit()
    {
    }
}
