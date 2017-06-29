using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowing : EnemyStates
{

	int speed;
	int damp = 6;
	public bool canChange = true;
	float radiusCollider;
	float capsuleRadius;
	public Vector3 target;
    public GameObject m_prefabEffect;

    //Flying enemy
    private float nextFire;
    private float fireRate = 2;
    private float innerRadiusToPlayer = 10;
    private float outterRadiusToPlayer = 20;
    //private GameObject planet;

    public void Awake()
    {
        if (!m_prefabEffect)
            m_prefabEffect = (GameObject)Resources.Load("Prefabs/Effects/CFX3_IceBall_A", typeof(GameObject));  
    }

    public override void Start ()
    {
		base.Start();
        m_type = States.FOLLOWING;
        //planet = transform.parent.gameObject;
    }

	public override bool OnUpdate (DamageData data)
    {
		bool ret = false;

		float distance = Vector3.Distance (m_enemy.player.transform.position, transform.position); 

        if (m_enemy.m_animator != null)
        {
			//m_enemy.m_animator.SetInteger("RandomAnimation", Random.Range(0, 2));
			m_enemy.m_animator.speed = Random.Range(1, 3);
            m_enemy.m_animator.SetFloat("PlayerDistance", distance);

            if (m_enemy.m_animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) // 1 is flying enemy
            {
				m_enemy.m_animator.speed = 1;
                Move();
            }
        }
        else
            Move();

        if (m_enemy.m_type == Enemy.Types.FLYING)
            Attack();

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

        //Pruebas
        //Quaternion rotationAngle = Quaternion.LookRotation (pointOnPlane, transform.up);
        //transform.rotation = Quaternion.Slerp (transform.rotation, rotationAngle, Time.deltaTime * damp*2);
        

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
                Quaternion rotationAngle = Quaternion.LookRotation(dir, transform.up);
                Quaternion temp = Quaternion.Slerp(transform.rotation, rotationAngle, Time.deltaTime * damp);
                transform.rotation = new Quaternion(transform.rotation.x, temp.y, transform.rotation.z, temp.w);
                /*if (Physics.Raycast(transform.position, transform.forward + transform.right, 1))
                {
                    transform.position += transform.right * speed * Time.deltaTime;
                }
                else*/
                    transform.position += transform.forward * speed * Time.deltaTime;
                break;
          /*  case Enemy.Types.FLYING:
                if(difference.sqrMagnitude < innerRadiusToPlayer )
                    transform.position -= transform.forward * speed * Time.deltaTime;
                else if(difference.sqrMagnitude > outterRadiusToPlayer)
                    transform.position += transform.forward * speed * Time.deltaTime;

                float distance_playerY = (m_enemy.player.transform.position - planet.transform.position + m_enemy.player.transform.up).sqrMagnitude;
                float distance_enemyY = (transform.position - planet.transform.position).sqrMagnitude;

                if(distance_playerY > distance_enemyY && distance_playerY - distance_enemyY > speed)
                    transform.position += transform.up * speed * Time.deltaTime;
                else if (distance_playerY < distance_enemyY && distance_enemyY - distance_playerY  > speed)
                    transform.position -= transform.up * speed * Time.deltaTime;

                break;*/
        }
	}

    public void Attack() // Only for flying enemy
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            EffectsManager.Instance.GetEffect(m_prefabEffect, transform);
        }
    }
}
