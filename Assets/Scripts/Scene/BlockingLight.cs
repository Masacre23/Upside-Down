using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingLight : MonoBehaviour
{
    public float m_minimumIntensity = 1.0f;
    public float m_maximumIntensity = 8.0f;

    public GameObject m_viewReference;
    public List<Light> m_lights;

    float m_maximumDistance;

    int layerMask;

    void Awake()
    {
        SphereCollider sphere = GetComponent<SphereCollider>();
        m_maximumDistance = 2.0f * sphere.radius * transform.parent.localScale.x;
    }

	// Use this for initialization
	void Start ()
    {
        if (!m_viewReference)
            m_viewReference = GameObject.Find("Player");

        layerMask = 1 << LayerMask.NameToLayer("BlockLight");
    }
	
	// Update is called once per frame
	void Update ()
    {
        float decrease = m_maximumIntensity - m_minimumIntensity;

        foreach (Light light in m_lights)
        {
            Vector3 referenceToLight = light.transform.position - m_viewReference.transform.position;
            float distance = referenceToLight.magnitude;

            RaycastHit referenceTarget;
            if (Physics.Raycast(m_viewReference.transform.position, referenceToLight.normalized, out referenceTarget, distance, layerMask))
            {
                RaycastHit lightTarget;
                if (Physics.Raycast(light.transform.position, -referenceToLight.normalized, out lightTarget, distance, layerMask))
                {
                    float blockDistance = (referenceTarget.point - lightTarget.point).magnitude;
                    light.intensity = m_maximumIntensity - decrease * blockDistance / m_maximumDistance;
                }
            }
            else
            {
                light.intensity = m_maximumIntensity;
            }
        }
	}
}
