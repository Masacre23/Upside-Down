using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class IsometricCamera : MonoBehaviour {

	public GameObject player;
	public GameObject[] lerpPoints;
	public int actualPoint;
	public Vector3 dir;

	public int zoom = 2;
	public int normal = 60;
	public float m_turnSpeed = 2f;

	//private bool isZoomed = false;

	float m_lookAngleX;
	float m_lookAngleY;
	float m_tiltAngle;
	float m_tiltMin = 45f;
	float m_tiltMax = 60f;

	public float FOV;
	// Update is called once per frame
	void Update () {
		float x = CrossPlatformInputManager.GetAxis ("Mouse X") * 2.5f;
		float y = CrossPlatformInputManager.GetAxis ("Mouse Y") * 2.5f;

		//m_lookAngleX += x * m_turnSpeed;
		//m_lookAngleY += y * m_turnSpeed;
		m_lookAngleX = Mathf.Lerp(m_lookAngleX, x, m_turnSpeed * Time.deltaTime);
		m_lookAngleY = Mathf.Lerp(m_lookAngleY, y, m_turnSpeed * Time.deltaTime);

		transform.LookAt (player.transform.position + transform.right * m_lookAngleX + transform.up * m_lookAngleY, player.transform.up);
		if(actualPoint != -1 && transform.parent == null && dir == Vector3.zero)
			transform.position = Vector3.Lerp (transform.position, lerpPoints [actualPoint].transform.position, Time.deltaTime);

		if (dir != Vector3.zero)
			transform.position = Vector3.Lerp (transform.position, player.transform.position + dir * 5 + player.transform.up * 5, Time.deltaTime);
		
		FOV = normal - Vector3.Distance (transform.position, player.transform.position) * zoom + 10;
		GetComponent<Camera> ().fieldOfView = FOV;
	}
}
