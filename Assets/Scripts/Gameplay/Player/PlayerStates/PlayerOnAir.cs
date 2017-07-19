using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnAir : PlayerStates
{
    public float m_oxigenDoubleJump = 20.0f;
    private bool m_doubleJump = false;

    public override void Start()
    {
        base.Start();
        m_type = States.ONAIR;
    }

    //Main player update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public override bool OnUpdate(float axisHorizontal, float axisVertical, bool jumping, bool pickObjects, bool aimingObject, float timeStep)
    {
        bool ret = false;

        if (axisHorizontal == 0.0f && axisVertical == 0.0f)
            m_player.m_playerStopped = true;
        else
            m_player.m_playerStopped = false;

        m_player.OnAir();
        m_player.UpdateUp();
        m_player.MoveOnAir(timeStep);
        if (m_player.CheckGroundStatus())
        {
            m_player.m_currentState = m_player.m_grounded;
            ret = true;
        }

        return ret;
    }

    public override void OnEnter()
    {
        m_player.m_markAimedObject = true;
        m_doubleJump = false;   
    }

    public override void OnExit()
    {
    }
}
