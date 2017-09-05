using UnityEngine;
using System.Collections;

public class TomaPantalla : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.F10))
        {
            string screenshotIMGName = System.DateTime.Now.ToString();
            string subString = screenshotIMGName.Replace('/', '_');
            string gypsy = subString.Replace(':', '_');
            Debug.Log("Screen shot captured: " + gypsy + ".png");
            Application.CaptureScreenshot(gypsy + ".png");
        }
    }
}