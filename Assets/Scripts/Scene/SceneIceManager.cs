using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneIceManager : MonoBehaviour {

    public GameObject canvas;
    Image fade;

	// Use this for initialization
	void Start () {
        canvas.SetActive(true);
        fade = GameObject.Find("Fade").GetComponent<Image>();
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        while (fade.color.a > 0)
        {
            Color temp = fade.color;
            temp.a -= Time.deltaTime / 5;
            fade.color = temp;
            yield return 0;
        }
    }
}
