using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetectorByTag : MonoBehaviour {

    public string m_tag;
    public List<GameObject> m_targets;

    void Awake()
    {
        m_targets = new List<GameObject>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag(m_tag))
        {
            m_targets.Add(col.gameObject);
            MarkObject markObject = col.GetComponent<MarkObject>();
            if (markObject)
                markObject.BeginMarking();
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (m_targets.Contains(col.gameObject))
        {
            m_targets.Remove(col.gameObject);
            MarkObject markObject = col.GetComponent<MarkObject>();
            if (markObject)
                markObject.StopMarking();
        }
    }
}
