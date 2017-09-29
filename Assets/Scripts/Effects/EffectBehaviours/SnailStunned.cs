using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailStunned : MonoBehaviour {

    public GameObject m_prefabEffect;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            /*Enemy enemy = GetComponentInParent<Enemy>();
            if(enemy != null)
            {
                enemy.m_sound.PlayCrash();
            }*/
            EffectsManager.Instance.GetEffect(m_prefabEffect, transform);
        }
    }
}
