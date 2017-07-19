using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PlayerThrowing : PlayerStates
{
    public override void Start()
    {
        base.Start();
        m_type = States.THROWING;

    }

    //Main player update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public override bool OnUpdate(float axisHorizontal, float axisVertical, bool jumping, bool pickObjects, bool aimingObject, float timeStep)
    {
        bool ret = false;

        if (aimingObject)
        {
            m_player.m_currentState = m_player.m_grounded;
            ret = true;
        }
        else if (!m_player.m_floatingObjects.HasObjectsToThrow())
        {
            m_player.m_currentState = m_player.m_grounded;
            ret = true;
        }
        else
        {
            RaycastHit targetHit;
            bool hasTarget = false;
            hasTarget = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out targetHit, m_player.m_throwDetectionRange);
            if (hasTarget)
            {
                if (targetHit.collider.tag.Contains("Enemy"))
                    HUDManager.ChangeColorSight(true);
                else
                    HUDManager.ChangeColorSight(false);
            }
            else
                HUDManager.ChangeColorSight(false);

            if (pickObjects)
            {
                if (hasTarget)
                    m_player.m_floatingObjects.ThrowObjectToTarget(targetHit, Camera.main.transform, m_player.m_throwForce);
                else
                    m_player.m_floatingObjects.ThrowObjectToDirection(Camera.main.transform, m_player.m_throwDetectionRange, m_player.m_throwForce);
            }


        }

        return ret;
    }

    public override void OnEnter()
    {
        m_player.m_markAimedObject = false;
        m_rigidBody.isKinematic = true;
        m_player.m_camController.SetCameraTransition(CameraStates.States.AIMING, true);
        m_player.m_camController.SetAimLockOnTarget(true, "Enemy");
        HUDManager.ShowGravityPanel(true);
    }

    public override void OnExit()
    {
        m_rigidBody.isKinematic = false;
        m_player.m_camController.SetCameraTransition(CameraStates.States.BACK);
        m_player.m_camController.UnsetAimLockOnTarget();
        HUDManager.ShowGravityPanel(false);
        HUDManager.ChangeColorSight(false);
    }

}
