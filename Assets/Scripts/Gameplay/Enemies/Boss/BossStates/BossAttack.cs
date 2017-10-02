using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : BossStates {

	AnimatorStateInfo info;
	public float currentTime;
	public int frames = 0;
	float counter = 4;
	Vector3 randomPos;

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
		if (m_boss.m_phase == 1 /*&& info.IsName("Attack3")*/)
			ThrowBall ();

		if (m_boss.m_phase == 2)
			Move ();

        if (m_boss.m_phase == 3)
        {
            m_boss.m_sound.PlayWalk();
            RunAway();
        }
		
		if(info.normalizedTime > 0.9f && (m_boss.m_phase <= 1 || !m_boss.m_canChase) && !info.IsName("Laught"))
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

        if (distance > m_boss.minDistanceToPlayer * transform.localScale.x)
        {
            transform.position += direction * Time.deltaTime * m_boss.m_speed;
            m_boss.m_sound.PlayWalk();
        }else
        {
            m_boss.m_sound.StopWalk();
        }
	}

	void ThrowBall()
	{
        if (frames == 23) 
		{
			GameObject go = (GameObject)Instantiate (m_boss.ball, m_boss.OBJETO_TIRAR.transform.position, Quaternion.identity);
			go.transform.parent = m_boss.OBJETO_TIRAR.transform;
           // go.transform.lossyScale.Set(4, 4, 4);
        }
		else if (frames == 50) 
		{
			GameObject go = m_boss.OBJETO_TIRAR.transform.GetChild (0).gameObject;
			go.GetComponent<SphereCollider> ().enabled = true;
			go.transform.parent = null;
            go.transform.localScale = new Vector3(4, 4, 4);
			Rigidbody rb = go.GetComponent<Rigidbody> ();
			rb.useGravity = true;
			rb.AddForce ((transform.up + 2*transform.forward + transform.right) * m_boss.m_force);
		}

		frames++;
	}

	void RunAway()
	{
		counter += Time.deltaTime;
		if (counter > 2.5f) 
		{
			randomPos = new Vector3 (Random.Range (-10, 10), transform.position.y, Random.Range (-10, 10));
			counter = 0;
		}
		Quaternion rotation = Quaternion.LookRotation(randomPos - transform.position, Vector3.up);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * m_boss.m_rotationSpeed);

		Vector3 heading = randomPos - transform.position;
		heading.y = 0;
		float distance = heading.magnitude;
		Vector3 direction = heading / distance;

		transform.position += transform.forward * Time.deltaTime * m_boss.m_speed;
	}

	public override void OnEnter()
	{
        Vector3 relativePoint = transform.InverseTransformPoint(m_boss.player.transform.position);
        if (relativePoint.x < 0.0f)
            m_boss.m_animator.SetBool("LeftRight", true); //Left
        else if (relativePoint.x > 0.0f)
            m_boss.m_animator.SetBool("LeftRight", false); //Right

		m_boss.m_animator.SetBool ("IsAttacking", true);
	}

	public override void OnExit()
	{
		m_boss.m_animator.SetBool ("IsAttacking", false);
		frames = 0;
        m_boss.m_sound.StopWalk();
	}
}
