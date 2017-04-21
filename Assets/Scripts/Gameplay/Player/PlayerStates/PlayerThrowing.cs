using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PlayerThrowing : PlayerStates
{
    float m_timeThrowing;
    float m_objectDetectionRadius;
    List<GameObjectGravity> m_objects;
    List<Vector3> m_objectsInitialPositions;
    float m_strengthWithoutTarget;

    public override void Start()
    {
        base.Start();
        m_timeThrowing = 0.0f;
        GameObject gravSphere = m_player.m_gravitationSphere;
        SphereCollider sphereCollider = gravSphere.GetComponent<SphereCollider>();
        m_objectDetectionRadius = sphereCollider.radius * gravSphere.transform.localScale.x;
        m_objects = new List<GameObjectGravity>();
        m_objectsInitialPositions = new List<Vector3>();
        m_type = States.THROWING;

        m_strengthWithoutTarget = m_player.m_throwDetectionRange;
    }

    //Main player update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public override bool OnUpdate(float axisHorizontal, float axisVertical, bool jumping, bool changeGravity, bool throwing, float timeStep)
    {
        bool ret = false;
        HUDManager.ChangeFloatTime(1 - (m_timeThrowing / m_player.m_maxTimeThrowing));

        float perc = m_timeThrowing / m_player.m_maxTimeThrowing;
        for (int i = 0; i < m_objects.Count; i++)
            m_objects[i].Float(m_objectsInitialPositions[i], m_objectsInitialPositions[i] + m_player.transform.up * m_player.m_objectsFloatingHeight, perc);

        if (m_timeThrowing > m_player.m_maxTimeThrowing)
        {
            m_player.m_currentState = m_player.m_grounded;
            ret = true;
        }
        else
        {
            m_timeThrowing += timeStep;
            RaycastHit target;
            bool hasTarget = m_player.m_playerGravity.ViableTargetForThrowing(out target);
            if (!throwing)
            {
                float throwPower = 0.0f;
                if (m_player.m_incresePowerWithTime)
                    throwPower = m_player.m_throwStrengthPerSecond * m_timeThrowing;
                else
                    throwPower = m_player.m_throwStrengthOnce;

                if (hasTarget)
                {
                    foreach (GameObjectGravity gravityObject in m_objects)
                    {
                        Vector3 throwVector = (target.point - gravityObject.transform.position) * throwPower;
                        gravityObject.ThrowObject(throwVector);
                    }
                }
                else
                {
                    Vector3 throwVector = m_player.m_camController.m_camRay.direction * m_strengthWithoutTarget * throwPower;
                    foreach (GameObjectGravity gravityObject in m_objects)
                    {
                        gravityObject.ThrowObject(throwVector);
                    }
                }
                
                m_player.m_currentState = m_player.m_grounded;
                ret = true;
            }
        }

        return ret;
    }

    public override void OnEnter()
    {
        Collider[] allobjects = Physics.OverlapSphere(m_player.transform.position + m_player.transform.up * (m_player.m_capsuleHeight / 2), m_objectDetectionRadius);
        for (int i = 0; i < allobjects.Length; i++)
        {
            if (allobjects[i].transform.tag == "GravityAffected")
            {
                GameObjectGravity gravity_object = allobjects[i].transform.GetComponent<GameObjectGravity>();
                m_objects.Add(gravity_object);
                m_objectsInitialPositions.Add(gravity_object.transform.position);
            }
        }

        m_player.m_camController.SetCameraTransition(CameraStates.States.AIMING);
        m_player.m_gravitationSphere.SetActive(true);
        m_player.m_throwButtonReleased = false;
        HUDManager.ShowGravityPanel(true);
    }

    public override void OnExit()
    {
        m_objects.Clear();
        m_objectsInitialPositions.Clear();
        m_player.m_camController.SetCameraTransition(CameraStates.States.BACK);
        m_player.m_gravitationSphere.SetActive(false);
        m_timeThrowing = 0.0f;
        HUDManager.ShowGravityPanel(false);
    }
}
