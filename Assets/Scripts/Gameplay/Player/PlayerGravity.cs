using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class controls the gravity changes caused by the player, either on himself or into the surroundings.
public class PlayerGravity : MonoBehaviour {

    float m_objectDetectionRadius;
    Player m_player;
    GameObjectGravity m_playerGravity;
    LineRenderer m_rayLine;
    GameObject m_objectDetected = null;

	// Use this for initialization
	void Start ()
    {
        m_player = GetComponent<Player>();
        m_playerGravity = GetComponent<Player>().GetComponent<GameObjectGravity>();

        GameObject gravSphere = m_player.m_gravitationSphere;
        SphereCollider sphereCollider = gravSphere.GetComponent<SphereCollider>();
        m_objectDetectionRadius = sphereCollider.radius * gravSphere.transform.localScale.x;

        m_rayLine = gameObject.AddComponent<LineRenderer>();
        m_rayLine.startWidth = 0.05f;
        m_rayLine.endWidth = 0.05f;
        m_rayLine.numPositions = 2;
        m_rayLine.material = new Material(Shader.Find("Mobile/Particles/Additive"));
	}

    //We should implicitly destroy the material since it isn't destroyed by the garbage collection
    private void OnDestroy()
    {
        Destroy(m_rayLine.material);
    }

    public void DrawRay()
    {
        HighlightObject(null);
        m_rayLine.SetPosition(0, m_player.transform.position + transform.up * m_player.m_capsuleHeight);
        RaycastHit target_wall;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out target_wall, m_player.m_gravityRange))
        {
            m_rayLine.SetPosition(1, target_wall.point);
            HUDManager.ChangeColorSight(target_wall.collider.tag == "GravityWall");
            if (target_wall.collider.tag == "GravityWall")
            {
                if(m_player.m_currentState.m_type == PlayerStates.States.FLOATING)
                    HighlightObject(target_wall.collider.gameObject);
                m_rayLine.startColor = Color.green;
                m_rayLine.endColor = Color.green;
                m_rayLine.material.color = Color.green;
            }
            else
            {
                m_rayLine.startColor = Color.red;
                m_rayLine.endColor = Color.red;
                m_rayLine.material.color = Color.red;
            }
            m_rayLine.enabled = true;
        }
        else
        {
            HUDManager.ChangeColorSight(false);
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out target_wall))
            {
                m_rayLine.SetPosition(1, target_wall.point);
                m_rayLine.startColor = Color.red;
                m_rayLine.endColor = Color.red;
                m_rayLine.material.color = Color.red;
                m_rayLine.enabled = true;
            }
            else
            {
                m_rayLine.enabled = false;
            }
        }
    }

    private void HighlightObject(GameObject newObjectDetected)
    {
        if(m_objectDetected != null && m_objectDetected != newObjectDetected)
        {
            MeshRenderer mesh = m_objectDetected.GetComponent<MeshRenderer>();
            if(mesh != null)
            {
                mesh.material.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            }
        }
        m_objectDetected = newObjectDetected;
        if(m_objectDetected != null)
        {
            MeshRenderer mesh = m_objectDetected.GetComponent<MeshRenderer>();
            if (mesh != null)
            {
                mesh.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }
    }

    //This function sees if the player is pointing to an appropiate attractor (e.g GravityWall tag) in order to change the character gravitation.
    public bool ChangePlayerGravity()
    {
        RaycastHit target_wall;
        bool ret = false;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out target_wall, m_player.m_gravityRange))
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
        bool hit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out target_wall);
        if (target_wall.transform.tag == "GravityAffected")
        {
            RaycastHit[] alltargets = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, 1000.0f);
            foreach (RaycastHit rayhit in alltargets)
            {
                if (rayhit.transform.tag != "GravityAffected")
                {
                    target_wall = rayhit;
                    break;
                }
            }
        }

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
