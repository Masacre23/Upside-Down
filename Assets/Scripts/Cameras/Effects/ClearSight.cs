using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearSight : MonoBehaviour {

    public Transform player;

    void Update()
    {
        RaycastHit[] hits;
        // you can also use CapsuleCastAll()
        // TODO: setup your layermask it improve performance and filter your hits.
        hits = Physics.RaycastAll(transform.position, player.position - transform.position, (transform.position - player.position).magnitude);
        foreach (RaycastHit hit in hits)
        {
            HideObjects hitObject = hit.collider.GetComponent<HideObjects>();
            if(hitObject != null)
                hitObject.HideObject = true;
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, (player.position - transform.position));
    }
#endif
}
