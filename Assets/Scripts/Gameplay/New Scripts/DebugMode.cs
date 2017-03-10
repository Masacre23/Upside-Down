using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMode : MonoBehaviour {

    bool m_debugMode = false;

    void OnPreRender()
    {
        if (m_debugMode)
            GL.wireframe = true;
    }

    void Update()
    {
        if (Input.GetKeyDown("f12"))
            m_debugMode = !m_debugMode;
    }

    void OnPostRender()
    {
        GL.wireframe = false;
    }
}
