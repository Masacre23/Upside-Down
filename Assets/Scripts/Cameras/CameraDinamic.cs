using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDinamic : MonoBehaviour {
    float time;
    public GameObject[] points;
    public int counter;
    Vector3 startingPos;
    Vector3 startingRot;
    Vector3[] positions;
    Quaternion[] rotations;
    float numPointsSpline;

    void Start () {
        startingPos = transform.position;
        startingRot = transform.rotation.eulerAngles;

        numPointsSpline = 1.0f / 0.001f;
        positions = new Vector3[(int)numPointsSpline * points.Length];
        rotations = new Quaternion[(int)numPointsSpline * points.Length];
        for (int i = 1; i < points.Length + 1; i++)
        {
            for (int j = 0; j < numPointsSpline; j++)
            {
                positions[j] = points[0].GetComponent<SplineSection>().GetPositionAt(j * 0.001f * i);
                rotations[j] = points[0].GetComponent<SplineSection>().GetRotationAt(j * 0.001f * i);
            }
        }
    }
	
	void FixedUpdate () {
        time += 0.1f;

        counter++;
        transform.position = positions[counter];
        transform.rotation = rotations[counter];
    }
}
