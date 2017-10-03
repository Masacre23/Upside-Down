using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour {

	public float speed = 0.5f;

	void Start () {
		float distance = Vector3.Distance(Camera.main.transform.position, gameObject.transform.position);
		float height = 2.0f * Mathf.Tan(0.5f * Camera.main.fieldOfView * Mathf.Deg2Rad) * distance;
		float width = 4 * height * Screen.width / Screen.height;
		gameObject.transform.localScale = new Vector3(width, height, 1);
	}

	// Update is called once per frame
	void Update () {
		Vector2 offset = new Vector2 (Time.time * speed, 0);
		GetComponent<Renderer> ().material.mainTextureOffset = offset;
	}
}
