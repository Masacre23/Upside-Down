using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

    public float m_speed = 0.2f;
    public Sprite m_sprite;
    public GameObject m_prefabEffect;

    private Player m_player = null;
    private float m_aceleration = 2.0f;
    private float m_velocity = 0.0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_player != null)
        {
            gameObject.SetActive(false);
        }
        transform.Translate(new Vector3(0.0f, 0.0f, m_velocity * Time.deltaTime * m_speed));
        //transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + m_velocity * Time.deltaTime * m_speed);
        float last_velocity = m_velocity;
        m_velocity += m_aceleration * Time.deltaTime;
        if((m_velocity >= 1.0 && last_velocity < 1.0f) || (m_velocity <= -1.0 && last_velocity > -1.0))
        {
            m_aceleration = -m_aceleration;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        int player = LayerMask.NameToLayer("Player");
        if (col.gameObject.layer == player) {
            EffectsManager.Instance.GetEffect(m_prefabEffect, transform);
            /*SoundEffects sound = col.gameObject.GetComponent<SoundEffects>();
            if (sound != null)
            {
                sound.PlaySound("Collectable");
            }*/ // TODO: CHECK SOUND
            m_player = col.gameObject.GetComponent<Player>();
            HUDManager.GetCollectable();
        }
    }
}
