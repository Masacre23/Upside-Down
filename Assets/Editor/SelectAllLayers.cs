using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SelectAllLayers : ScriptableWizard
{

    public string m_searchLayer = "Your layer here";
    public bool m_searchOnlyOnChildren = false;

    [MenuItem("Additional Tools/Select All Of Layer...")]
    static void SelectAllOfTagWizard()
    {
        ScriptableWizard.DisplayWizard<SelectAllLayers>("Select All Of Layer...", "Make Selection");
    }

    void OnWizardCreate()
    {
        GameObject selected = Selection.activeGameObject;
        GameObject[] gameObjects;

        int layerId = LayerMask.NameToLayer(m_searchLayer);

        if (m_searchOnlyOnChildren && selected)
        {
            gameObjects = FindGameObjectsWithLayer(layerId);
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
            gameObjects = FindGameObjectsWithLayer(layerId);
        }

        Selection.objects = gameObjects;
    }

    GameObject[] FindGameObjectsWithLayer(int layer)
    {
        GameObject[] found = GameObject.FindObjectsOfType<GameObject>();

        for (int i = 0; i < found.Length; i++)
        {
            if (found[i].layer != layer)
            {
                found.SetValue(null, i);
            }
        }

        return found;
    }
}
