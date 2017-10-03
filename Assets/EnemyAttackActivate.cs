using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackActivate : MonoBehaviour {

    private EnemyAttacCollider m_enemyCollider;

    void Start()
    {
        m_enemyCollider = GetComponentInChildren<EnemyAttacCollider>();
    }

    public void CanAttack()
    {
        m_enemyCollider.CanAttack();
    }

    public void CannotAttack()
    {
        m_enemyCollider.CannotAttack();
    }
}
