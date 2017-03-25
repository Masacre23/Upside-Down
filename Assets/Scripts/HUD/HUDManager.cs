using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {
    public GameObject m_lifePanel;
    public GameObject m_gravityPanel;
    public Image m_sight;
    public Slider m_floatTime;
    public Slider m_energy;

    private static bool m_showLifePanel = true;
    private static bool m_showGravityPanel = false;

    private static bool m_isGreen = false;
    private static float m_floatTimeValue = 0.0f;
    private static float m_energyValue = 1.0f;
	// Update is called once per frame
	void Update () {
        m_gravityPanel.SetActive(m_showGravityPanel);
        m_lifePanel.SetActive(m_showLifePanel);
        m_sight.color = m_isGreen ? new Color(0.0f, 1.0f, 0.0f, 0.5f) : new Color(1.0f, 0.0f, 0.0f, 0.5f);
        m_floatTime.value = m_floatTimeValue;
        m_energy.value = m_energyValue;
    }

    public static void ShowLifePanel(bool showLifePanel)
    {
        m_showLifePanel = showLifePanel;
    }

    public static void ShowGravityPanel(bool showGravityPanel)
    {
        m_showGravityPanel = showGravityPanel;
    }

    public static void ChangeEnergyValue(float energy)
    {
        m_energyValue = energy;
    }

    public static void ChangeColorSight(bool isGreen)
    {
        m_isGreen = isGreen;
    }

    public static void ChangeFloatTime(float floatTime)
    {
        m_floatTimeValue = floatTime;
    }

}
