using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCamera : MonoBehaviour {

    public GameObject target;
    public GameObject planet;
    public Vector3 offset = new Vector3(0, 0, 0);
    public bool lookat = true;
    public float distance = 10;
    public float damping = 1.0f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = (planet.transform.position - transform.position).normalized;
        Vector3 desiredPosition = target.transform.position + dir * -distance - offset;
        Vector3 position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);
        if(lookat)
            transform.LookAt(target.transform);
        transform.position = position;
    }
}
