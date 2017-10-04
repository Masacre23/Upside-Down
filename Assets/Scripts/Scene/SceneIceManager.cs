using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneIceManager : MonoBehaviour {

    public GameObject canvas;
    public Image fade;
    Data data;
    public Toggle toggle;
    bool startChange = true;

	// Use this for initialization
	void Start () {
        canvas.SetActive(true);
      //  fade = GameObject.Find("Fade").GetComponent<Image>();
        StartCoroutine(Fade());
        data = GameObject.Find("Data").GetComponent<Data>();
        startChange = data.spanish;
        if(toggle)
            toggle.isOn = data.spanish;
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

    public void ToggleDialogues()
    {
        if (startChange)
            startChange = false;
        else
            data.ToggleDialogues();
    }
}
