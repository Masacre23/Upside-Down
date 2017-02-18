using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityOnGameObject : MonoBehaviour {
    public float m_AttractorDetection = 1.0f;

    Rigidbody m_RigidBody;
    public Vector3 m_Gravity;          //The direction of the gravity. Not taken into account the negative value of it;
    public RaycastHit m_Attractor;

    static float GravityStrength = -9.8f;

    void Start () {
        m_RigidBody = GetComponent<Rigidbody>();
        m_Gravity = Vector3.up;

        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit[] allhits = Physics.RaycastAll(ray, 100.0f);

        for (int i = 0; i < allhits.Length; i++)
        {
            if (allhits[i].transform.tag == "GravityWall")
                m_Attractor = allhits[i];
        }
    }

    void FixedUpdate()
    {
        m_RigidBody.AddForce(GravityStrength * m_RigidBody.mass * m_Gravity);
    }

    private void OnCollisionStay(Collision col)
    {
        if (col.collider.name == m_Attractor.collider.name)
        {
            m_Gravity = m_Attractor.normal;
        }
    }

}
