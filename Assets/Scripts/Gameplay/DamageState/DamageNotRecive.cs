using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNotRecive : DamageStates {

    private float m_notReciveTime = 5.0f;
    private float m_speedPaint = 0.5f;
    private float m_currentTime;
    SkinnedMeshRenderer[] meshes;

    public override void Start()
    {
        base.Start();
        m_type = States.NOT_RECIVE;
        m_currentTime = 0.0f;
        meshes = m_charapter.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    //Main camera update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public override bool OnUpdate(bool recive, int damage)
    {
        bool ret = false;
        m_currentTime += Time.fixedDeltaTime;

        float aWithDecimal = m_currentTime / m_speedPaint;
        int aWithoutDecimal = (int)aWithDecimal;
        float a = aWithDecimal - aWithoutDecimal;
        for (int i = 0; i<meshes.Length; i++)
        {
            meshes[i].enabled = (a > 0.5);
        }

        if (m_currentTime >= m_notReciveTime)
        {
            m_charapter.m_damageState = m_charapter.m_recive;
            ret = true;
        }

        return ret;
    }

    public override void OnEnter()
    {
        m_currentTime = 0.0f;
    }

    public override void OnExit()
    {
        m_currentTime = 0.0f;

        for (int i = 0; i < meshes.Length; i++)
        {
            meshes[i].enabled = true;
        }
    }
}
