using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    Rigidbody m_rigidBody;

    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        GameObjectGravity gravity = other.GetComponent<GameObjectGravity>();
        if (gravity)
        {
            gravity.m_planets.Add(m_rigidBody);
        }
    }

    void OnTriggerExit(Collider other)
    {
        GameObjectGravity gravity = other.GetComponent<GameObjectGravity>();
        if (gravity)
        {
            if (gravity.m_planets.Contains(m_rigidBody))
            {
                gravity.m_planets.Remove(m_rigidBody);
            }
        }
    }

    public static void AlignWithFather(GameObject go)
    {
        Transform parentTransform = go.transform.parent;
        if (parentTransform.tag == "Planet")
        {
            Vector3 localPosition = go.transform.localPosition;

            Vector3 radialPosition = go.transform.position - parentTransform.position;
            Quaternion targetRotation = Quaternion.FromToRotation(go.transform.forward, radialPosition.normalized);
            go.transform.rotation = targetRotation * go.transform.rotation;

            go.transform.localPosition = localPosition;
        }
    }

}
