using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class controls the DebugMode of the game.
//It can be included in any GameObject (Camera is a good choice).
//Debug options are activated with F12.
public class DebugMode : MonoBehaviour {

    float m_averageDeltaTime = 0.0f;
    ulong m_numTris = 0;

    //Debug options
    bool m_debugMode = false;
    bool m_activePerformance = false;
    bool m_activeCoordinates = false;
    bool m_activeGravityCoord = false;
    bool m_activeWireframe = false;
    bool m_activeDrawAxis = false;

    //Debug menu variables
    int m_screenWidth;
    int m_screenHeight;

    GUIStyle m_styleDebugMenu;
    Rect m_rectDebugMenu;
    string m_titleDebugMenu;

    GUIStyle m_stylePerformanceMenu;
    Rect m_rectPerformanceMenu;
    string m_titlePerformanceMenu;

    GUIStyle m_stylePerformanceInfo;
    Rect m_rectPerformanceInfo;

    GUIStyle m_styleCoordinatesMenu;
    Rect m_rectCoordinatesMenu;
    string m_titleCoordinatesMenu;

    GUIStyle m_styleCoordinatesInfo;
    Rect m_rectCoordinatesInfo;
    GameObject m_player;

    GUIStyle m_styleGravityMenu;
    Rect m_rectGravityMenu;
    string m_titleGravityMenu;

    GUIStyle m_styleGravityInfo;
    Rect m_rectGravityInfo;
    GameObjectGravity m_playerGravity;

    GUIStyle m_styleWireframeMenu;
    Rect m_rectWireframeMenu;
    string m_titleWireframeMenu;

    GUIStyle m_styleDrawAxisMenu;
    Rect m_rectDrawAxisMenu;
    string m_titleDrawAxisMenu;

    Color m_colorActive;
    Color m_colorInactive;
    Color m_colorInfo;

    float m_axisLength = 1.5f;
    Material m_material;

