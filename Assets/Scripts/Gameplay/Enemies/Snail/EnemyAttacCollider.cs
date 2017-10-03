using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacCollider : MonoBehaviour {

    private Animator m_animator;
    private int m_playerLayer;
    private int m_enemyAttackLayer;

    [HideInInspector]public bool is_attacking = false;

    // Use this for initialization
    void Start () {
        m_animator = GetComponentInParent<Animator>();
        Enemy enemy = GetComponentInParent<Enemy>();
        m_playerLayer = LayerMask.NameToLayer("Player");
        m_enemyAttackLayer = LayerMask.NameToLayer("EnemyAttack");

    }

    public void CanAttack()
    {
        is_attacking = true;
        Physics.IgnoreLayerCollision(m_playerLayer, m_enemyAttackLayer, false);
    }

    public void CannotAttack()
    {
        is_attacking = false;
        Physics.IgnoreLayerCollision(m_playerLayer, m_enemyAttackLayer, true);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == m_playerLayer)
        {
            col.gameObject.GetComponent<Player>().m_damageData.m_recive = true;
            col.gameObject.GetComponent<Player>().m_damageData.m_damage = 20;

            Vector3 diff = transform.position - col.transform.position;
            float distance = diff.magnitude;
            Vector3 dir = diff / distance;
        }
    }
}
