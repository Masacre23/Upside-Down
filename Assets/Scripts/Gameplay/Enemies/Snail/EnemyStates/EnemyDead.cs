using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDead : EnemyStates
{
   // string m_animationName = "Death";
	float counter;

    public override void Start ()
    {
		base.Start();
        m_type = States.DEAD;
    }

	public override bool OnUpdate (DamageData data)
    {
		bool ret = false;

        //AnimatorStateInfo animatorInfo = m_enemy.m_animator.GetCurrentAnimatorStateInfo(0);
        //if (animatorInfo.IsName(m_animationName) && animatorInfo.normalizedTime >= 1.0f)
        //{
        //    OnExit(data);
        //}
		counter += Time.deltaTime/10;
		m_enemy.m_renderer.sharedMaterial.SetFloat("_DisAmount", counter);
		if (counter > 1)
			gameObject.SetActive (false);

        return ret;
	}

	public override void OnEnter()
	{
		/*foreach(Collider c in GetComponents<Collider> ()) {
			c.enabled = true;
		}*/
		if (m_enemy.m_type == Enemy.Types.SNAIL) 
		{
			m_enemy.m_animator.SetInteger ("HitDirection", (int)m_enemy.hitDirection);
			m_enemy.m_animator.SetBool ("Dead", true);
			m_enemy.m_renderer.material = m_enemy.m_transparentMat;
			m_enemy.m_rendererShell.material = m_enemy.m_transparentMat;
			m_enemy.m_renderer.sharedMaterial.SetFloat ("_DisAmount", 0);
		}
        else
            this.gameObject.SetActive(false);
    }

	public override void OnExit()
	{
        if (m_enemy.m_type == Enemy.Types.SNAIL)
            m_enemy.m_animator.SetBool("Dead", false);
    }
}
