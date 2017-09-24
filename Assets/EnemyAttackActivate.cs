using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackActivate : MonoBehaviour {

    private EnemyAttacCollider m_enemyCollider;

    void Start()
    {
        m_enemyCollider = GetComponentInChildren<EnemyAttacCollider>();
    }

    public void CanAttack(bool attack)
    {
        m_enemyCollider.CanAttack(attack);
    }
}
