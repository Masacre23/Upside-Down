using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class should be added to any GameObject which gravity can be changed during the game.
//It controls the current gravity of the object, and adds it to its rigid body.
public class GameObjectGravity : MonoBehaviour {

    Rigidbody m_rigidBody;
    public RaycastHit m_attractor;
    public Vector3 m_gravity;

    //This should be the same for all gameobjects
    static float m_gravityStrength = -9.8f;

	// Use this for initialization
	void Start ()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_rigidBody.useGravity = false;
        m_gravity = Physics.gravity;

        //Find an attractor if possible (if not, its possible that the GameObject is not correctly positioned or oriented.
        //If found, set gravity as the normal of the hit;
        //RaycastHit[] allhits = Physics.RaycastAll(m_rigidBody.transform.position, -Vector3.up, 100.0f);
        RaycastHit[] allhits = Physics.RaycastAll(m_rigidBody.transform.position, -transform.up, 100.0f);
        foreach (RaycastHit hit in allhits)
        {
            if (hit.transform.tag == "GravityWall")
            {
                m_attractor = hit;
                m_gravity = m_attractor.normal;
            }
        }
	}
	
	// Called to add gravity force into the rigid body.
	public void FixedUpdate ()
    {
        m_rigidBody.AddForce(m_gravityStrength * m_rigidBody.mass * m_gravity);
	}

    //This function is called automatically when this object colliders begins to touch another collider.
    //It's used so when the gameobject reaches it's attractor, it changes its current gravity for the attractor's normal.
    //It's main importance regards objects falling (characters have it's own way to detect floors).
    private void OnCollisionEnter(Collision col)
    {
        if (m_attractor.collider != null)
        {
            if (col.collider == m_attractor.collider)
            {
                m_gravity = m_attractor.normal;
            }
        }
    }

    //This function sets the passed raycasthit as the current attractor for the game object
    //It's mainly to be used by characters in order update its attractor while they walk
    public void GravityOnFeet(RaycastHit hit)
    {
        if (hit.collider.tag == "GravityWall")
        {
            m_attractor = hit;
            m_gravity = hit.normal;
        }
    }
}
