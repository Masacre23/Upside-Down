using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {
    public GameObject m_gravityPanel;
    public Image m_sight;

    public Sprite[] m_lifeSprites;
    public Image[] m_collectableImages;
    public Image m_imageLife;

    private int m_lifeIndex = 0;
    private int m_collectableIndex = 0;

    private static bool m_showGravityPanel = false;
    private static bool m_isGreen = false;
    private static bool m_changeLife = false;
    private static bool m_newCollectable = false;
    private static float[] m_fillAmount = { 1.0f, 0.667f, 0.334f, 0 };
    private static Sprite m_newCollectableSprite;

    void Update ()
    {
        m_gravityPanel.SetActive(m_showGravityPanel);
        m_sight.color = m_isGreen ? new Color(0.0f, 1.0f, 0.0f, 1.0f) : new Color(1.0f, 0.0f, 0.0f, 1.0f);

        if (m_changeLife)
        {
            m_changeLife = false;
            ++m_lifeIndex;
            if(m_lifeIndex < m_lifeSprites.Length)
                m_imageLife.sprite = m_lifeSprites[m_lifeIndex];
            if(m_lifeIndex < m_fillAmount.Length)
            m_imageLife.fillAmount = m_fillAmount[m_lifeIndex];
        }
        if (m_newCollectable)
        {
            m_newCollectable = false;
            m_collectableImages[m_collectableIndex].gameObject.SetActive(true);
            m_collectableImages[m_collectableIndex].sprite = m_newCollectableSprite;
            ++m_collectableIndex;
        }
    }

    public static void ShowGravityPanel(bool showGravityPanel)
    {
        m_showGravityPanel = showGravityPanel;
    }

    public static void ChangeColorSight(bool isGreen)
    {
        m_isGreen = isGreen;
    }

    public static void LostLife()
    {
        m_changeLife = true;
    }

    public static void GetCollectable(Sprite sprite)
    {
        m_newCollectableSprite = sprite;
        m_newCollectable = true;
    }
}
