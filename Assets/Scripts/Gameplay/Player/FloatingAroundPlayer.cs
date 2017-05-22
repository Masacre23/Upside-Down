using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingAroundPlayer : MonoBehaviour
{
    public List<Transform> m_floatingPositions;
    Dictionary<Transform, GameObject> m_pickedObjects;

    public LayerMask m_layersPicking;
    public float m_objectDetectionRadius = 1.5f;
    public float m_rotationSpeed = 200.0f;
    public float m_upDownSpeed = 1.0f;
    public float m_defaultTimeFloating = 20.0f;

    // Use this for initialization
    void Start ()
    {
        m_pickedObjects = new Dictionary<Transform, GameObject>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Rotation of the positions around the player
        transform.Rotate(0, m_rotationSpeed * Time.deltaTime, 0);

        //Moving the positions up and down
        foreach (Transform location in m_floatingPositions)
        {

        }
	}

    public void PickObjects(Vector3 detectionOrigin)
    {
        List<Collider> detectedObjects = new List<Collider>(Physics.OverlapSphere(detectionOrigin, m_objectDetectionRadius, m_layersPicking));
        detectedObjects.Sort(delegate (Collider a, Collider b) { return Vector3.Distance(detectionOrigin, a.transform.position).CompareTo(Vector3.Distance(detectionOrigin, b.transform.position)); });

        for (int i = 0; i < detectedObjects.Count; i++)
        {
            int indexTransform = m_pickedObjects.Count;
            int freeSpace = m_floatingPositions.Count - m_pickedObjects.Count;

            if (freeSpace <= 0)
                return;

            ThrowableObject throwableObject = detectedObjects[i].transform.GetComponent<ThrowableObject>();
            if (throwableObject && throwableObject.m_canBePicked)
            {
                Transform transformPosition = FindNearestPosition(throwableObject.gameObject);
                m_pickedObjects.Add(transformPosition, throwableObject.gameObject);
                throwableObject.BeginFloating(transformPosition, this, m_defaultTimeFloating);
            }
        }
    }

    private Transform FindNearestPosition(GameObject objectToPick)
    {
        Transform ret = null;

        float distance2 = 10000.0f;
        foreach (Transform transformPosition in m_floatingPositions)
        {
            if (!m_pickedObjects.ContainsKey(transformPosition))
            {
                float thisDistance2 = (objectToPick.transform.position - transformPosition.position).magnitude;
                if (thisDistance2 < distance2)
                {
                    distance2 = thisDistance2;
                    ret = transformPosition;
                }
            }
        }

        return ret;
    }

    public void FreeSpace(Transform transform)
    {
        m_pickedObjects.Remove(transform);
    }

    public void ThrowObject()
    {

    }

    public int NumFloatingPositions()
    {
        return m_floatingPositions.Count;
    }

    public int NumPickedObjects()
    {
        return m_pickedObjects.Count;
    }

    public bool CanPickMoreObejects()
    {
        return m_pickedObjects.Count < m_floatingPositions.Count;
    }

    public bool HasObjectsToThrow()
    {
        return m_pickedObjects.Count > 0;
    }
}
