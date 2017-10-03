using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

	float ShakeY = 0.8f;
	float ShakeYSpeed = 0.8f;

	public string m_inputNameButton = "Activate";

	void Update()
	{
		Vector2 _newPosition = new Vector2(0, ShakeY);
		if (ShakeY < 0)
		{
			ShakeY *= ShakeYSpeed;
		}
		ShakeY = -ShakeY;
		transform.Translate(_newPosition, Space.Self);

		if (Input.GetButtonDown (m_inputNameButton))
			ShakeY = 0.8f;
	}
}
