using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBall : MonoBehaviour {

	public GameObject m_prefabEffect;
	public float m_disappearVelocity = 2.0f; 

	private bool m_disappear = false;
    public bool snowman;
    private SnowBallSoundEffects m_sound;

	// Use this for initialization
	void Start () {
        m_sound = GetComponent<SnowBallSoundEffects>();
	}
	
	// Update is called once per frame
	void Update () {
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

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player") 
		{
			col.gameObject.GetComponent<Player> ().m_damageData.m_recive = true;
			col.gameObject.GetComponent<Player> ().m_damageData.m_damage = 20;
		}
		if (col.tag == "Player" || col.gameObject.layer == 21 || snowman? col.tag == "Boss" : false) {
            if (snowman)
                GameObject.Find("MechanicLaserManager").GetComponent<MechanicLaserManager> ().DisableLaser();
			EffectsManager.Instance.GetEffect (m_prefabEffect, transform);
            if(m_sound != null)
            {
                m_sound.PlayCrash();
            }
			m_disappear = true;
		}
	}
}
