using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class should be added to any GameObject which gravity can be changed during the game.
//It controls the current gravity of the object, and adds it to its rigid body.
public class GameObjectGravity : MonoBehaviour
{
    [SerializeField] private float m_speedAutoExitAttractor = 10.0f;
    [SerializeField] private float m_maxTimeGravity = 2.0f;
    [SerializeField] private float m_maxDistanceFromAttractor = 8.0f;
    private float m_timeGravity = 0.0f;
    [HideInInspector] public Rigidbody m_rigidBody;
    private RaycastHit m_attractor;
    public GameObject m_attractorGameObject { get; private set; }
    public Vector3 m_gravity { get; private set; }
    public List<Rigidbody> m_planets;
    public bool m_planetGravity { get; private set; }
    public bool m_getAttractorOnFeet { get; private set; }
    public bool m_changingToAttractor { get; set; }
    public float m_maxTimeTravelled = 0.5f;

    public bool m_ignoreGravity = false;
    float m_timeTravelled;
    Vector3 m_impulseForce;

    public bool m_inPlanetGravity = true;

    public bool m_getStrongestGravity = true;

    //This should be the same for all gameobjects
    static float m_gravityStrength = -19.0f;

    Player m_player;

    void Awake()
    {
        m_planets = new List<Rigidbody>();

        m_rigidBody = GetComponent<Rigidbody>();
        m_rigidBody.useGravity = false;
        m_rigidBody.velocity = Vector3.zero;

        m_gravity = Vector3.zero;
    }

	// Use this for initialization
	void Start ()
    {
        m_planetGravity = true;
        m_changingToAttractor = false;
        m_impulseForce = Vector3.zero;
        m_getAttractorOnFeet = false;
        m_attractorGameObject = null;

        m_player = GetComponent<Player>();

        m_inPlanetGravity = m_planetGravity;
    }
	
    public void Update()
    {
        m_inPlanetGravity = m_planetGravity;
        if (!m_changingToAttractor && !m_planetGravity)
        {
            if ((transform.position - m_attractor.point).sqrMagnitude > 1.0f)
            {
                ReturnToPlanet();
            }
        }
    }

	// Called to add gravity force into the rigid body.
	public void FixedUpdate ()
    {
		float strength = 0;

        if (!m_ignoreGravity)
        {
            if (!m_planetGravity || m_planets.Count == 0)
            {
                strength = m_gravityStrength;
            }
            else
            {
                Vector3 gravity = Vector3.zero;
                if (m_getStrongestGravity)
                    GetStrongestPlanetGravity(ref strength, ref gravity);
                else
                    GetSumPlanetGravity(ref strength, ref gravity);

                m_gravity = gravity;
            }

            m_rigidBody.AddForce(strength * m_rigidBody.mass * m_gravity);
        }
        else
        {
            m_timeTravelled += Time.fixedDeltaTime;
            if (m_timeTravelled > m_maxTimeTravelled)
            {
                m_timeTravelled = 0.0f;
                m_ignoreGravity = false;
            }
        }
    }

    public void UpdatePlanetGravity()
    {
        float strength = 0;
        Vector3 gravity = Vector3.zero;
        if (m_getStrongestGravity)
            GetStrongestPlanetGravity(ref strength, ref gravity);
        else
            GetSumPlanetGravity(ref strength, ref gravity);
        m_gravity = gravity;
    }

    //Get the strongest gravity among the planets currently affecting the object
    private void GetStrongestPlanetGravity(ref float strength, ref Vector3 directionGravity)
    {
        foreach (Rigidbody planet in m_planets)
        {
            Vector3 newGravity = transform.position - planet.transform.position;
            float distance = newGravity.magnitude;
            newGravity.Normalize();

            float newStrength = GravityStrength(planet.mass, distance);
            if (newStrength < strength)
            {
                strength = newStrength;
                directionGravity = newGravity;
            }
        }
    }

    //Get the sum of all the planet gravities affecting the object
    private void GetSumPlanetGravity(ref float strength, ref Vector3 directionGravity)
    {
        Vector3 gravity = Vector3.zero;
        foreach (Rigidbody planet in m_planets)
        {
            Vector3 newGravity = transform.position - planet.transform.position;
            float distance = newGravity.magnitude;
            newGravity.Normalize();
            float newStrength = GravityStrength(planet.mass, distance);

            gravity += newGravity * newStrength;
        }

        strength = - gravity.magnitude;
        directionGravity = - gravity.normalized;
    }

    //Function to compute planet gravity strength
    private float GravityStrength(float mass, float distance)
    {
        return - mass / (25.0f * distance);
    }

    //This function sets the passed raycasthit as the current attractor for the game object
    //It's mainly to be used by characters in order update its attractor while they walk on an attractor, not on planet
    public void GravityOnFeet(RaycastHit hit)
    {
        if (hit.collider.tag == "GravityWall" && m_getAttractorOnFeet)
        {
            m_attractor = hit;
            m_attractorGameObject = hit.transform.gameObject;
            m_gravity = hit.normal;
            m_planetGravity = false;
            m_changingToAttractor = false;
        }
        else
        {
            m_planetGravity = true;
            m_attractorGameObject = null;
        }
    }

    //This function changes object gravity so it falls towards the collision point
    public void ChangeGravityToPoint(RaycastHit attractor, Vector3 objectPosition)
    {
        m_attractor = attractor;
        m_attractorGameObject = attractor.transform.gameObject;
        m_gravity = (objectPosition - attractor.point).normalized;
    }

    //This function changes object gravity so it falls into a direction equal to the normal of the position hit
    public void ChangeToNormal(RaycastHit attractor)
    {
        m_attractor = attractor;
        m_attractorGameObject = attractor.transform.gameObject;
        m_gravity = attractor.normal;
    }

    //This functions only changes the attractor, without changing the current gravity
    public void SetAttractor(RaycastHit attractor)
    {
        m_attractor = attractor;
        m_attractorGameObject = attractor.transform.gameObject;
    }

    //This function is called to disable the gravity from attractors, and to return to the planet gravity
    public void ReturnToPlanet()
    {
        m_planetGravity = true;
        m_changingToAttractor = false;
        m_attractorGameObject = null;
        m_getAttractorOnFeet = false;
    }

    //This function is called when the player begins changing to an attractor
    public void ChangeToAttractor()
    {
        m_planetGravity = false;
        m_changingToAttractor = true;
        m_getAttractorOnFeet = true;
    }

    //This function is called when we want a object to float. Usually called when player is floating while changing gravity
    //or when an object is floating due the player throwing them.
    public void Float(Vector3 initialPosition, Vector3 finalPosition, float percentage)
    {
        float perc = 2 * percentage - percentage * percentage * percentage;
        m_rigidBody.transform.position = Vector3.Slerp(initialPosition, finalPosition, perc);
    }

    //This function is similar, but to be used only with non-kinematic rigidBodies 
    //(in order to avoid passing through colliders when performing a translation or RigidBody.Move)
    public void FloatVelocity(Vector3 finalPosition, float speedFactor)
    {
        Vector3 distance = finalPosition - transform.position;
        Vector3 speed = distance / Time.fixedDeltaTime;
        speed = speedFactor > 1.0f ? speed : speed * speedFactor;
        m_rigidBody.velocity = speed;
    }
}
