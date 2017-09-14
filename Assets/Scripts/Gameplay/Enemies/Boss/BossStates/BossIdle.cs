using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdle : BossStates {
	float counter;

	public override void Start ()
	{
		base.Start ();
		m_type = States.IDLE;
	}
	
	// Update is called once per frame
	public override bool OnUpdate () 
	{
		bool ret = false;

		Rotate ();

		counter += Time.deltaTime;
		if (counter >= m_boss.m_attackRate) 
		{
			ret = true;
			counter = 0;
			m_boss.m_currentState = m_boss.m_Attack;
		}

		return ret;
	}

	void Rotate()
	{
		Vector3 target = m_boss.player.transform.position;

		Vector3 difference = target - transform.position;

		float distanceToPlane = Vector3.Dot(transform.up, difference);
		Vector3 pointOnPlane = target - (transform.up * distanceToPlane);

		Quaternion origRot = transform.rotation;
		transform.LookAt(pointOnPlane, transform.up);
		Quaternion actualRot = transform.rotation;

		transform.rotation = Quaternion.Slerp (origRot, actualRot, Time.deltaTime * m_boss.m_rotationSpeed);
	}

	public override void OnEnter()
	{
	}

	public override void OnExit()
	{
	}


}
