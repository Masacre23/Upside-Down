using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingOnWaterCubic : MonoBehaviour
{
    public GameObject m_prefabEffect;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            Vector3 direction = other.transform.position - transform.position;

            if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
            {
                if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.z))
                {
                    direction.y = 0.0f;
                    direction.z = 0.0f;
                }
                else
                {
                    direction.x = 0.0f;
                    direction.y = 0.0f;
                }
            }
            else if (Mathf.Abs(direction.y) >= Mathf.Abs(direction.z))
            {
                direction.x = 0.0f;
                direction.z = 0.0f;
            }
            else
            {
                direction.x = 0.0f;
                direction.y = 0.0f;
            }

            EffectsManager.Instance.GetEffect(m_prefabEffect, other.transform.position, direction.normalized, transform);

            SoundEffects sound = other.gameObject.GetComponent<SoundEffects>();
            if (sound)
                sound.PlaySound("SplashWater");

            if (other.gameObject.layer == LayerMask.NameToLayer("ThrowableObject"))
                other.gameObject.SetActive(false);
        }
    }
}
