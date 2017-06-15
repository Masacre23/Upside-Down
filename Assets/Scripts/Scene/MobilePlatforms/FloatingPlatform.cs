using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour {

    public float m_speed;

    bool m_hasPlayer = false;
    Quaternion m_originalRotation;
    Vector3 m_distanceCenter;

    void Start()
    {
        m_originalRotation = transform.rotation;
    }

    void Update()
    {
        if (m_hasPlayer)
        {
            transform.Rotate(new Vector3(-m_distanceCenter.y * m_speed * Time.deltaTime, m_distanceCenter.x * m_speed * Time.deltaTime, 0.0f));
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, m_originalRotation, Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            m_hasPlayer = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            m_hasPlayer = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            m_distanceCenter = transform.InverseTransformPoint(collision.contacts[0].point);
            Debug.Log(m_distanceCenter);
        }
    }
}
