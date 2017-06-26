using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class controls the gravity changes caused by the player, either on himself or into the surroundings.
public class PlayerGravity : MonoBehaviour {

    Player m_player;
    GameObjectGravity m_playerGravity;
    GameObject m_objectDetected = null;

    Color m_preMaterialColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

	// Use this for initialization
	void Start ()
    {
        m_player = GetComponent<Player>();
        m_playerGravity = GetComponent<Player>().GetComponent<GameObjectGravity>();
	}

    //This functions returns true if the target aimed by the player is a legal GravityWall. Also controls the color of the sight
    public bool ViableGravityChange(out RaycastHit target_wall)
    {
        bool ret = false;
        UnlightObject();
        int layerMask = 1 << LayerMask.NameToLayer("Terrain");
        if (Physics.Raycast(m_player.m_camController.m_camRay, out target_wall, m_player.m_gravityRange, layerMask))
        {
            HUDManager.ChangeColorSight(target_wall.collider.tag == "GravityWall");
            if (target_wall.collider.tag == "GravityWall")
            {
                HighlightObject(target_wall.collider.gameObject);
                ret = true;
            }
        }
        else
        {
            HUDManager.ChangeColorSight(false);
        }

        return ret;
    }

    //This functions is called when a gravity change is performed by aiming at another legal surface
    public void ChangeGravityTo(RaycastHit attractor)
    {
        //m_playerGravity.ChangeGravityToPoint(attractor, m_player.transform.position);
    }

    //This function is called to see if the player is targeting an object or a direction in order to throw.
    //Also controls sight color.
    public bool ViableTargetForThrowing(out RaycastHit target_wall, int layer)
    {
        bool ret = false;
        int layerMask = 1 << layer;
        layerMask = ~layerMask;
        if (Physics.Raycast(m_player.m_camController.m_camRay, out target_wall, m_player.m_throwDetectionRange, layerMask))
            ret = true;

        HUDManager.ChangeColorSight(ret);

        return ret;
    }

    //This function is called in order to set the color of the aimed objects
    private void HighlightObject(GameObject newObjectDetected)
    {
        if(m_objectDetected != null && m_objectDetected != newObjectDetected)
        {
            MeshRenderer mesh = m_objectDetected.GetComponent<MeshRenderer>();
            if(mesh != null)
            {
                mesh.material.color = m_preMaterialColor;
            }
        }
        m_objectDetected = newObjectDetected;
        if(m_objectDetected != null)
        {
            MeshRenderer mesh = m_objectDetected.GetComponent<MeshRenderer>();
            if (mesh != null)
            {
                m_preMaterialColor = mesh.material.color;
                mesh.material.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
            }
        }
    }

    //This function is called to unset any highlighted objects
    public void UnlightObject()
    {
        HighlightObject(null);
    }
}
