using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePlanetGravity:PlanetGravity
{
    public float m_strengh = 50;
    public Vector3 m_up = Vector3.zero;

    public override void GetDistanceAndGravityVector(Vector3 position, ref Vector3 gravity, ref float strengh)
    {
        gravity = position - transform.parent.position;
        strengh = -m_strengh;
        gravity = m_up;
        if(gravity == Vector3.zero)
            gravity = transform.up;
        gravity.Normalize();
        Debug.Log(strengh);
    }
}