using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChanging : EnemyStates {
	RaycastHit hit;
	bool hitted;
	Vector3 origPos;
	Quaternion origRot;
	Quaternion finalRot;
	float time;

	// Use this for initialization
	public override void Start ()
    {
		base.Start ();
        m_type = States.CHANGING;
    }
	
	// Update is called once per frame
	public override bool OnUpdate (DamageData data)
    {
		bool ret = false;
		//Debug.DrawRay(transform.position, 2 * (transform.forward + transform.up), Color.green);
		time += 0.01f;
		transform.position = Vector3.Lerp(origPos, hit.point, time);
		transform.rotation = Quaternion.Lerp(origRot, finalRot, time);
		if (transform.rotation == finalRot) 
		{
			ret = true;
			OnExit();
		}

		return ret;
	}

	public override void OnEnter()
	{
		m_type = States.CHANGING;
		m_rigidBody.isKinematic = true;

		origPos = transform.position;
		origRot = transform.rotation;

		if (Physics.Raycast (transform.position, 2 * (transform.forward + transform.up), out hit)) 
		{
			if (hit.collider.tag == "GravityWall") 
			{
				m_enemy.m_gravityOnCharacter.m_attractor = hit;
				m_enemy.m_gravityOnCharacter.m_gravity = hit.transform.up;
				//m_enemy.currentWall = hit.collider.gameObject;
			}
		}
		finalRot = Quaternion.FromToRotation (transform.up, m_enemy.m_gravityOnCharacter.m_gravity) * transform.rotation;
	}

	public override void OnExit()
	{
		m_rigidBody.isKinematic = false;
		m_enemy.m_currentState = m_enemy.m_Following;
		time = 0;
	}
}
