using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacCollider : MonoBehaviour {

    private GameObject m_prefabHit1;
    private Animator m_animator;
    private int m_playerLayer;
    private int m_enemyAttackLayer;

    // Use this for initialization
    void Start () {
        m_animator = GetComponentInParent<Animator>();
        Enemy enemy = GetComponentInParent<Enemy>();
        m_prefabHit1 = enemy.m_prefabHit1;
        m_playerLayer = LayerMask.NameToLayer("Player");
        m_enemyAttackLayer = LayerMask.NameToLayer("EnemyAttack");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CanAttack(bool attack)
    {
        Physics.IgnoreLayerCollision(m_playerLayer, m_enemyAttackLayer, !attack);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == m_playerLayer)
        {

            col.gameObject.GetComponent<Player>().m_damageData.m_recive = true;
            col.gameObject.GetComponent<Player>().m_damageData.m_damage = 20;

            Vector3 diff = transform.position - col.transform.position;
            float distance = diff.magnitude;
            Vector3 dir = diff / distance;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, dir, out hit, 1f))
            {
                EffectsManager.Instance.GetEffect(m_prefabHit1, col.transform.position + transform.up / 2 + col.transform.forward / 2, transform.up, null);
            }
        }
    }
}
