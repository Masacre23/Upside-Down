using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageStates : MonoBehaviour {

    public enum States
    {
        RECIVE,
        ANIMATION,
        RESPAWN,
        NOT_RECIVE, 
        DEAD
    }

    public States m_type;
    protected Character m_charapter;

    // Use this for initialization
    public virtual void Start()
    {
        m_charapter = GetComponent<Character>();
    }

    //Main player update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public virtual bool OnUpdate(DamageData data)
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
