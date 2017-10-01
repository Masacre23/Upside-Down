﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickedObject : MonoBehaviour
{
    public Transform m_pickedObjectPosition;
    private ThrowableObject m_pickedObject = null;

    public LayerMask m_layersPicking;
    private Player m_player;

    public float m_objectDetectionRadius = 1.5f;
    public float m_horizontalThrowForceDefault = 5.0f;
    public float m_timeToFall = 2.0f;

    private Vector3 m_throwVector = Vector3.forward;
    private float m_throwForce = 0.0f;
    private float m_throwHorizontalForce = 1.0f;

    // Use this for initialization
    void Start ()
    {
        m_player = GetComponent<Player>();
	}

    // This functions serves to pick the nearest objects (that are in m_layerPicking mask) to the player
    public bool PickObjects(Vector3 detectionOrigin)
    {
        bool pickedAny = false;

        List<Collider> detectedObjects = new List<Collider>(Physics.OverlapSphere(detectionOrigin, m_objectDetectionRadius, m_layersPicking));
        detectedObjects.Sort(delegate (Collider a, Collider b) { return Vector3.Distance(detectionOrigin, a.transform.position).CompareTo(Vector3.Distance(detectionOrigin, b.transform.position)); });

        for (int i = 0; i < detectedObjects.Count; i++)
        {
            ThrowableObject throwableObject = detectedObjects[i].transform.GetComponent<ThrowableObject>();
            if (throwableObject && throwableObject.m_canBePicked)
            {
                m_pickedObject = throwableObject;
                m_pickedObject.BeginCarried(m_pickedObjectPosition, this);
                pickedAny = true;
                break;
            }
        }

        return pickedAny;
    }

    //This function is called to drop the carried object
    public void Drop()
    {
        if (m_pickedObject)
            m_pickedObject.StopCarried();
    }

    //This function is called to throw an object to a target
    public void ThrowObjectToTarget(RaycastHit target, Transform playerOrigin, float throwForce)
    {
        Vector3 playerToTarget = target.point - playerOrigin.position;

        m_player.RotateModel(playerToTarget.normalized);

        m_throwVector = target.point - m_pickedObject.transform.position;
        m_throwForce = throwForce;
        m_throwHorizontalForce = m_horizontalThrowForceDefault;
    }

    //This function is called to throw an object in a direction
    public void ThrowObjectToDirection(Transform playerOrigin, float maxAimLength, float throwForce)
    {
        Vector3 playerToTarget = playerOrigin.forward * maxAimLength;
        Vector3 finalPosition = playerOrigin.position + playerToTarget;

        m_player.RotateModel(playerToTarget.normalized);

        m_throwVector = finalPosition - m_pickedObject.transform.position;
        m_throwForce = throwForce;
        m_throwHorizontalForce = m_horizontalThrowForceDefault;
    }

    public void ThrowObjectNow()
    {
        Throw();
    }

    //This function actually throws a specific object in a specific direction with an specific force
    private void Throw()
    {
        if(m_pickedObject != null)
            m_pickedObject.ThrowObject(m_throwForce, m_throwHorizontalForce, m_player.transform.up,m_throwVector.normalized);
    }

    //This function serves to free a space for picking objects, called when they are thrown or fall
    public void FreeSpace()
    {
        m_pickedObject = null;
    }

    public bool CanPickMoreObjects()
    {
        return m_pickedObject == null;
    }

    public bool HasObjectsToThrow()
    {
        return m_pickedObject;
    }

    public bool EnemyIsFloating(GameObject enemy)
    {
        bool ret = false;

        if (m_pickedObject != null && m_pickedObject.gameObject == enemy)
            ret = true;

        return ret;
    }
}
