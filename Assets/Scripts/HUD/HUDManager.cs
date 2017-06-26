using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {
    public GameObject m_lifePanel;
    public GameObject m_gravityPanel;
    public Image m_sight;
    public Image m_energy;
    public Image m_oxigen;
    public Image m_plunger;
    public Animator m_signAnimator;
    public Sprite[] m_plungersSprites;
    public Sprite[] m_energySprites;
    public Character m_playerGravity;


    private static bool m_showLifePanel = true;
    private static bool m_showGravityPanel = false;

    private static bool m_isGreen = false;
    private static float m_floatTimeValue = 0.0f;
    private static float m_maxEnergyValue = 120.0f;
    private static float m_oxigenValue = 1.0f;
    private static int m_energyValue = 8;
    private static int m_energyMaxValue = 9;

    void Update ()
    {
        m_gravityPanel.SetActive(m_showGravityPanel);
        m_lifePanel.SetActive(m_showLifePanel);
        m_sight.color = m_isGreen ? new Color(0.0f, 1.0f, 0.0f, 1.0f) : new Color(1.0f, 0.0f, 0.0f, 1.0f);
        m_sight.fillAmount = m_floatTimeValue;
        m_energy.sprite = m_energySprites[m_energyValue];
        m_oxigen.fillAmount = m_oxigenValue;

        int index = (int) ((1 - m_oxigenValue) * (m_plungersSprites.Length - 1));
        if (index >= 0 && index < m_plungersSprites.Length)
            m_plunger.sprite = m_plungersSprites[index];
        //m_signAnimator.SetBool("PlanetGravity", !m_playerGravity.m_gravityOnCharacter.m_getAttractorOnFeet);
    }

    public static void ShowLifePanel(bool showLifePanel)
    {
        m_showLifePanel = showLifePanel;
    }

    public static void ShowGravityPanel(bool showGravityPanel)
    {
        m_showGravityPanel = showGravityPanel;
    }

    public static void SetMaxEnergyValue(float value)
    {
        m_maxEnergyValue = value;
    }

    public static void ChangeEnergyValue(float energy)
    {
        m_energyValue = (int)((energy / m_maxEnergyValue) * (m_energyMaxValue - 1));
    }

    public static void ChangeColorSight(bool isGreen)
    {
        m_isGreen = isGreen;
    }

    public static void ChangeFloatTime(float floatTime)
    {
        m_floatTimeValue = floatTime;
    }

    public static void ChangeOxigen(float oxigen)
    {
        m_oxigenValue = oxigen;
    }

}
