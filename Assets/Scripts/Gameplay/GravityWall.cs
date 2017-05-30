using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWall : MonoBehaviour
{
    GameObject m_gravityWall;
    CapsuleCollider m_gravityZone;
    int layerMask; 

	// Use this for initialization
	void Start ()
    {
        m_gravityZone = GetComponent<CapsuleCollider>();

        m_gravityWall = gameObject.transform.parent.gameObject;

        layerMask = 1 << m_gravityWall.layer;
	}

    void OnTriggerEnter(Collider col)
    {
        GameObjectGravity objectGravity = col.GetComponent<GameObjectGravity>();
        if (objectGravity)
        {
            if (!objectGravity.m_planetGravity && !objectGravity.m_changingToAttractor)
            {
                RaycastHit target;
                Vector3 direction = transform.position - col.transform.position;
                if (Physics.Raycast(col.transform.position, direction.normalized, out target, direction.magnitude, layerMask))
                {
                    objectGravity.m_attractor = target;
                }
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        GameObjectGravity objectGravity = col.GetComponent<GameObjectGravity>();
        if (objectGravity)
        {
            if (!objectGravity.m_planetGravity && objectGravity.m_attractor.transform.gameObject == m_gravityWall)
            {
                objectGravity.m_planetGravity = true;
            }
        }
    }
}
