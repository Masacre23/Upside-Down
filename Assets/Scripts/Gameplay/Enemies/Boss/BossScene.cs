using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossScene : MonoBehaviour {
	public int maxTime;
	public int speed;

	float time;
    Image fade;
    public bool dialogueFinished = false;
    bool b = true;

	// Use this for initialization
	void Start () {
		transform.GetChild (0).gameObject.SetActive (true);
        fade = GameObject.Find("Fade").GetComponent<Image>();
	}

	void FixedUpdate () {
		time += Time.deltaTime;
        if (dialogueFinished)
            transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        if (time < maxTime)
			transform.Translate (-Vector3.up * Time.deltaTime * speed);
        else if(dialogueFinished)
        {
            transform.GetChild(0).GetComponent<Animator>().SetBool("Attack", true);
            StartCoroutine(EndLevel());
        }
	}

    private IEnumerator EndLevel()
    {
        if (b)
        {
            b = false;
            while (fade.color.a < 1)
            {
                Color temp = fade.color;
                temp.a += Time.deltaTime/5;
                fade.color = temp;
                yield return 0;
            }
            SceneManager.LoadScene(2);
        }
    }
}
