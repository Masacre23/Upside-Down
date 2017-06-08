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
    public override bool OnUpdate(float axisHorizontal, float axisVertical, bool jumping, bool pickObjects, bool aimGravity, bool changeGravity, bool aimingObject, bool throwing, float timeStep)
    {
        bool ret = false;

        if (axisHorizontal == 0.0f && axisVertical == 0.0f)
            m_player.m_playerStopped = true;
        else
            m_player.m_playerStopped = false;

        //RaycastHit target;
        //bool hasTarget = m_player.SetMarkedTarget(out target);

        if (jumping && !m_doubleJump && m_player.m_oxigen.HasEnoughOxygen(m_oxigenDoubleJump))
        {
            m_player.Jump(axisHorizontal, axisVertical);
            m_doubleJump = true;
            m_player.m_doubleJumping = true;
            m_player.m_oxigen.LostOxigen(m_oxigenDoubleJump);
        }
        //if (changeGravity)
        //{
        //    if (hasTarget)
        //    {
        //        m_player.m_playerGravity.ChangeGravityTo(target);
        //        m_player.m_gravityOnCharacter.ChangeToAttractor();
        //        if (m_player.m_soundEffects != null)
        //            m_player.m_soundEffects.PlaySound("NewGravity");
        //        m_player.m_rigidBody.velocity = Vector3.zero;
        //        m_player.m_freezeMovement = true;
        //        m_player.m_currentState = m_player.m_onAir;
        //        ret = true;
        //    }
        //    else
        //        m_player.SwapGravity();
        //}
        //else
        //{
            //if (changeGravity && !m_player.m_gravityOnCharacter.m_changingToAttractor)
            //    m_player.m_gravityOnCharacter.ReturnToPlanet();
            m_player.OnAir();
            m_player.UpdateUp();
            m_player.MoveOnAir(timeStep);
            if (m_player.CheckGroundStatus())
            {
                m_player.m_currentState = m_player.m_grounded;
                ret = true;
            }
        //}

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
