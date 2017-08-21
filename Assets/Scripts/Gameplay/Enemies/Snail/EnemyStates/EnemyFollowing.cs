using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowing : EnemyStates
{

	int speed;
	int damp = 2;
	public bool canChange = true;
	float radiusCollider;
	float capsuleRadius;
	public Vector3 target;

	public float stunnedTime = 5;
	float timer ;
	private float nextAttack;
	public float attackRate = 2;

    public override void Start ()
    {
		base.Start();
        m_type = States.FOLLOWING;
    }

	public override bool OnUpdate (DamageData data)
    {
		bool ret = false;
		float distance = Vector3.Distance (m_enemy.player.transform.position, transform.position); 

		m_enemy.m_animator.speed = Random.Range(1, 3);
        m_enemy.m_animator.SetFloat("PlayerDistance", distance);

		AnimatorStateInfo info = m_enemy.m_animator.GetCurrentAnimatorStateInfo (0);
		if (info.IsName ("Walk")) 
		{
			m_enemy.m_animator.SetBool ("CanAttack", false);
			m_enemy.m_animator.speed = 1;
			Move ();
			Attack ();
		} else if (info.IsName ("Stunned")) 
		{
			if (timer < stunnedTime && m_enemy.m_animator.GetBool("Stunned")) 
			{
				timer += Time.deltaTime;
			}
			else 
			{
				timer = 0;
				m_enemy.m_animator.SetBool ("Stunned", false);
				this.gameObject.transform.GetChild (3).gameObject.GetComponent<ParticleSystem> ().Stop ();
			}
		}
        if (data.m_recive)
        {
            ret = true;
            m_enemy.DamageManager(data);
        }

        return ret;
	}

	public override void OnEnter()
	{
		m_type = States.FOLLOWING;

        if(m_enemy.m_animator != null)
		    m_enemy.m_animator.SetBool("PlayerDetected", true);

		speed = m_enemy.m_speed;

		radiusCollider = m_enemy.GetComponent<SphereCollider>().radius;
		m_enemy.GetComponent<SphereCollider>().radius = 0;

        SoundEffects sound = m_enemy.GetComponent<SoundEffects>();
        if(sound != null)
        {
            sound.PlaySoundLoop("Walk");
        }
	}

	public override void OnExit()
	{
		m_enemy.GetComponent<SphereCollider>().radius = radiusCollider;
        SoundEffects sound = m_enemy.GetComponent<SoundEffects>();
        if (sound != null)
        {
            sound.StopSoundLoop("Walk");
        }
    }

	public void Move()
	{
		//Vector3 target = m_enemy.player.transform.position;
		target = m_enemy.player.transform.position;

		Vector3 difference = target - transform.position;

		float distanceToPlane = Vector3.Dot(transform.up, difference);
        Vector3 pointOnPlane = target - (transform.up * distanceToPlane);

        //Original
        //transform.LookAt(pointOnPlane, transform.up);        

        switch (m_enemy.m_type)
        {
		case Enemy.Types.SNAIL:
			Vector3 dir = difference.normalized;
			RaycastHit hit;

                //Check forward raycast
               /* if (Physics.Raycast(transform.position, transform.forward, out hit, 0.5f))
                    if (hit.transform != transform)
                    {
                        Debug.DrawLine(transform.position, hit.point, Color.red);
                        dir += (Vector3.Cross(hit.normal, transform.right) + Vector3.Cross(hit.normal, transform.up)) * speed;
                    }

                Vector3 leftRay = transform.position - transform.right/2;
                Vector3 rightRay = transform.position + transform.right/2;

                if (Physics.Raycast(leftRay, transform.forward, out hit, 0.5f))
                    if (hit.transform != transform)
                    {
                        Debug.DrawLine(leftRay, hit.point, Color.red);
                        dir += (Vector3.Cross(hit.normal, transform.right) + Vector3.Cross(hit.normal, transform.up)) * speed;
                    }
                if (Physics.Raycast(rightRay, transform.forward, out hit, 0.5f))
                    if (hit.transform != transform)
                    {
                        Debug.DrawLine(rightRay, hit.point, Color.red);
                        dir += (Vector3.Cross(hit.normal, transform.right) + Vector3.Cross(hit.normal, transform.up)) * speed;
                    }
*/
			Quaternion rotationAngle = Quaternion.LookRotation (dir, transform.up);
            Quaternion temp = Quaternion.Slerp(transform.rotation, rotationAngle, Time.deltaTime * damp);
            transform.rotation = new Quaternion(transform.rotation.x, temp.y, transform.rotation.z, temp.w);
                /*if (Physics.Raycast(transform.position, transform.forward + transform.right, 1))
                {
                    transform.position += transform.right * speed * Time.deltaTime;
                }
                else*/
             transform.position += transform.forward * speed * Time.deltaTime;
             break;
        }
	}

    public void Attack()
    {
		if (Time.time > nextAttack) 
		{
			nextAttack = Time.time + attackRate;
			m_enemy.m_animator.SetBool ("CanAttack", true);
		}			
    }
}
