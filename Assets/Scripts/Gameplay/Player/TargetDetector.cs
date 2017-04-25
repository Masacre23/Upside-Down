using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : MonoBehaviour {

    public string m_tag;
    public List<GameObject> m_targets;

    Player m_player;

    void Awake()
    {
        m_targets = new List<GameObject>();
    }

    // Use this for initialization
    void Start ()
    {
        GameObject player = GameObject.Find("Player");
        m_player = player.GetComponent<Player>();

        switch (m_tag)
        {
            case "Enemy":
                SetUpCollider(new Vector3(0, m_player.m_capsuleHeight / 2, 0), m_player.m_throwDetectionRange);
                m_player.m_targetsDetectors.Add(m_tag, this);
                break;
            case "GravityWall":
                SetUpCollider(new Vector3(0, m_player.m_capsuleHeight / 2, 0), m_player.m_gravityRange);
                m_player.m_targetsDetectors.Add(m_tag, this);
                break;
            default:
                break;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {	
	}

    void SetUpCollider(Vector3 center, float radius)
    {
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
        if (col.tag.Contains(m_tag) && !m_targets.Contains(col.gameObject))
        {
            if (m_tag.Contains("Enemy"))
            {
                if (col.gameObject.GetComponent<Enemy>())
                    m_targets.Add(col.gameObject);
            }
            else
                m_targets.Add(col.gameObject);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (m_targets.Contains(col.gameObject))
            m_targets.Remove(col.gameObject);
    }
}
