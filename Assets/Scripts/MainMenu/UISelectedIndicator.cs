using UnityEngine;
using UnityEngine.UI;

public class UISelectedIndicator : MonoBehaviour {

    public float m_offset = 0.0f;

    private GameObject m_currentButton;
    private Text m_currentText;
    private Shadow m_currentShadow;
    private Image m_image;

    void Start()
    {
        m_currentButton = null;
        m_image = GetComponent<Image>();
    }

    void Update()
    {
        if(m_currentButton != null)
        {
            Color c = m_image.color;
            Color cs = m_currentShadow.effectColor;
            cs.a = c.a;
            m_currentShadow.effectColor = cs;
            Color ct = m_currentText.color;
            ct.a = c.a;
            m_currentText.color = ct;
        }
    }

    public void SelectNewButton(GameObject button)
    {
        if(m_currentButton != null)
        {
            Color cs = m_currentShadow.effectColor;
            cs.a = 1.0f;
            m_currentShadow.effectColor = cs;
            Color ct = m_currentText.color;
            ct.a = 1.0f;
            m_currentText.color = ct;
        }
        m_currentButton = button;
        m_currentShadow = button.GetComponent<Shadow>();
        m_currentText = button.GetComponent<Text>();
        Vector3 new_position = new Vector3(button.transform.position.x + m_offset, button.transform.position.y, button.transform.position.z);
        transform.position = new_position;
    }
}
