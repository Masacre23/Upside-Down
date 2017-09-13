﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterBoxAttractor : GravityAttractor
{
    BoxCollider m_collider;

    public override void Start()
    {
        m_collider = GetComponent<BoxCollider>();
        if (!m_collider)
            m_collider = gameObject.AddComponent<BoxCollider>();

        base.Start();
    }

    public override void GetDistanceAndGravityVector(Vector3 position, ref Vector3 gravity, ref float distance)
    {
        gravity = position - transform.position;
        distance = gravity.magnitude;
        gravity.Normalize();
    }

    public override float GetDistance(Vector3 position)
    {
        return (position - transform.position).magnitude;
    }

    public override Vector3 GetGravity(Vector3 position)
    {
        return (position - transform.position).normalized;
    }

    public override Vector3 GetGravityWithDistance(Vector3 position)
    {
        return position - transform.position;
    }
}
