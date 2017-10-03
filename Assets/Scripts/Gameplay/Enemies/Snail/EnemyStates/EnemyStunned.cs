using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStunned: EnemyStates {

    public float stunnedTime = 5;
    float timer;

    // Use this for initialization
    public override void Start ()
    {
        timer = 0;
		base.Start ();
        m_type = States.STUNNED;
    }
	
	// Update is called once per frame
	public override bool OnUpdate (DamageData data, bool stunned)
    {
        bool ret = false;
        ThrowableObject throwableObject = m_enemy.gameObject.GetComponent<ThrowableObject>();
        if (timer < stunnedTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            if (!throwableObject || !throwableObject.m_isCarring)
            {
                m_enemy.m_currentState = m_enemy.m_Idle;
                ret = true;
            }
        }

        if (data.m_recive)
        {
            ret = true;
            data.m_damage *= 2;
            m_enemy.DamageManager(data);
        }

        return ret;
    }

	public override void OnEnter()
	{
        ThrowableObject throwableObject = m_enemy.gameObject.GetComponent<ThrowableObject>();
        if (throwableObject)
            throwableObject.m_canBePicked = true;
        m_enemy.m_animator.SetBool("Stunned", true);
        transform.GetChild(3).gameObject.SetActive(true);
        transform.GetChild(3).gameObject.GetComponent<ParticleSystem>().Play();
        m_enemy.m_wasStunned = false;
    }

	public override void OnExit()
	{
        ThrowableObject throwableObject = m_enemy.gameObject.GetComponent<ThrowableObject>();
        if (throwableObject)
            throwableObject.m_canBePicked = false;
        timer = 0;
        m_enemy.m_animator.SetBool("Stunned", false);
        ParticleSystem ps = transform.GetChild(3).gameObject.GetComponent<ParticleSystem>();
        ParticleSystem ps_child = ps.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.particleCount];
        int num = ps.GetParticles(particles);
        for (int i = 0; i < num; i++)
        {
            particles[i].remainingLifetime = 0;
        }
        ps.SetParticles(particles, num);
        particles = new ParticleSystem.Particle[ps_child.particleCount];
        num = ps_child.GetParticles(particles);
        for (int i = 0; i < num; i++)
        {
            particles[i].remainingLifetime = 0;
        }
        ps_child.SetParticles(particles, num);
        ps.Stop();
    }
}
