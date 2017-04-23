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


    private static bool m_showLifePanel = true;
    private static bool m_showGravityPanel = false;

    private static bool m_isGreen = false;
    private static float m_floatTimeValue = 0.0f;
    private static float m_maxEnergyValue = 120.0f;
    private static float m_oxigenValue = 1.0f;
    private static int m_energyValue = 7;
    private static float[] m_energyValues = new float[7] { 0.0f, 0.175f, 0.329f, 0.5f, 0.675f, 0.821f, 1.0f };
	// Update is called once per frame
	void Update ()
    {
        m_gravityPanel.SetActive(m_showGravityPanel);
        m_lifePanel.SetActive(m_showLifePanel);
        m_sight.color = m_isGreen ? new Color(0.0f, 1.0f, 0.0f, 1.0f) : new Color(1.0f, 0.0f, 0.0f, 1.0f);
        m_sight.fillAmount = m_floatTimeValue;
        m_energy.fillAmount = m_energyValues[m_energyValue];
        m_oxigen.fillAmount = m_oxigenValue;
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
        m_energyValue = (int)((energy / m_maxEnergyValue) * (m_energyValues.Length - 1));
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
