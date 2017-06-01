using System;
using System.Collections;
using UnityEngine;

public class VariableCameraProtectFromWallClip : MonoBehaviour
{
    public float m_clipMoveTime = 0.05f;              // time taken to move when avoiding cliping (low value = fast, which it should be)
    public float m_returnTime = 0.4f;                 // time taken to move back towards desired position, when not clipping (typically should be a higher value than clipMoveTime)
    public float m_sphereCastRadius = 0.1f;           // the radius of the sphere used to test for object between camera and target
    public bool m_visualiseInEditor;                  // toggle for visualising the algorithm through lines for the raycast in the editor
    public float m_closestDistance = 0.5f;            // the closest distance the camera can be from the target
    public bool m_protecting { get; private set; }    // used for determining if there is an object between the target and the camera
    public string m_dontClipTag = "Player";           // don't clip against objects with this tag (useful for not clipping against the targeted object)

    bool m_protectionEnabled = true;

    private Transform m_Cam;                  // the transform of the camera
    private Transform m_Pivot;                // the point at which the camera pivots around
    private float m_OriginalDist;             // the original distance to the camera before any modification are made
    private float m_MoveVelocity;             // the velocity at which the camera moved
    private float m_CurrentDist;              // the current distance from the camera to the target
    private Ray m_Ray;                        // the ray used in the lateupdate for casting between the camera and the target
    private RaycastHit[] m_Hits;              // the hits between the camera and the target
    private RayHitComparer m_RayHitComparer;  // variable to compare raycast hit distances
    private Vector3 m_originalPosition;

    private Vector3 m_playerToCam;
    public LayerMask m_layersToIgnore;

    private void Start()
    {
        // find the camera in the object hierarchy
        m_Cam = GetComponentInChildren<Camera>().transform;
        m_Pivot = m_Cam.parent;
        m_originalPosition = m_Cam.localPosition;
        m_OriginalDist = -m_Cam.localPosition.z;
        m_CurrentDist = m_OriginalDist;

        m_playerToCam = m_Cam.position - (transform.position + transform.up);

        // create a new RayHitComparer
        m_RayHitComparer = new RayHitComparer();

    }


    private void LateUpdate()
    {
        if (m_protectionEnabled)
        {
            LayerMask layersToCollide = ~m_layersToIgnore;

            // initially set the target distance
            float targetDist = m_OriginalDist;

            m_playerToCam = m_Cam.position - (transform.position + transform.up);
            //m_Ray.origin = m_Pivot.position + m_Pivot.up * m_originalPosition.y + m_Pivot.forward * m_sphereCastRadius;
            //m_Ray.direction = -m_Pivot.forward;
            m_Ray.origin = transform.position + transform.up;
            m_Ray.direction = m_playerToCam;
            Debug.DrawRay(m_Ray.origin, m_Ray.direction, Color.cyan);


            // initial check to see if start of spherecast intersects anything
            var cols = Physics.OverlapSphere(m_Ray.origin, m_sphereCastRadius, layersToCollide);

            bool initialIntersect = false;
            bool hitSomething = false;

            // loop through all the collisions to check if something we care about
            for (int i = 0; i < cols.Length; i++)
            {
                if ((!cols[i].isTrigger) &&
                    !(cols[i].attachedRigidbody != null && cols[i].attachedRigidbody.CompareTag(m_dontClipTag)))
                {
                    initialIntersect = true;
                    break;
                }
            }

            // if there is a collision
            if (initialIntersect)
            {
                m_Ray.origin += m_playerToCam.normalized * m_sphereCastRadius;
                //m_Ray.origin += m_Pivot.forward * m_sphereCastRadius;

                // do a raycast and gather all the intersections
                m_Hits = Physics.RaycastAll(m_Ray, m_playerToCam.magnitude - m_sphereCastRadius, layersToCollide);
                //m_Hits = Physics.RaycastAll(m_Ray, m_OriginalDist - m_sphereCastRadius, ignorePlayer);
            }
            else
            {
                // if there was no collision do a sphere cast to see if there were any other collisions
                m_Hits = Physics.SphereCastAll(m_Ray, m_sphereCastRadius, m_playerToCam.magnitude + m_sphereCastRadius, layersToCollide);
                //m_Hits = Physics.SphereCastAll(m_Ray, m_sphereCastRadius, m_OriginalDist + m_sphereCastRadius, ignorePlayer);
            }

            // sort the collisions by distance
            Array.Sort(m_Hits, m_RayHitComparer);

            // set the variable used for storing the closest to be as far as possible
            float nearest = Mathf.Infinity;

            // loop through all the collisions
            for (int i = 0; i < m_Hits.Length; i++)
            {
                // only deal with the collision if it was closer than the previous one, not a trigger, and not attached to a rigidbody tagged with the dontClipTag
                if (m_Hits[i].distance < nearest && (!m_Hits[i].collider.isTrigger) &&
                    !(m_Hits[i].collider.attachedRigidbody != null &&
                      m_Hits[i].collider.attachedRigidbody.CompareTag(m_dontClipTag)))
                {
                    // change the nearest collision to latest
                    nearest = m_Hits[i].distance;
                    targetDist = -m_Pivot.InverseTransformPoint(m_Hits[i].point).z;
                    hitSomething = true;
                }
            }

            // visualise the cam clip effect in the editor
            if (hitSomething)
            {
                Debug.DrawRay(m_Ray.origin, -m_playerToCam * (targetDist + m_sphereCastRadius), Color.red);
                //Debug.DrawRay(m_Ray.origin, -m_Pivot.forward * (targetDist + m_sphereCastRadius), Color.red);
            }

            // hit something so move the camera to a better position
            m_protecting = hitSomething;
            m_CurrentDist = Mathf.SmoothDamp(m_CurrentDist, targetDist, ref m_MoveVelocity,
                                           m_CurrentDist > targetDist ? m_clipMoveTime : m_returnTime);
            m_CurrentDist = Mathf.Clamp(m_CurrentDist, m_closestDistance, m_OriginalDist);
            m_Cam.localPosition = -Vector3.forward * m_CurrentDist + Vector3.up * m_originalPosition.y;
        }
    }


    // comparer for check distances in ray cast hits
    public class RayHitComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            return ((RaycastHit)x).distance.CompareTo(((RaycastHit)y).distance);
        }
    }

    public void SetProtection(bool protectCam)
    {
        m_protectionEnabled = protectCam;
    }
}
