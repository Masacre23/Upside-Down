using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SelectAllComponents : ScriptableWizard
{
    public string m_searchComponent = "Your component here";
    public bool m_searchOnlyOnChildren = false;

    [MenuItem("Additional Tools/Select All Of Component...")]
    static void SelectAllOfTagWizard()
    {
        ScriptableWizard.DisplayWizard<SelectAllComponents>("Select All Of Component...", "Make Selection");
    }

    void OnWizardCreate()
    {
        GameObject selected = Selection.activeGameObject;
        GameObject[] gameObjects;

        if (m_searchOnlyOnChildren && selected)
        {
            gameObjects = FindGameObjectsWithComponent(m_searchComponent);
            for (int i = 0; i < gameObjects.Length; i++)
            {
                if (gameObjects[i] != null && gameObjects[i].transform.parent != selected.transform)
                {
                    gameObjects.SetValue(null, i);
                }
            }
        }
        else
        {
            gameObjects = FindGameObjectsWithComponent(m_searchComponent);
        }

        Selection.objects = gameObjects;
    }

    GameObject[] FindGameObjectsWithComponent(string component)
    {
        GameObject[] found = GameObject.FindObjectsOfType<GameObject>();

        for (int i = 0; i < found.Length; i++)
        {
            if (!found[i].GetComponent(component))
            {
                found.SetValue(null, i);
            }
        }

        return found;
    }
}
