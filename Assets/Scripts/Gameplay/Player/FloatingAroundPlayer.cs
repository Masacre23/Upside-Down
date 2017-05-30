using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingAroundPlayer : MonoBehaviour
{
    public List<Transform> m_floatingPositions;
    Dictionary<Transform, GameObject> m_pickedObjects;
    public LayerMask m_layersPicking;
    public Player m_player;

    public GameObject m_markTarget;
    public bool m_targetSet = false;

    public float m_objectDetectionRadius = 1.5f;
    public float m_rotationSpeed = 200.0f;
    public float m_upDownAmplitude = 0.25f;
    public float m_upDownPeriod = 1.0f;
    public float m_upDownDelay = 0.3f;
    public float m_defaultTimeFloating = 20.0f;

    List<float> m_initialY;
    float m_timeUpDown = 0.0f;

    bool m_savedThrow = false;
    Vector3 m_savedPosition;
    float m_savedStrength = 0;

    // Use this for initialization
    void Start ()
    {
        m_pickedObjects = new Dictionary<Transform, GameObject>();
        m_initialY = new List<float>();
        for (int i = 0; i < m_floatingPositions.Count; i++)
            m_initialY.Add(m_floatingPositions[i].localPosition.y);

        if (m_markTarget)
            m_markTarget.SetActive(false);
        else
        {
            m_markTarget = GameObject.Find("MarkTargetSprite");
            if (m_markTarget)
                m_markTarget.SetActive(false);
        }

        m_player = GameObject.Find("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Rotation of the positions around the player
        transform.Rotate(0, m_rotationSpeed * Time.deltaTime, 0);

        m_timeUpDown += Time.deltaTime;
        while (m_timeUpDown > m_upDownPeriod)
            m_timeUpDown -= m_upDownPeriod;

        //Moving the positions up and down
        for (int i = 0; i < m_floatingPositions.Count; i++)
        {
            float frac = 2.0f * Mathf.PI * (m_timeUpDown - m_upDownDelay * i) / m_upDownPeriod;
            Vector3 new_position;
            new_position.x = m_floatingPositions[i].localPosition.x;
            new_position.y = m_initialY[i] + m_upDownAmplitude * Mathf.Sin(frac);
            new_position.z = m_floatingPositions[i].localPosition.z;
            m_floatingPositions[i].localPosition = new_position;
        }

        //Throwing saved throw
        if (m_savedThrow)
            ThrowSaved();

        if (m_targetSet)
            m_markTarget.transform.forward = Camera.main.transform.forward;
	}

    // This functions serves to pick the nearest objects (that are in m_layerPicking mask) to the player
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

    //This function is called to drop all pickup objects
    public void DropAll()
    {
        List<ThrowableObject> pickeds = new List<ThrowableObject>();

        foreach (GameObject pickedObject in m_pickedObjects.Values)
        {
            ThrowableObject throwScript = pickedObject.GetComponent<ThrowableObject>();
            if (throwScript)
                pickeds.Add(throwScript);
        }

        foreach (ThrowableObject picked in pickeds)
            picked.StopFloating();
    }

    //This functions serves to allocate the object in the nearest free position in the player
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

    //This function is called to throw an object to a target
    public void ThrowObjectToTarget(RaycastHit target, Transform playerOrigin, float throwForce)
    {
        Vector3 playerToTarget = target.point - playerOrigin.position;
        //We need to know which object is the one throwed:
        GameObject objectToThrow = null;
        //First, we check if the object is closer to the target than the player
        foreach (GameObject objectPicked in m_pickedObjects.Values)
        {
            Vector3 objectToTarget = target.point - objectPicked.transform.position;
            if (Vector3.Dot(playerToTarget.normalized, objectToTarget) < playerToTarget.magnitude)
            {
                objectToThrow = objectPicked;
                break;
            }
        }

        //If we cannot throw any, we need to save the target to throw it later
        if (objectToThrow != null)
            Throw(objectToThrow, target.point - objectToThrow.transform.position, throwForce);
        else
        {
            //Otherwise, we will need to wait for one of them to be in a correct position.
            m_savedThrow = true;
            m_savedPosition = target.point;
            m_savedStrength = throwForce;
        }
    }

    //This function is called to throw an object in a direction
    public void ThrowObjectToDirection(Transform playerOrigin, float maxAimLength, float throwForce)
    {
        Vector3 playerToTarget = playerOrigin.forward * maxAimLength;
        Vector3 finalPosition = playerOrigin.position + playerToTarget;

        //We need to know which object is the one throwed:
        GameObject objectToThrow = null;
        //First, we check if the object is closer to the target than the player
        foreach (GameObject objectPicked in m_pickedObjects.Values)
        {
            Vector3 objectToTarget = finalPosition - objectPicked.transform.position;
            if (Vector3.Dot(playerToTarget.normalized, objectToTarget) < playerToTarget.magnitude)
            {
                objectToThrow = objectPicked;
                break;
            }
        }

        //If we cannot throw any, we need to save the target to throw it later
        if (objectToThrow != null)
            Throw(objectToThrow, finalPosition - objectToThrow.transform.position, throwForce);
        else
        {
            //Otherwise, we will need to wait for one of them to be in a correct position.
            m_savedThrow = true;
            m_savedPosition = finalPosition;
            m_savedStrength = throwForce;
        }

    }

    //This function tries to throw a saved thrown
    private void ThrowSaved()
    {
        //We need to know which object is the one throwed:
        GameObject objectToThrow = null;
        Vector3 playerToTarget = m_savedPosition - m_player.transform.position;

        //Check if there are any obstacles between the object and the target
        foreach (GameObject objectPicked in m_pickedObjects.Values)
        {
            Vector3 objectToTarget = m_savedPosition - objectPicked.transform.position;
            if (Vector3.Dot(playerToTarget.normalized, objectToTarget) < playerToTarget.magnitude)
            {
                objectToThrow = objectPicked;
                break;
            }
        }
        
        //If we cannot throw any, we need to save the target to throw it later
        if (objectToThrow != null)
            Throw(objectToThrow, m_savedPosition - objectToThrow.transform.position, m_savedStrength);
    }

    //This function actually throws a specific object in a specific direction with an specific force
    private void Throw(GameObject objectToThrow, Vector3 throwVector, float throwForce)
    {
        ThrowableObject throwScript = objectToThrow.GetComponent<ThrowableObject>();
        if (throwScript)
            throwScript.ThrowObject(throwVector.normalized * throwForce);
        m_savedThrow = false;
    }

    //This function sets the target mark in a target position
    public void SetTarget(Vector3 position)
    {
        m_markTarget.transform.position = position;
        m_markTarget.transform.forward = Camera.main.transform.forward;
        m_markTarget.SetActive(true);
        m_targetSet = true;
    }

    //This function unsets the target mark
    public void UnsetTarget()
    {
        m_markTarget.SetActive(false);
        m_targetSet = false;
    }

    //This function serves to free a space for picking objects, called when they are thrown or fall
    public void FreeSpace(Transform transform)
    {
        m_pickedObjects.Remove(transform);
    }

    public int NumFloatingPositions()
    {
        return m_floatingPositions.Count;
    }

    public int NumPickedObjects()
    {
        return m_pickedObjects.Count;
    }

    public bool CanPickMoreObjects()
    {
        return m_pickedObjects.Count < m_floatingPositions.Count;
    }

    public bool HasObjectsToThrow()
    {
        return m_pickedObjects.Count > 0;
    }

    public bool EnemyIsFloating(GameObject enemy)
    {
        bool ret = false;

        foreach (GameObject objectFloating in m_pickedObjects.Values)
        {
            if (objectFloating == enemy)
                ret = true;
        }

        return ret;
    }
}
