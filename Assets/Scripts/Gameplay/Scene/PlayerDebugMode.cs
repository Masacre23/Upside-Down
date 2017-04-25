using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDebugMode : MonoBehaviour {

    public Transform p1;
    public Transform p2;
    public Transform p3;
    public Transform p4;

    GameObject m_player;
    Player m_playerClass;

    //Debug options
    bool m_debugMode = false;
    bool m_debugPlayerMode = false;
    bool m_activePerformance = false;
    bool m_activeCoordinates = false;
    bool m_activeGravityCoord = false;
    bool m_activeWireframe = false;
    bool m_activeDrawAxis = false;

    //Debug menu variables
    int m_screenWidth;
    int m_screenHeight;

    GUIStyle m_stylePlayerDebugMenu;
    Rect m_rectPlayerDebugMenu;
    string m_titlePlayerDebugMenu;

    GUIStyle m_styleFixedSpawnsMenu;
    Rect m_rectFixedSpawnsMenu;
    string m_titleFixedSpawnsMenu;

    GUIStyle m_styleCurrentSpawnsMenu;
    Rect m_rectCurrentSpawnsMenu;
    string m_titleCurrentSpawnsMenu;

    GUIStyle m_styleHitPlayerMenu;
    Rect m_rectHitPlayerMenu;
    string m_titleHitPlayerMenu;

    GUIStyle m_styleKillPlayerMenu;
    Rect m_rectKillPlayerMenu;
    string m_titleKillPlayerMenu;

    Color m_colorActive;
    Color m_colorInactive;
    Color m_colorInfo;

    //This function will be called on start.
    void Start()
    {
        m_player = GameObject.Find("Player");
        m_playerClass = m_player.GetComponent<Player>();

        m_debugMode = false;
        m_debugPlayerMode = false;

        m_screenWidth = Screen.width;
        m_screenHeight = Screen.height;

        m_colorActive = new Color(0.0f, 1.0f, 0.0f, 1.0f);
        m_colorInactive = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        m_colorInfo = new Color(0.0f, 0.0f, 1.0f, 1.0f);

        m_stylePlayerDebugMenu = new GUIStyle();
        m_stylePlayerDebugMenu.alignment = TextAnchor.UpperLeft;
        m_stylePlayerDebugMenu.fontSize = 4 * m_screenHeight / 100;
        m_stylePlayerDebugMenu.normal.textColor = m_colorInfo;
        m_rectPlayerDebugMenu = new Rect(m_screenWidth / 8, 0, m_screenWidth / 4, 4 * m_screenHeight / 100);
        m_titlePlayerDebugMenu = "Player Debug Mode";

        m_styleFixedSpawnsMenu = new GUIStyle();
        m_styleFixedSpawnsMenu.alignment = TextAnchor.UpperLeft;
        m_styleFixedSpawnsMenu.fontSize = 2 * m_screenHeight / 100;
        m_styleFixedSpawnsMenu.normal.textColor = m_colorInactive;
        m_rectFixedSpawnsMenu = new Rect(m_screenWidth / 8, 4 * m_screenHeight / 100, m_screenWidth / 8, 2 * m_screenHeight / 100);
        m_titleFixedSpawnsMenu = "1 - 4 : Spawn points";

        m_styleCurrentSpawnsMenu = new GUIStyle();
        m_styleCurrentSpawnsMenu.alignment = TextAnchor.UpperLeft;
        m_styleCurrentSpawnsMenu.fontSize = 2 * m_screenHeight / 100;
        m_styleCurrentSpawnsMenu.normal.textColor = m_colorInactive;
        m_rectCurrentSpawnsMenu = new Rect(m_screenWidth / 8, 6 * m_screenHeight / 100, m_screenWidth / 8, 2 * m_screenHeight / 100);
        m_titleCurrentSpawnsMenu = "5 : Spawn points";

        m_styleHitPlayerMenu = new GUIStyle();
        m_styleHitPlayerMenu.alignment = TextAnchor.UpperLeft;
        m_styleHitPlayerMenu.fontSize = 2 * m_screenHeight / 100;
        m_styleHitPlayerMenu.normal.textColor = m_colorInactive;
        m_rectHitPlayerMenu = new Rect(m_screenWidth / 8, 8 * m_screenHeight / 100, m_screenWidth / 8, 2 * m_screenHeight / 100);
        m_titleHitPlayerMenu = "8 - Hit player";

        m_styleKillPlayerMenu = new GUIStyle();
        m_styleKillPlayerMenu.alignment = TextAnchor.UpperLeft;
        m_styleKillPlayerMenu.fontSize = 2 * m_screenHeight / 100;
        m_styleKillPlayerMenu.normal.textColor = m_colorInactive;
        m_rectKillPlayerMenu = new Rect(m_screenWidth / 8, 10 * m_screenHeight / 100, m_screenWidth / 8, 2 * m_screenHeight / 100);
        m_titleKillPlayerMenu = "9 - Kill player";        
        
    }

    //This will be called once per frame. Here we take get input and compute fps
    void Update()
    {

        if (Input.GetKeyDown("f11"))
        {
            m_debugPlayerMode = !m_debugPlayerMode;
        }

        if (m_debugPlayerMode)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                Spawn(p1);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                Spawn(p2);
            if (Input.GetKeyDown(KeyCode.Alpha3))
                Spawn(p3);
            if (Input.GetKeyDown(KeyCode.Alpha4))
                Spawn(p4);
            if (Input.GetKeyDown(KeyCode.Alpha5))
                Spawn(m_playerClass.m_checkPoint);
            if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Alpha9))
                m_playerClass.HitDebug();
        }

    }

    void Spawn(Transform tr)
    {
        transform.position = tr.position;
        transform.rotation = tr.rotation;
        m_playerClass.Restart();
    }

    //This function would be called automatically each time there is event. It may result in more than once per frame (no logic here!).
    //Used to draw to the Debug options
    void OnGUI()
    {

        if (m_debugPlayerMode)
        {
            GUI.Label(m_rectPlayerDebugMenu, m_titlePlayerDebugMenu, m_stylePlayerDebugMenu);
            m_stylePlayerDebugMenu.normal.textColor = m_colorInfo;
            GUI.Label(m_rectFixedSpawnsMenu, m_titleFixedSpawnsMenu, m_styleFixedSpawnsMenu);
            m_styleFixedSpawnsMenu.normal.textColor = m_colorInactive;
            GUI.Label(m_rectCurrentSpawnsMenu, m_titleCurrentSpawnsMenu, m_styleCurrentSpawnsMenu);
            m_styleCurrentSpawnsMenu.normal.textColor = m_colorInactive;
            GUI.Label(m_rectHitPlayerMenu, m_titleHitPlayerMenu, m_styleHitPlayerMenu);
            m_styleHitPlayerMenu.normal.textColor = m_colorInactive;
            GUI.Label(m_rectKillPlayerMenu, m_titleKillPlayerMenu, m_styleKillPlayerMenu);
            m_styleKillPlayerMenu.normal.textColor = m_colorInactive;
        }

    }
}
