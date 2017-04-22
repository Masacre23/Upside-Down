using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour {

    public static EffectsManager m_instance;

    private Transform m_defaultParent;
    private Dictionary<GameObject, List<GameObject>> m_poolEffects;

    void Awake()
    {
        m_instance = this;
        m_defaultParent = this.transform;
    }

	void Start ()
    {
        m_poolEffects = new Dictionary<GameObject, List<GameObject>>();
	}
	
	public GameObject GetEffect(GameObject effectPrefab, Vector3 effectPosition, Transform effectParent = null)
    {
        GameObject ret = null;

        if (effectPrefab == null)
            return null;

        ret = GetEffectFromPool(effectPrefab);

        ret.SetActive(true);

        ret.transform.position = effectPosition;

        if (effectParent == null)
            ret.transform.parent = m_defaultParent;
        else
            ret.transform.parent = effectParent;

        return ret;
    }

    private GameObject GetEffectFromPool(GameObject effectPrefab)
    {
        GameObject ret = null;
        List<GameObject> listInstances;

        if (m_poolEffects.ContainsKey(effectPrefab))
        {
            m_poolEffects.TryGetValue(effectPrefab, out listInstances);
            if (listInstances != null)
            {
                foreach (GameObject instance in listInstances)
                {
                    if (instance != null && !instance.activeInHierarchy)
                    {
                        ret = instance;
                        break;
                    }
                }
            }
        }
        else
        {
            ret = GameObject.Instantiate(effectPrefab);
            listInstances = new List<GameObject>();
            listInstances.Add(ret);
            m_poolEffects.Add(effectPrefab, listInstances);
        }

        return ret;
    }
}
