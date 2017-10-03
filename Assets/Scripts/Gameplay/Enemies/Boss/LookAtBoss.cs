using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtBoss : MonoBehaviour {
	GameObject boss;
	//public float maxAngle;
	// float minAngle;

	// Use this for initialization
	public void Start () {
		boss = GameObject.Find ("Boss");
	}
	
	// Update is called once per frame
	void Update () {
		if (boss) 
		{
			transform.LookAt (boss.transform, transform.right);

			// get the normalized target direction:
			/*Vector3 dir = (boss.transform.position - transform.position).normalized;
			float maxSin = Mathf.Sin(maxAngle * Mathf.Deg2Rad); // get sine of max angle
			float minSin = Mathf.Sin(minAngle * Mathf.Deg2Rad); // get sine of min angle
			float sine = Mathf.Clamp(dir.y, minSin, maxSin); // get the clamped angle sine
			float cos = Mathf.Sqrt(1 - (sine * sine)); // calculate the cosine with Pythagoras
			// compound the new direction vector:
			dir = new Vector3(dir.x, 0, dir.z).normalized * cos; // set the horizontal direction...
			dir.y = sine; // and set the vertical component
			Quaternion qTo = Quaternion.LookRotation(dir, Vector3.up); // look at the new direction
			//transform.rotation = Quaternion.RotateTowards(transform.rotation, qTo, maxDegreesPerSecond * Time.deltaTime);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, qTo, Time.deltaTime);*/
		}	
	}
}
