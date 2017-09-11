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
    public Sprite[] m_collectableSprites;
    public Image m_imageLife;
    public Image m_collectableNumber;

    private int m_lifeIndex = 0;

    private static bool m_showGravityPanel = false;
    private static bool m_isGreen = false;
    private static bool m_changeLife = false;
    private static bool m_gainLife = false;
    private static bool m_newCollectable = false;
    private static bool m_showGameOverPanel = false;
    private static bool m_showWinPanel = false;
    private static float[] m_fillAmount = { 1.0f, 1.0f, 1.0f, 1.0f };
    private static Sprite m_newCollectableSprite;
    private static int m_numberCollectable = 0;

    void Start()
    {
        m_showGravityPanel = false;
        m_isGreen = false;
        m_changeLife = false;
        m_gainLife = false;
        m_newCollectable = false;
        m_showGameOverPanel = false;
        m_showWinPanel = false;
        m_collectableNumber.sprite = m_collectableSprites[m_numberCollectable];
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
                m_collectableNumber.sprite = m_collectableSprites[m_numberCollectable];
                m_newCollectable = false;
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

    public static void GetCollectable()
    {
        m_newCollectable = true;
        m_numberCollectable++;
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
