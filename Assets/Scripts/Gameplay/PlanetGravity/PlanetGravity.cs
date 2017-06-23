using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    protected Rigidbody m_rigidBody;

    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        GameObjectGravity gravity = other.GetComponent<GameObjectGravity>();
        //if (gravity)
        //{
        //    if (!gravity.m_planets.Contains(m_rigidBody))
        //       gravity.m_planets.Add(m_rigidBody);
        //}
        if (gravity)
        {
            if (!gravity.m_planets.Contains(this))
                gravity.m_planets.Add(this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        GameObjectGravity gravity = other.GetComponent<GameObjectGravity>();
        //if (gravity)
        //{
        //    if (gravity.m_planets.Contains(m_rigidBody))
        //    {
        //        gravity.m_planets.Remove(m_rigidBody);
        //        float magnitude = (other.transform.position - m_rigidBody.transform.position).magnitude;
        //    }
        //}
        if (gravity)
        {
            if (gravity.m_planets.Contains(this))
            {
                gravity.m_planets.Remove(this);
                //float magnitude = (other.transform.position - m_rigidBody.transform.position).magnitude;
            }
        }
    }

    public virtual void GetDistanceAndGravityVector(Vector3 position, ref Vector3 gravity, ref float distance)
    {

    }

    public static void AlignWithFather(GameObject go)
    {
        Transform parentTransform = go.transform.parent;
        //if (parentTransform.tag == "Planet")
        //{
            Vector3 localPosition = go.transform.localPosition;

            Vector3 radialPosition = go.transform.position - parentTransform.position;
            Quaternion targetRotation = Quaternion.FromToRotation(go.transform.forward, radialPosition.normalized);
            go.transform.rotation = targetRotation * go.transform.rotation;

            go.transform.localPosition = localPosition;
        //}
    }

}
