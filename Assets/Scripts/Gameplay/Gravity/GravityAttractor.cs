﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAttractor : MonoBehaviour
{
    public enum GravityType
    {
        PLANET,
        OBJECT
    }

    public GravityType m_type;
    protected Rigidbody m_rigidBody;
    public GameObject m_attractor;

    public virtual void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();

        if (!m_attractor)
            m_attractor = transform.parent.gameObject;
    }

    public virtual void GetDistanceAndGravityVector(Vector3 position, ref Vector3 gravity, ref float distance)
    {
    }

    public virtual float GetDistance(Vector3 position)
    {
        return 0;
    }

    public virtual Vector3 GetGravity(Vector3 position)
    {
        return Vector3.zero;
    }

    public virtual Vector3 GetGravityWithDistance(Vector3 position)
    {
        return Vector3.zero;
    }

    void OnTriggerEnter(Collider other)
    {
        GameObjectGravity gravity = other.GetComponent<GameObjectGravity>();
        if (gravity)
        {
            if (m_type == GravityType.PLANET)
            {
                if (!gravity.m_planetsGravity.Contains(this))
                    gravity.m_planetsGravity.Add(this);
            }
            else
            {
                if (!gravity.m_objectsGravity.Contains(this))
                    gravity.m_objectsGravity.Add(this);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        GameObjectGravity gravity = other.GetComponent<GameObjectGravity>();
        if (gravity)
        {
            if (gravity.m_planetsGravity.Contains(this))
                gravity.m_planetsGravity.Remove(this);
            else if (gravity.m_objectsGravity.Contains(this))
                gravity.m_objectsGravity.Remove(this);
        }
    }
}
