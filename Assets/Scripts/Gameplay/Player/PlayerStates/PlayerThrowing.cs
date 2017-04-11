using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PlayerThrowing : PlayerStates
{
    float m_timeThrowing;
    float m_objectDetectionRadius;
    List<GameObjectGravity> m_objects;

    public override void Start()
    {
        base.Start();
        m_timeThrowing = 0.0f;
        GameObject gravSphere = m_player.m_gravitationSphere;
        SphereCollider sphereCollider = gravSphere.GetComponent<SphereCollider>();
        m_objectDetectionRadius = sphereCollider.radius * gravSphere.transform.localScale.x;
        m_objects = new List<GameObjectGravity>();
        m_type = States.THROWING;
    }

    //Main player update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public override bool OnUpdate(float axisHorizontal, float axisVertical, bool jumping, bool changeGravity, bool throwing, float timeStep)
    {
        bool ret = false;
        HUDManager.ChangeFloatTime(1 - (m_timeThrowing / m_player.m_maxTimeFloating));

        if( !throwing)
        {
            m_player.m_currentState = m_player.m_grounded;
            RaycastHit target_wall;
            bool hit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out target_wall);
            if (target_wall.transform.tag == "GravityAffected")
            {
                RaycastHit[] alltargets = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, 1000.0f);
                foreach (RaycastHit rayhit in alltargets)
                {
                    if (rayhit.transform.tag != "GravityAffected")
                    {
                        target_wall = rayhit;
                        break;
                    }
                }
            }

            foreach (GameObjectGravity gravity_object in m_objects)
            {
                gravity_object.SetAsThrowingObject(null);
                gravity_object.m_attractor = target_wall;
                gravity_object.m_gravity = (gravity_object.gameObject.transform.position - target_wall.point).normalized;
            }
            m_objects.Clear();
            ret = true;
        }
        else if (m_timeThrowing > m_player.m_maxTimeFloating)
        {
            m_player.m_currentState = m_player.m_grounded;
            foreach(GameObjectGravity gravityObject in m_objects)
            {
                gravityObject.SetAsThrowingObject(null);
            }
            m_objects.Clear();
            ret = true;
        }
        else
        {
            m_timeThrowing += timeStep;
            Collider[] allobjects = Physics.OverlapSphere(m_player.transform.position + m_player.transform.up * (m_player.m_capsuleHeight / 2), m_objectDetectionRadius);
            for (int i = 0; i < allobjects.Length; i++)
            {
                if (allobjects[i].transform.tag == "GravityAffected")
                {
                    GameObjectGravity gravity_object = allobjects[i].transform.GetComponent<GameObjectGravity>();
                    if (!m_objects.Contains(gravity_object))
                    {
                        m_objects.Add(gravity_object);
                        gravity_object.SetAsThrowingObject(this.gameObject);
                    }
                }
            }
            m_player.UpdateUp();
            m_player.Move(timeStep);
            if (!m_player.CheckGroundStatus())
            {
                m_player.m_currentState = m_player.m_onAir;
                ret = true;
            }
        }

        return ret;
    }

    public override void OnEnter()
    {
        m_rigidBody.isKinematic = true;
        m_player.m_gravitationSphere.SetActive(true);
        m_player.m_reachedGround = false;
        HUDManager.ShowGravityPanel(true);
    }

    public override void OnExit()
    {
        m_rigidBody.isKinematic = false;
        m_player.m_gravitationSphere.SetActive(false);
        m_timeThrowing = 0.0f;
        HUDManager.ShowGravityPanel(false);
    }
}
