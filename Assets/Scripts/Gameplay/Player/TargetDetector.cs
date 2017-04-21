using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : MonoBehaviour {

    public List<GameObject> m_targets;
    string m_tag;

    // Use this for initialization
    void Start ()
    {
        m_targets = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update ()
    {	
	}

    public void SetUpCollider(string tag, Vector3 center, float radius)
    {
        m_tag = tag;
        SphereCollider targetDetector = GetComponent<SphereCollider>();
        targetDetector.isTrigger = true;
        if (targetDetector)
        {
            targetDetector.center = center;
            targetDetector.radius = radius;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == m_tag && !m_targets.Contains(col.gameObject))
        {
            m_targets.Add(col.gameObject);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (m_targets.Contains(col.gameObject))
            m_targets.Remove(col.gameObject);
    }
}
