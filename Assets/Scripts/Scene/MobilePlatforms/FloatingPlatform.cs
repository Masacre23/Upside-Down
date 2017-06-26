using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour {

    //Define Enum
    public enum DirectionUp { X, Y, Z, NOT_X, NOT_Y, NOT_Z };

    //This is what you need to show in the inspector.
    public DirectionUp m_up = DirectionUp.Z;
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
            switch (m_up)
            {
                case DirectionUp.X:
                    transform.Rotate(new Vector3(0.0f, -m_distanceCenter.z * m_speed * Time.deltaTime, m_distanceCenter.y * m_speed * Time.deltaTime));
                    break;
                case DirectionUp.Y:
                    transform.Rotate(new Vector3(m_distanceCenter.z * m_speed * Time.deltaTime, 0.0f, -m_distanceCenter.x * m_speed * Time.deltaTime));
                    break;
                case DirectionUp.Z:
                    transform.Rotate(new Vector3(-m_distanceCenter.y * m_speed * Time.deltaTime, m_distanceCenter.x * m_speed * Time.deltaTime, 0.0f));
                    break;
                case DirectionUp.NOT_X:
                    transform.Rotate(new Vector3(0.0f, m_distanceCenter.z * m_speed * Time.deltaTime, -m_distanceCenter.y * m_speed * Time.deltaTime));
                    break;
                case DirectionUp.NOT_Y:
                    transform.Rotate(new Vector3(-m_distanceCenter.z * m_speed * Time.deltaTime,  0.0f, m_distanceCenter.x * m_speed * Time.deltaTime));
                    break;
                case DirectionUp.NOT_Z:
                    transform.Rotate(new Vector3(m_distanceCenter.y * m_speed * Time.deltaTime, -m_distanceCenter.x * m_speed * Time.deltaTime, 0.0f));
                    break;
            }
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
        }
    }
}
