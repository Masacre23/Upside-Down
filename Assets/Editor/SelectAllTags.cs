using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SelectAllTags : ScriptableWizard
{
    public string m_searchTag = "Your tag here";
    public bool m_searchOnlyOnChildren = false; 

    [MenuItem("Additional Tools/Select All Of Tag...")]
    static void SelectAllOfTagWizard()
    {
        ScriptableWizard.DisplayWizard<SelectAllTags>("Select All Of Tag...", "Make Selection");
    }

    void OnWizardCreate()
    {
        GameObject selected = Selection.activeGameObject;
        GameObject[] gameObjects;

        if (m_searchOnlyOnChildren && selected)
        {
            gameObjects = GameObject.FindGameObjectsWithTag(m_searchTag);
            for (int i = 0; i < gameObjects.Length; i++)
            {
                if (gameObjects[i].transform.parent != selected.transform)
                {
                    gameObjects.SetValue(null, i);
                }
            }
        }
        else
        {
            gameObjects = GameObject.FindGameObjectsWithTag(m_searchTag);
        }

        Selection.objects = gameObjects;
    }
}
