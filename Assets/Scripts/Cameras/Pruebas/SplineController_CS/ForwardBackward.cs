using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardBackward : MonoBehaviour {
	public bool forward = false;
	public bool backward = false;

	void OnTriggerEnter(Collider col)
	{
		transform.parent.gameObject.GetComponent<SplineInterpolator> ().forward = forward;
		transform.parent.gameObject.GetComponent<SplineInterpolator> ().backward = backward;
	}
}