    //This function will be called on start.
    void Start()
    {
        m_player = GameObject.Find("Player");
        m_playerGravity = m_player.GetComponent<GameObjectGravity>();

        m_debugMode = false;
        m_averageDeltaTime = 0.0f;

        m_screenWidth = Screen.width;
        m_screenHeight = Screen.height;

        m_colorActive = new Color(0.0f, 1.0f, 0.0f, 1.0f);
        m_colorInactive = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        m_colorInfo = new Color(0.0f, 0.0f, 1.0f, 1.0f);

        m_styleDebugMenu = new GUIStyle();
        m_styleDebugMenu.alignment = TextAnchor.UpperLeft;
        m_styleDebugMenu.fontSize = 4 * m_screenHeight / 100;
        m_styleDebugMenu.normal.textColor = m_colorInfo;
        m_rectDebugMenu = new Rect(0, 0, m_screenWidth / 4, 4 * m_screenHeight / 100);
        m_titleDebugMenu = "Debug Mode";

        m_stylePerformanceMenu = new GUIStyle();
        m_stylePerformanceMenu.alignment = TextAnchor.UpperLeft;
        m_stylePerformanceMenu.fontSize = 2 * m_screenHeight / 100;
        m_stylePerformanceMenu.normal.textColor = m_colorInactive;
        m_rectPerformanceMenu = new Rect(0, 4 * m_screenHeight / 100, m_screenWidth / 8, 2 * m_screenHeight / 100);
        m_titlePerformanceMenu = "F1 - Performance";

        m_stylePerformanceInfo = new GUIStyle();
        m_stylePerformanceInfo.alignment = TextAnchor.UpperLeft;
        m_stylePerformanceInfo.fontSize = 2 * m_screenHeight / 100;
        m_stylePerformanceInfo.normal.textColor = m_colorInfo;
        m_rectPerformanceInfo = new Rect(m_screenWidth / 8, 4 * m_screenHeight / 100, m_screenWidth / 8, 2 * m_screenHeight / 100);

        m_styleCoordinatesMenu = new GUIStyle();
        m_styleCoordinatesMenu.alignment = TextAnchor.UpperLeft;
        m_styleCoordinatesMenu.fontSize = 2 * m_screenHeight / 100;
        m_styleCoordinatesMenu.normal.textColor = m_colorInactive;
        m_rectCoordinatesMenu = new Rect(0, 6 * m_screenHeight / 100, m_screenWidth / 8, 2 * m_screenHeight / 100);
        m_titleCoordinatesMenu = "F2 - Coordinates";

        m_styleCoordinatesInfo = new GUIStyle();
        m_styleCoordinatesInfo.alignment = TextAnchor.UpperLeft;
        m_styleCoordinatesInfo.fontSize = 2 * m_screenHeight / 100;
        m_styleCoordinatesInfo.normal.textColor = m_colorInfo;
        m_rectCoordinatesInfo = new Rect(m_screenWidth / 8, 6 * m_screenHeight / 100, m_screenWidth / 8, 2 * m_screenHeight / 100);

        m_styleGravityMenu = new GUIStyle();
        m_styleGravityMenu.alignment = TextAnchor.UpperLeft;
        m_styleGravityMenu.fontSize = 2 * m_screenHeight / 100;
        m_styleGravityMenu.normal.textColor = m_colorInactive;
        m_rectGravityMenu = new Rect(0, 8 * m_screenHeight / 100, m_screenWidth / 8, 2 * m_screenHeight / 100);
        m_titleGravityMenu = "F3 - Gravity";

        m_styleGravityInfo = new GUIStyle();
        m_styleGravityInfo.alignment = TextAnchor.UpperLeft;
        m_styleGravityInfo.fontSize = 2 * m_screenHeight / 100;
        m_styleGravityInfo.normal.textColor = m_colorInfo;
        m_rectGravityInfo = new Rect(m_screenWidth / 8, 8 * m_screenHeight / 100, m_screenWidth / 8, 2 * m_screenHeight / 100);

        m_styleWireframeMenu = new GUIStyle();
        m_styleWireframeMenu.alignment = TextAnchor.UpperLeft;
        m_styleWireframeMenu.fontSize = 2 * m_screenHeight / 100;
        m_styleWireframeMenu.normal.textColor = m_colorInactive;
        m_rectWireframeMenu = new Rect(0, 10 * m_screenHeight / 100, m_screenWidth / 8, 2 * m_screenHeight / 100);
        m_titleWireframeMenu = "F4 - Wireframe";

        m_styleDrawAxisMenu = new GUIStyle();
        m_styleDrawAxisMenu.alignment = TextAnchor.UpperLeft;
        m_styleDrawAxisMenu.fontSize = 2 * m_screenHeight / 100;
        m_styleDrawAxisMenu.normal.textColor = m_colorInactive;
        m_rectDrawAxisMenu = new Rect(0, 12 * m_screenHeight / 100, m_screenWidth / 8, 2 * m_screenHeight / 100);
        m_titleDrawAxisMenu = "F5 - World Axis";

        Shader shader = Shader.Find("Hidden/Internal-Colored");
        m_material = new Material(shader);
        m_material.hideFlags = HideFlags.HideAndDontSave;
        // Turn on alpha blending
        m_material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        m_material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        // Turn backface culling off
        m_material.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        // Turn off depth writes
        m_material.SetInt("_ZWrite", 0);

        CalculateTriangles();
    }

    //We should implicitly destroy the material since it isn't destroyed by the garbage collection
    private void OnDestroy()
    {
        Destroy(m_material);
    }

    //This function is called automatically by the renderer before rendering anything
    void OnPreRender()
    {
        if (m_activeWireframe)
            GL.wireframe = true;
    }

