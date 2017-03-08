using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(GravityOnGameObject))]
public class GravityChange : MonoBehaviour {

    public float m_MaxDistanceChange = 50.0f;
    public float m_ChangeGravityRotationSpeed = 5.0f;
    public bool m_ZeroSpeedOnChange = true;
    [SerializeField] float m_ObjectDetectionRadius = 1.0f;
    [SerializeField] bool m_LandingBeforeChange = true;

    Player m_Player;
    GravityOnGameObject m_GravityOnPlayer;

    void Start()
    {
        m_Player = GetComponent<Player>();
        m_GravityOnPlayer = GetComponent<GravityOnGameObject>();
    }

    public void DrawRay(Vector3 origin_ray)
    {
        Transform OriginTransform;
        OriginTransform = Camera.main.transform;

        Debug.DrawRay(origin_ray, OriginTransform.forward * m_MaxDistanceChange, Color.red);
    }

    public bool ChangePlayerGravity(Vector3 origin_ray)
    {
        Transform OriginTransform;
        OriginTransform = Camera.main.transform;

        RaycastHit target_wall;
        bool hit = false;
        hit = Physics.Raycast(origin_ray, OriginTransform.forward, out target_wall, m_MaxDistanceChange);

        if (hit)
        {
            if (target_wall.collider.tag == "GravityWall")
            {
                m_GravityOnPlayer.m_Attractor = target_wall;
                m_GravityOnPlayer.m_Gravity = (m_Player.transform.position - target_wall.point).normalized;
            }
        }

        return hit;
    }

    public bool ChangeObjectsGravity(Vector3 origin_ray)
    {
        Transform OriginTransform;
        if (Camera.main != null)
            OriginTransform = Camera.main.transform;
        else
            OriginTransform = transform;

        RaycastHit target_wall;
        bool hit = false;
        hit = Physics.Raycast(origin_ray, OriginTransform.forward, out target_wall, m_MaxDistanceChange + 1000);

        if (hit)
        {
            /*if (target_wall.collider.tag == "GravityWall")
            {*/
                Collider[] allobjects = Physics.OverlapSphere(m_Player.transform.position, m_ObjectDetectionRadius);
                for (int i = 0; i < allobjects.Length; i++)
                {
                    if (allobjects[i].transform.tag == "GravityAffected")
                    {
                        GravityOnGameObject gravity_object = allobjects[i].transform.GetComponent<GravityOnGameObject>();
                        gravity_object.m_Attractor = target_wall;
                        gravity_object.m_Gravity = (allobjects[i].transform.position - target_wall.point).normalized;
                    }
                }
            //}
        }

        return hit;
    }

    public void GravityOnFeet(RaycastHit hit)
    {
        if (hit.collider.tag == "GravityWall")
        {
            m_GravityOnPlayer.m_Attractor = hit;
            m_GravityOnPlayer.m_Gravity = m_GravityOnPlayer.m_Attractor.normal;
        }
    }
}
