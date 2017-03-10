using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class controls the gravity changes caused by the player, either on himself or into the surroundings.
[RequireComponent(typeof(NewPlayer))]
public class PlayerGravity : MonoBehaviour {

    [SerializeField] float m_gravityRange = 10.0f;
    [SerializeField] float m_objectDetectionRadius = 0.5f;

    NewPlayer m_player;
    GameObjectGravity m_playerGravity;
    LineRenderer m_rayLine;

	// Use this for initialization
	void Start ()
    {
        m_player = GetComponent<NewPlayer>();
        m_playerGravity = GetComponent<NewPlayer>().GetComponent<GameObjectGravity>();

        m_rayLine = gameObject.AddComponent<LineRenderer>();
        m_rayLine.startWidth = 0.05f;
        m_rayLine.endWidth = 0.05f;
        m_rayLine.numPositions = 2;
        m_rayLine.material = new Material(Shader.Find("Mobile/Particles/Additive"));
	}

    private void OnDestroy()
    {
        Destroy(m_rayLine.material);
    }

    public void DrawRay()
    {
        m_rayLine.SetPosition(0, m_player.transform.position + transform.up * m_player.m_capsuleHeight / 2);
        RaycastHit target_wall;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out target_wall, m_gravityRange))
        {
            m_rayLine.SetPosition(1, target_wall.point);
            if (target_wall.collider.tag == "GravityWall")
            {
                m_rayLine.startColor = Color.green;
                m_rayLine.endColor = Color.green;
            }
            else
            {
                m_rayLine.startColor = Color.red;
                m_rayLine.endColor = Color.red;
            }
            m_rayLine.enabled = true;
        }
        else
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out target_wall))
            {
                m_rayLine.SetPosition(1, target_wall.point);
                m_rayLine.startColor = Color.red;
                m_rayLine.endColor = Color.red;
                m_rayLine.enabled = true;
            }
            else
            {
                m_rayLine.enabled = false;
            }
        }
    }

    //This function sees if the player is pointing to an appropiate attractor (e.g GravityWall tag) in order to change the character gravitation.
    public bool ChangePlayerGravity()
    {
        RaycastHit target_wall;
        bool ret = false;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out target_wall, m_gravityRange))
        {
            if (target_wall.collider.tag == "GravityWall")
            {
                m_playerGravity.m_attractor = target_wall;
                m_playerGravity.m_gravity = (m_player.transform.position - target_wall.point).normalized;
                ret = true;
            }
        }
        return ret;
    }

    //This function will find an appropiate attractor for the objects near the player.
    //Its not limited to GravityWalls as for characters, so the player will be able to throw objects in any direction
    public void ChangeObjectGravity()
    {
        RaycastHit target_wall;
        bool hit = false;
        hit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out target_wall);

        if (hit)
        {
            Collider[] allobjects = Physics.OverlapSphere(m_player.transform.position + m_player.transform.up * (m_player.m_capsuleHeight / 2), m_objectDetectionRadius);
            for (int i = 0; i < allobjects.Length; i++)
            {
                if (allobjects[i].transform.tag == "GravityAffected")
                {
                    GameObjectGravity gravity_object = allobjects[i].transform.GetComponent<GameObjectGravity>();
                    gravity_object.m_attractor = target_wall;
                    gravity_object.m_gravity = (allobjects[i].transform.position - target_wall.point).normalized;
                }
            }
        }
    }
}
