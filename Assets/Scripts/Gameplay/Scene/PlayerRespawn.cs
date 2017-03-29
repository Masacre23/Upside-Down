using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRespawn : MonoBehaviour {
	public GameObject[] spawnPoints;
	float fadeTime = 1.0f;
	public Image fadePanel;
	GameObject player;

	void Start()
	{
		player = GameObject.Find ("Player");
	}

	IEnumerator Fading()
	{
		for (float t = 0.0f; t < fadeTime;) 
		{
			t += Time.deltaTime / (fadeTime);
			fadePanel.color = new Color (0f, 0f, 0f, t);
			yield return null;
		}

		player.transform.position = spawnPoints [0].transform.position;
		player.transform.rotation = spawnPoints [0].transform.rotation;
		player.transform.GetChild(0).transform.rotation = spawnPoints [0].transform.rotation;
        player.GetComponent<Player>().Start();
        player.GetComponent<Rigidbody>().ResetInertiaTensor();

		for (float t = fadeTime; t > 0.0f;) 
		{
			t -= Time.deltaTime / (fadeTime);
			fadePanel.color = new Color (0f, 0f, 0f, t);
			yield return null;
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.name == "Player")
			StartCoroutine (Fading ());
	}
}
