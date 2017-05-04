using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAnimation : DamageStates {

    private const float m_animtionTime = 0.5f;
    private float m_currentTime;

    public override void Start()
    {
        base.Start();
        m_type = States.ANIMATION;
        m_currentTime = 0.0f;
    }

    //Main camera update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public override bool OnUpdate(DamageData data)
    {
        bool ret = false;
        m_currentTime += Time.fixedDeltaTime;

        //m_character.gameObject.transform.Translate(data.m_force * Time.fixedDeltaTime);

        if (m_currentTime >= m_animtionTime)
        {
            m_character.m_damageState = m_character.m_notRecive;
            ret = true;
        }

        return ret;
    }

    public override void OnEnter()
    {
        m_character.m_rigidBody.AddForce(m_character.m_damage.m_force, ForceMode.VelocityChange);
		base.m_character.m_animator.SetBool("Damaged", true);
        m_currentTime = 0.0f;
        if (m_character is Player)
        {
            m_character.GetComponent<Player>().m_negatePlayerInput = true;
        }
    }

    public override void OnExit()
    {
        m_currentTime = 0.0f;  
		base.m_character.m_animator.SetBool("Damaged", false);
        if (m_character is Player)
        {
            m_character.GetComponent<Player>().m_negatePlayerInput = false;
        }
    }
}
