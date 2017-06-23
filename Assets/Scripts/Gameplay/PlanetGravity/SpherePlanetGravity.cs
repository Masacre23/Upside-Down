using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpherePlanetGravity: MonoBehaviour
{
    private Rigidbody m_rigidBody;

    public void SetRigidBody(Rigidbody rigidBody)
    {
        m_rigidBody = rigidBody;
    }

    public void GetDistanceAndGravityVector(Vector3 position, ref Vector3 gravity, ref float strengh)
    {
        gravity = position - transform.position;
        strengh = -m_rigidBody.mass / (25.0f * gravity.magnitude); ;
        gravity.Normalize();
    }
}
