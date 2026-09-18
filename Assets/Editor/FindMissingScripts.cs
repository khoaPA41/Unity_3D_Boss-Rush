using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class FindMissingScripts : EditorWindow
{
    [MenuItem("Tools/Find Missing Scripts")]
    public static void ShowWindow()
    {
        GetWindow<FindMissingScripts>("Find Missing Scripts");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Find Missing Scripts in current Scene", GUILayout.Height(30)))
        {
            FindInCurrentScene();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Find Missing Scripts in all Prefabs", GUILayout.Height(30)))
        {
            FindInAllPrefabs();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Find Missing Scripts in all Scene + Prefab", GUILayout.Height(40)))
        {
            FindInAllScenesAndPrefabs();
        }
    }

    static void FindInCurrentScene()
    {
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        int count = 0;

        foreach (GameObject go in allObjects)
        {
            count += CheckGameObject(go);
        }

        Debug.Log($"<color=cyan>Success!</color> Found <b>{count}</b> Missing Script in current Scene.");
    }

    static void FindInAllPrefabs()
    {
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab");
        int count = 0;

        foreach (string guid in prefabGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab != null)
            {
                count += CheckGameObject(prefab, path);
            }
        }

        Debug.Log($"<color=cyan>Success!</color> Found <b>{count}</b> Missing Script in all Prefab.");
    }

    static void FindInAllScenesAndPrefabs()
    {
        // 1. Prefabs
        FindInAllPrefabs();

        // 2. Tất cả Scene
        string[] sceneGuids = AssetDatabase.FindAssets("t:Scene");
        int totalCount = 0;

        foreach (string guid in sceneGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var scene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene(path, UnityEditor.SceneManagement.OpenSceneMode.Additive);

            GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            int sceneCount = 0;

            foreach (GameObject go in allObjects)
            {
                sceneCount += CheckGameObject(go, path);
            }

            totalCount += sceneCount;
            UnityEditor.SceneManagement.EditorSceneManager.CloseScene(scene, true);
        }

        Debug.Log($"<color=green><b>COMPLETE!</b></color> A total of Missing Scripts were found throughout the whole project.");
    }

    static int CheckGameObject(GameObject go, string path = "")
    {
        int count = 0;
        Component[] components = go.GetComponents<Component>();

        for (int i = 0; i < components.Length; i++)
        {
            if (components[i] == null)
            {
                count++;
                string location = string.IsNullOrEmpty(path) ? go.name : $"{path} → {GetFullPath(go)}";
                Debug.LogWarning($"<color=yellow>Missing Script</color> at: <b>{location}</b>", go);
            }
        }

        // Kiểm tra cả children
        foreach (Transform child in go.transform)
        {
            count += CheckGameObject(child.gameObject, path);
        }

        return count;
    }

    static string GetFullPath(GameObject go)
    {
        string path = go.name;
        while (go.transform.parent != null)
        {
            go = go.transform.parent.gameObject;
            path = go.name + "/" + path;
        }
        return path;
    }
}
