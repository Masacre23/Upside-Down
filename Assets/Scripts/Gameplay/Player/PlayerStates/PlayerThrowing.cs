using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PlayerThrowing : PlayerStates
{
    float m_timeThrowing;
    List<GameObjectGravity> m_objects;
    List<Vector3> m_objectsInitialPositions;
    float m_strengthWithoutTarget;

    public override void Start()
    {
        base.Start();
        m_timeThrowing = 0.0f;
        m_objects = new List<GameObjectGravity>();
        m_objectsInitialPositions = new List<Vector3>();
        m_type = States.THROWING;

        m_strengthWithoutTarget = m_player.m_throwDetectionRange;
    }

    //Main player update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public override bool OnUpdate(float axisHorizontal, float axisVertical, bool jumping, bool changeGravity, bool aimingObject, bool throwing, float timeStep)
    {
        bool ret = false;
        HUDManager.ChangeFloatTime(1 - (m_timeThrowing / m_player.m_maxTimeThrowing));

        float perc = m_timeThrowing / m_player.m_objectsRisingTime;
        if (perc > 1.0f)
            perc = 1.0f;
        for (int i = 0; i < m_objects.Count; i++)
        {
            //m_objects[i].FloatVelocity(m_player.m_camController.m_cam.position + 2 * m_player.m_camController.m_camRay.direction, 0.5f);
            //m_objects[i].Float(m_objectsInitialPositions[i], m_player.m_camController.m_cam.position + 2 * m_player.m_camController.m_camRay.direction, perc);
            m_objects[i].Float(m_objectsInitialPositions[i], m_objectsInitialPositions[i] + m_player.transform.up * m_player.m_objectsFloatingHeight, perc);
        }

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
            if (!aimingObject)
            {
                m_player.m_currentState = m_player.m_grounded;
                ret = true;
            }
            else if (throwing)
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
                        Vector3 vectorToTarget = target.point - gravityObject.transform.position;
                        Vector3 throwVector = vectorToTarget.normalized * throwPower;
                        gravityObject.ThrowObject(throwVector, vectorToTarget.magnitude);
                    }
                }
                else
                {
                    Vector3 throwVector = m_player.m_camController.m_camRay.direction * throwPower;
                    foreach (GameObjectGravity gravityObject in m_objects)
                    {
                        gravityObject.ThrowObject(throwVector, m_player.m_throwDetectionRange);
                    }
                }
                m_objects.Clear();
                m_objectsInitialPositions.Clear();
            }
        }

        return ret;
    }

    public override void OnEnter()
    {
        int numObjects = m_player.m_maxNumberObjects;
        if (numObjects > 0)
            numObjects = LoadObjects(1 << LayerMask.NameToLayer("ThrowableObject"), numObjects);
        if (numObjects > 0)
            numObjects = LoadObjects(1 << LayerMask.NameToLayer("Enemy"), numObjects);

        m_player.m_camController.SetCameraTransition(CameraStates.States.AIMING);
        m_player.m_camController.SetAimLockOnTarget(true, "Enemy");
        m_player.m_throwButtonReleased = false;
        HUDManager.ShowGravityPanel(true);
    }

    public override void OnExit()
    {
        foreach (GameObjectGravity gravityObject in m_objects)
        {
            Rigidbody rigidBody = gravityObject.GetComponent<Rigidbody>();
            if (rigidBody)
                rigidBody.isKinematic = false;
        }

        m_objects.Clear();
        m_objectsInitialPositions.Clear();
        m_player.m_camController.SetCameraTransition(CameraStates.States.BACK);
        m_player.m_camController.UnsetAimLockOnTarget();
        m_timeThrowing = 0.0f;
        HUDManager.ShowGravityPanel(false);
    }

    private int LoadObjects(int layerMask, int numObjects)
    {
        int ret = numObjects;
        Vector3 sphereOrigin = m_player.transform.position + m_player.transform.up * (m_player.m_capsuleHeight / 2);
        List<Collider> allobjects = new List<Collider>(Physics.OverlapSphere(sphereOrigin, m_player.m_objectDetectionRadius, layerMask));
        allobjects.Sort(delegate (Collider a, Collider b) { return Vector3.Distance(sphereOrigin, a.transform.position).CompareTo(Vector3.Distance(sphereOrigin, b.transform.position)); });

        for (int i = 0; i < allobjects.Count; i++)
        {
            GameObjectGravity gravity_object = allobjects[i].transform.GetComponent<GameObjectGravity>();
            if (gravity_object && gravity_object.m_canBeThrowed)
            {
                Rigidbody rigidBody = gravity_object.GetComponent<Rigidbody>(); 
                if (rigidBody)
                {
                    rigidBody.isKinematic = true;
                    m_objects.Add(gravity_object);
                    m_objectsInitialPositions.Add(gravity_object.transform.position);
                    if (--numObjects == 0)
                        return 0;
                }
            }
        }
        return ret;
    }

    
}