    //This will be called once per frame. Here we take get input and compute fps
    void Update()
    {
        m_averageDeltaTime += (Time.deltaTime - m_averageDeltaTime);

        if (Input.GetKeyDown("f12"))
            m_debugMode = !m_debugMode;

        if (m_debugMode)
        {
            if (Input.GetKeyDown("f1"))
                m_activePerformance = !m_activePerformance;
            if (Input.GetKeyDown("f2"))
                m_activeCoordinates = !m_activeCoordinates;
            if (Input.GetKeyDown("f3"))
                m_activeGravityCoord = !m_activeGravityCoord;
            if (Input.GetKeyDown("f4"))
                m_activeWireframe = !m_activeWireframe;
            if (Input.GetKeyDown("f5"))
                m_activeDrawAxis = !m_activeDrawAxis;
        }

    }

    //This function would be called automatically each time there is event. It may result in more than once per frame (no logic here!).
    //Used to draw to the Debug options
    void OnGUI()
    {
        if (m_debugMode)
        {
            GUI.Label(m_rectDebugMenu, m_titleDebugMenu, m_styleDebugMenu);
            m_stylePerformanceMenu.normal.textColor = m_activePerformance ? m_colorActive : m_colorInactive;
            GUI.Label(m_rectPerformanceMenu, m_titlePerformanceMenu, m_stylePerformanceMenu);
            m_styleCoordinatesMenu.normal.textColor = m_activeCoordinates ? m_colorActive : m_colorInactive;
            GUI.Label(m_rectCoordinatesMenu, m_titleCoordinatesMenu, m_styleCoordinatesMenu);
            m_styleGravityMenu.normal.textColor = m_activeGravityCoord ? m_colorActive : m_colorInactive;
            GUI.Label(m_rectGravityMenu, m_titleGravityMenu, m_styleGravityMenu);
            m_styleWireframeMenu.normal.textColor = m_activeWireframe ? m_colorActive : m_colorInactive;
            GUI.Label(m_rectWireframeMenu, m_titleWireframeMenu, m_styleWireframeMenu);
            m_styleDrawAxisMenu.normal.textColor = m_activeDrawAxis ? m_colorActive : m_colorInactive;
            GUI.Label(m_rectDrawAxisMenu, m_titleDrawAxisMenu, m_styleDrawAxisMenu);
        }

        if (m_activePerformance)
        {
            float fps = 1.0f / m_averageDeltaTime;
            string textPerformance = string.Format("FPS: {0:0.00}. Triangles: {1:0.}", fps, m_numTris);
            GUI.Label(m_rectPerformanceInfo, textPerformance, m_stylePerformanceInfo);
        }

        if (m_activeCoordinates)
        {
            string textInfo = string.Format("X: {0:0.00}, Y: {1:0.00}, Z: {2:0.00}", m_player.transform.position.x, m_player.transform.position.y, m_player.transform.position.z);
            GUI.Label(m_rectCoordinatesInfo, textInfo, m_styleCoordinatesInfo);
        }

        if (m_activeGravityCoord)
        {
            string textGravity = string.Format("X: {0:0.00}, Y: {1:0.00}, Z: {2:0.00}", m_playerGravity.m_gravity.x, m_playerGravity.m_gravity.y, m_playerGravity.m_gravity.z);
            GUI.Label(m_rectGravityInfo, textGravity, m_styleGravityInfo);
        }

    }

    //This function is called automatically by the renderer after everything has been send to render
    void OnPostRender()
    {
        if (m_activeDrawAxis)
        {
            m_material.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(m_player.transform.position);
            GL.Vertex(m_player.transform.position + Vector3.right * m_axisLength);
            GL.Color(Color.green);
            GL.Vertex(m_player.transform.position);
            GL.Vertex(m_player.transform.position + Vector3.up * m_axisLength);
            GL.Color(Color.blue);
            GL.Vertex(m_player.transform.position);
            GL.Vertex(m_player.transform.position + Vector3.forward * m_axisLength);
            GL.End();
        }

        GL.wireframe = false;
    }

    //This function calculates the total number of triangles in the meshes of the scene.
    //Better not to call it at every interval due to the computational cost.
    private void CalculateTriangles()
    {
        m_numTris = 0;

        foreach (MeshFilter meshFilter in FindObjectsOfType(typeof(MeshFilter)))
        {
            m_numTris += (ulong) meshFilter.sharedMesh.triangles.Length / 3;
        }
    }
}
