using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCameraOnCollision : MonoBehaviour
{
    public IsometricCamera m_isometric;
    public AsteroidCamera m_asteroids;

    public bool m_activateIsometric = true;

    void OnCollisionEnter(Collision col)
	{
		if (col.transform.tag == "Player") 
		{
            if (m_isometric && m_asteroids)
            {
                if (m_activateIsometric)
                {
                    m_isometric.enabled = true;
                    m_asteroids.enabled = false;
                }
                else
                {
                    m_isometric.enabled = false;
                    m_asteroids.enabled = true;
                }
            }
		}
	}
}
