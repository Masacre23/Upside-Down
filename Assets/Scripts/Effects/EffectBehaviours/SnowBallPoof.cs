using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBallPoof : MonoBehaviour {
    public GameObject m_prefabEffect;
    public float m_disappearVelocity = 2.0f; 

    private bool m_disappear = false;

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        if(m_disappear)
        {
            if (transform.localScale.x > 0.01)
            {
                Vector3 scale = Vector3.one * m_disappearVelocity * Time.deltaTime;
                transform.localScale = transform.localScale - scale;
            }else
            {
                transform.gameObject.SetActive(false);
            } 
        }
    }


    void OnCollisionEnter(Collision col)
    {
        if(col.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            EffectsManager.Instance.GetEffect(m_prefabEffect, transform);
            m_disappear = true;
        }
    }
}
