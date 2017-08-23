using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {
    public GameObject m_gravityPanel;
    public GameObject m_lifePanel;
    public GameObject m_gameOverPanel;
    public GameObject m_winPanel;
    public Image m_sight;

    public Sprite[] m_lifeSprites;
    public Image[] m_collectableImages;
    public Image m_imageLife;

    private int m_lifeIndex = 0;
    private int m_collectableIndex = 0;

    private static bool m_showGravityPanel = false;
    private static bool m_isGreen = false;
    private static bool m_changeLife = false;
    private static bool m_gainLife = false;
    private static bool m_newCollectable = false;
    private static bool m_showGameOverPanel = false;
    private static bool m_showWinPanel = false;
    private static float[] m_fillAmount = { 1.0f, 1.0f, 1.0f, 1.0f };
    private static Sprite m_newCollectableSprite;

    void Start()
    {
        m_showGravityPanel = false;
        m_isGreen = false;
        m_changeLife = false;
        m_gainLife = false;
        m_newCollectable = false;
        m_showGameOverPanel = false;
        m_showWinPanel = false;
    }

    void Update ()
    {
        if (m_showGameOverPanel)
        {
            m_gameOverPanel.SetActive(true);
            m_gravityPanel.SetActive(false);
            m_lifePanel.SetActive(false);
        }
        else if ( m_showWinPanel )
        {
            m_winPanel.SetActive(true);
            m_gravityPanel.SetActive(false);
            m_lifePanel.SetActive(false);
        }
        else
        {
            m_gravityPanel.SetActive(m_showGravityPanel);
            m_sight.color = m_isGreen ? new Color(0.0f, 1.0f, 0.0f, 1.0f) : new Color(1.0f, 0.0f, 0.0f, 1.0f);

            if (m_changeLife)
            {
                m_changeLife = false;
                ++m_lifeIndex;
                if (m_lifeIndex < m_lifeSprites.Length)
                    m_imageLife.sprite = m_lifeSprites[m_lifeIndex];
                if (m_lifeIndex < m_fillAmount.Length)
                    m_imageLife.fillAmount = m_fillAmount[m_lifeIndex];
            }
            if (m_gainLife)
            {
                m_gainLife = false;
                --m_lifeIndex;
                if (m_lifeIndex < m_lifeSprites.Length)
                    m_imageLife.sprite = m_lifeSprites[m_lifeIndex];
                if (m_lifeIndex < m_fillAmount.Length)
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

    public static void GainLife()
    {
        m_gainLife = true;
    }

    public static void GetCollectable(Sprite sprite)
    {
        m_newCollectableSprite = sprite;
        m_newCollectable = true;
    }

    public static void ShowGameOverPanel(bool show)
    {
        m_showGameOverPanel = show;
    }

    public static void ShowWinPanel(bool show)
    {
        m_showWinPanel = show;
    }
}
