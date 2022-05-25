using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class TerrainGenerator : EditorWindow
{
    public int prefabCount = 1;
    public List<GameObject> prefabs = new List<GameObject>();
    public List<GameObject> SpawnedObjects;

    float minScaleVal = 1f;
    float maxScaleVal = 2f;
    float minScaleLimit = 0.5f;
    float maxScaleLimit = 3f;

    GameObject obj;

    Transform CreatedPrefabs;

    public Vector3 spawnPos;

    [MenuItem("Window/TerrainGenerator %g",false,-1)]
    public static void ShowWindow()
    {
        GetWindow<TerrainGenerator>("TerrainGenerator");
    }

    void OnGUI()
    {
        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty stringsProperty = so.FindProperty("prefabs");


        EditorGUILayout.PropertyField(stringsProperty, true);
        so.ApplyModifiedProperties();

        EditorGUILayout.Space();

        CreatedPrefabs = EditorGUILayout.ObjectField("CreatedPrefabs", CreatedPrefabs, typeof(Transform), true) as Transform;


        GUILayout.Label("Scale Object");

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Min Limit: " + minScaleLimit);
        EditorGUILayout.MinMaxSlider(ref minScaleVal, ref maxScaleVal, minScaleLimit, maxScaleLimit);
        EditorGUILayout.PrefixLabel("Max Limit: " + maxScaleLimit);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Min Value: " + minScaleVal.ToString());
        EditorGUILayout.LabelField("Max Value: " + maxScaleVal.ToString());
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Generate"))
        {
            for (int i = 0; i < prefabCount; i++)
            {
                for (int x = 0; x < prefabs.Count; x++)
                {
                    spawnPos = new Vector3(Random.Range(42, -27), 0, Random.Range(-22, 21));
                    GameObject newObject = Instantiate(prefabs[x], spawnPos, Quaternion.identity, CreatedPrefabs);
                    SpawnedObjects.Add(newObject);
                    newObject.transform.localScale = Vector3.one * Random.Range(minScaleVal, maxScaleVal);

                }
            }
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();


        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Remove"))
        {
            foreach (GameObject obj in SpawnedObjects)
            {
                if(Random.value < 0.2)
                {
                    DestroyImmediate(obj);
                }
            }
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }


}
 