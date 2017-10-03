using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class AsteroidCamera : MonoBehaviour {

	public Transform target;
	public Transform reference;
	public float upDistance    = 0.5f;
	public float backDistance  = 0.75f;
	public float trackingSpeed = 0.5f;
	public float rotationSpeed = 1.0f;

	private Vector3 v3To;
	private Quaternion qTo;

	float m_lookAngleX;
	float m_lookAngleY;
	public float m_turnSpeed = 2f;

	void LateUpdate () {
		Vector3 v3Up = (target.position - reference.position).normalized;
		//v3To = target.position - target.forward * backDistance + v3Up * upDistance;
		v3To = target.position + v3Up * upDistance - reference.up * backDistance;
		transform.position = Vector3.Lerp (transform.position, v3To, trackingSpeed * Time.deltaTime);

		float x = CrossPlatformInputManager.GetAxis ("Mouse X") * 2.5f;
		float y = CrossPlatformInputManager.GetAxis ("Mouse Y") * 2.5f;

		m_lookAngleX = Mathf.Lerp(m_lookAngleX, x, m_turnSpeed * Time.deltaTime);
		m_lookAngleY = Mathf.Lerp(m_lookAngleY, y, m_turnSpeed * Time.deltaTime);

		qTo = Quaternion.LookRotation(target.position - transform.position + transform.right * m_lookAngleX + transform.up * m_lookAngleY, v3Up);
		transform.rotation = Quaternion.Slerp (transform.rotation, qTo, rotationSpeed * Time.deltaTime);
	}
}
