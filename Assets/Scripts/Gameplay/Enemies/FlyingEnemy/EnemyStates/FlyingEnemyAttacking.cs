using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyAttacking : EnemyStates
{
    int speed = 2;
    int damp = 6;
    //public bool canChange = true;
   // float radiusCollider;
    float capsuleRadius;
    public Vector3 target;
    float nextFire;
    float fireRate = 1000;
    public GameObject m_prefabEffect;

    public override void Start()
    {
        base.Start();
        m_type = States.ATTACKING;
    }

    public override bool OnUpdate(DamageData data)
    {
        bool ret = false;

        float distance = Vector3.Distance(m_enemy.player.transform.position, transform.position);
       // m_enemy.m_animator.SetFloat("PlayerDistance", distance);

       // if (m_enemy.m_animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
       // {
            Move();
       // }

        if (data.m_recive)
        {
            ret = true;
            m_enemy.DamageManager(data);
        }
        
        return ret;
    }

    public override void OnEnter()
    {
        m_type = States.ATTACKING;
      //  m_enemy.m_animator.SetBool("PlayerDetected", true);

      //  speed = m_enemy.m_speed;

        //radiusCollider = m_enemy.GetComponent<SphereCollider>().radius;
       // m_enemy.GetComponent<SphereCollider>().radius = 0;
    }

    public override void OnExit()
    {
       // m_enemy.GetComponent<SphereCollider>().radius = radiusCollider;
    }

    public void Move()
    {
        //Vector3 target = m_enemy.player.transform.position;
        target = m_enemy.player.transform.position;

        Vector3 difference = target - transform.position;

        float distanceToPlane = Vector3.Dot(transform.up, difference);
        Vector3 pointOnPlane = target - (transform.up * distanceToPlane);

        transform.LookAt(pointOnPlane, transform.up);
        //Quaternion rotationAngle = Quaternion.LookRotation (pointOnPlane, transform.up);
        //transform.rotation = Quaternion.Slerp (transform.rotation, rotationAngle, Time.deltaTime * damp);

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void Attack()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            EffectsManager.Instance.GetEffect(m_prefabEffect, transform, transform);
        }
    }
}
