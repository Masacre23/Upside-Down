using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailAnimationEvents : MonoBehaviour
{
    [Header("SnowOnMoving")]
    public GameObject m_snowPrefab;
    public Transform m_prefabTransform;
	
	public void SnowOnMoving()
    {
        EffectsManager.Instance.GetEffect(m_snowPrefab, m_prefabTransform);
    }
}
