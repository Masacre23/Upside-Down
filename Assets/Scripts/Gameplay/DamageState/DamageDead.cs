using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDead : DamageStates {

    public override void Start()
    {
        base.Start();
        m_type = States.DEAD;
    }

    //Main camera update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public override bool OnUpdate(bool recive, int damage)
    {
        bool ret = false;

        return ret;
    }

    public override void OnEnter()
    {
         
    }

    public override void OnExit()
    {
       
    }
}
