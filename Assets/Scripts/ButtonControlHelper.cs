using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControlHelper : MonoBehaviour {

    [System.Serializable]
    struct ButtonInfo
    {
        public string m_buttonName;
        public Sprite m_sprite;
    }

    [SerializeField] ButtonInfo[] m_buttons;
    Dictionary<string, Sprite> m_buttonsDictionary;

    SpriteRenderer m_spriteRenderer;

    bool m_showingButton = false;

    void Awake()
    {
        m_buttonsDictionary = new Dictionary<string, Sprite>();

        foreach (ButtonInfo button in m_buttons)
        {
            m_buttonsDictionary.Add(button.m_buttonName, button.m_sprite);
        }

        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //if (m_showingButton)
        //    transform.forward = Camera.main.transform.forward;
	}

    public void SetImage(string name)
    {
        if (m_buttonsDictionary.ContainsKey(name))
        {
            m_spriteRenderer.sprite = m_buttonsDictionary[name];
            m_showingButton = true;
        }
        else
            UnsetImage();
    }

    public void UnsetImage()
    {
        m_spriteRenderer.sprite = null;
        m_showingButton = false;
    }
}
