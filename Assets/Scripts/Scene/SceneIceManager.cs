using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneIceManager : MonoBehaviour {

    public GameObject canvas;
    public Image fade;
    public Image[] controls;
    public Text[] controlsT;
    Data data;
    public Toggle toggle;
    bool startChange = true;

	// Use this for initialization
	void Start () {
        canvas.SetActive(true);
      //  fade = GameObject.Find("Fade").GetComponent<Image>();
        StartCoroutine(Fade());
        if(controls[0])
        StartCoroutine(FadeControls());

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

    private IEnumerator FadeControls()
    {
        while (controls[0].color.a < 1)
        {
            for (int i = 0; i < controls.Length; i++)
            {
                Color temp = controls[i].color;
                temp.a += Time.deltaTime / 5;
                controls[i].color = temp;
            }

            for (int i = 0; i < controlsT.Length; i++)
            {
                Color temp = controlsT[i].color;
                temp.a += Time.deltaTime / 5;
                controlsT[i].color = temp;
            }
            yield return 0;
        }

        while (controls[0].color.a > 0)
        {
            for (int i = 0; i < controls.Length; i++)
            {
                Color temp = controls[i].color;
                temp.a -= Time.deltaTime / 5;
                controls[i].color = temp;
            }

            for (int i = 0; i < controlsT.Length; i++)
            {
                Color temp = controlsT[i].color;
                temp.a -= Time.deltaTime / 5;
                controlsT[i].color = temp;
            }
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
