using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : BossStates {

	AnimatorStateInfo info;
	public float currentTime;
	public int frames = 0;

	public override void Start ()
	{
		base.Start ();
		m_type = States.ATTACK;
	}

	// Update is called once per frame
	public override bool OnUpdate () 
	{
		bool ret = false;

		info = m_boss.m_animator.GetCurrentAnimatorStateInfo (0);
		if (m_boss.m_phase == 1 && info.IsName("Attack3"))
			ThrowBall ();

		if (m_boss.m_phase == 2)
			Move ();
		
		if(info.normalizedTime > 0.9f)
		{
			ret = true;
			m_boss.m_currentState = m_boss.m_Idle;
		}

		return ret;
	}

	void Move()
	{
		//m_rigidBody.isKinematic = true;
		Quaternion rotation = Quaternion.LookRotation(m_boss.player.transform.position - transform.position, Vector3.up);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * m_boss.m_rotationSpeed * 16);

		Vector3 heading = m_boss.player.transform.position - transform.position;
		heading.y = 0;
		float distance = heading.magnitude;
		Vector3 direction = heading / distance;

		transform.position += direction * Time.deltaTime * m_boss.m_speed;
	}

	void ThrowBall()
	{
		//AnimatorClipInfo[] clipInfo = m_boss.m_animator.GetCurrentAnimatorClipInfo(0);
		//currentTime = clipInfo [0].clip.length * info.normalizedTime;
		//currentTime = info.length * info.normalizedTime;
	
		if (frames == 23) 
		{
			GameObject go = (GameObject)Instantiate (m_boss.ball, m_boss.OBJETO_TIRAR.transform.position, Quaternion.identity);
			go.transform.parent = m_boss.OBJETO_TIRAR.transform;
		}
		else if (frames == 50) 
		{
			GameObject go = m_boss.OBJETO_TIRAR.transform.GetChild (0).gameObject;
			go.transform.parent = null;
			Rigidbody rb = go.GetComponent<Rigidbody> ();
			rb.useGravity = true;
			rb.AddForce ((transform.up + 2*transform.forward + transform.right) * m_boss.m_force);
		}

		frames++;
	}

	public override void OnEnter()
	{
		m_boss.m_animator.SetBool ("IsAttacking", true);
	}

	public override void OnExit()
	{
		m_boss.m_animator.SetBool ("IsAttacking", false);
		frames = 0;
	}
}
