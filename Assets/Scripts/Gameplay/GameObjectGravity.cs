using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class should be added to any GameObject which gravity can be changed during the game.
//It controls the current gravity of the object, and adds it to its rigid body.
public class GameObjectGravity : MonoBehaviour {

    Rigidbody m_rigidBody;
    Vector3 m_oldGravity;
    public RaycastHit m_attractor;
    public Vector3 m_gravity;
    GameObject m_planetAttractor;
    public bool m_planetGravityActive = false;

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
            if (hit.transform.tag == "GravityWall" || hit.transform.tag == "Planet")
            {
                m_attractor = hit;
                m_gravity = m_attractor.normal;
                if (hit.transform.tag == "Planet")
                    m_planetGravityActive = true;
            }
        }
	}
	
	// Called to add gravity force into the rigid body.
	public void FixedUpdate ()
    {
        if(m_planetAttractor == null || !m_planetGravityActive)
            m_rigidBody.AddForce(m_gravityStrength * m_rigidBody.mass * m_gravity);
        else
        {
            Vector3 line = transform.position - m_planetAttractor.transform.position;
            line.Normalize();
            float distance = Vector3.Distance(transform.position, m_planetAttractor.gameObject.transform.position);
            Vector3 planet_force = line * (m_gravityStrength / distance) * m_planetAttractor.transform.GetComponent<Rigidbody>().mass;
            m_rigidBody.AddForce(planet_force);
         }
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
        if(col.collider.tag == "Planet")
            m_planetAttractor = col.gameObject;
    }

    //This function sets the passed raycasthit as the current attractor for the game object
    //It's mainly to be used by characters in order update its attractor while they walk
    public void GravityOnFeet(RaycastHit hit)
    {
        if (hit.collider.tag == "GravityWall" || hit.collider.tag == "Planet")
        {
            m_attractor = hit;
            m_gravity = hit.normal;
        }
    }

    //This function sets the GravityObject as a throwing object by the player. 
    public void SetAsThrowingObject(GameObject player)
    {
        if(player != null)
        {
            this.transform.parent = player.transform;
            this.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            this.transform.localPosition = new Vector3(0.0f, 1.0f, 1.0f);
            this.m_oldGravity = this.m_gravity;
            this.m_gravity = new Vector3(0.0f, 0.0f, 0.0f);
            HingeJoint hingeJoint = this.gameObject.AddComponent<HingeJoint>();
            hingeJoint.connectedBody = player.GetComponent<Rigidbody>();
        }
        else
        {
            HingeJoint hingeJoint = this.gameObject.GetComponent<HingeJoint>();
            Destroy(hingeJoint);
            this.transform.parent = null;
            this.m_gravity = this.m_oldGravity;
        }
       
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Planet")
        {
            m_planetAttractor = col.gameObject;
        }
    }
}
