using UnityEngine;
using UnityEditor;

public class ReplaceGameObjects : ScriptableWizard
{
	public GameObject Prefab;
	public GameObject[] ObjectsToReplace;
	public bool KeepOriginalNames = true;
	[MenuItem("Additional Tools/Replace GameObjects")]
	static void CreateWizard()
	{
		var replaceGameObjects = DisplayWizard<ReplaceGameObjects>("Replace GameObjects", "Replace");
		replaceGameObjects.ObjectsToReplace = Selection.gameObjects;
	}
	void OnWizardCreate()
	{
		foreach (GameObject go in ObjectsToReplace)
		{
			GameObject newObject;
			newObject = (GameObject)PrefabUtility.InstantiatePrefab(Prefab);
			newObject.transform.SetParent(go.transform.parent, true);
			newObject.transform.localPosition = go.transform.localPosition;
			newObject.transform.localRotation = go.transform.localRotation;
			newObject.transform.localScale = go.transform.localScale;
			if (KeepOriginalNames)
				newObject.transform.name = go.transform.name;
			DestroyImmediate(go);
		}
	}
}