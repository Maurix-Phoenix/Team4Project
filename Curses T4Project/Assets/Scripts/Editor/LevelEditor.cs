//LevelEditor.cs
//by MAURIZIO FISCHETTI

using UnityEditor;
using UnityEngine;
using static T4P;
public class LevelEditor
{
    [MenuItem("Level Editor/New Level")]
    static void NewLevel()
    {
        GameObject newLevel = new GameObject("Level");
        newLevel.tag = "Level";
        newLevel.transform.position = Vector3.zero;
        newLevel.AddComponent<Level>();
    }


    [MenuItem("Level Editor/Save Level")]
    static void SaveLevel()
    {
        GameObject[] levelObjects = Selection.gameObjects;

        foreach(GameObject lo in levelObjects)
        {
            Level level = Selection.activeGameObject.GetComponent<Level>();

            lo.name = $"{level.LevelID}-{level.LevelName}-{level.LevelDesigner}";
            string path = $"Assets/Resources/Levels/{lo.name}.prefab";
            //path = AssetDatabase.GenerateUniqueAssetPath(path);
            if(PrefabUtility.SaveAsPrefabAssetAndConnect(lo, path, InteractionMode.UserAction, out bool saved))
            {
                if (saved == true)
                    T4Debug.Log("[Level Editor] Level Saved");
                else
                    T4Debug.Log($"[Level Editor] Cannot save {saved}");
            }
        }
    }

    [MenuItem("Level Editor/Save Level", true)]
    static bool ValidateSaveLevel()
    {
        return Selection.activeGameObject != null && !EditorUtility.IsPersistent(Selection.activeGameObject) && Selection.activeGameObject.tag == "Level";
    }

}
