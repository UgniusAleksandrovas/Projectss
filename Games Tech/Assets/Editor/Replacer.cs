using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Replacer : EditorWindow
{

    int currentSelectionCount = 0;
    GameObject wantedObject;


    [MenuItem("Window/Replacer")]
    public static void ShowWindow()
    {
        GetWindow<Replacer>("Replacer");
    }

    private void OnGUI()
    {

        GetSelection();

        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Selection Count:" + currentSelectionCount.ToString(), EditorStyles.boldLabel);
        EditorGUILayout.Space();

        wantedObject = (GameObject)EditorGUILayout.ObjectField("Replace Object: ", wantedObject, typeof(GameObject), true);

        if (GUILayout.Button("Replace Selected Objects", GUILayout.ExpandWidth(true), GUILayout.Height(40)))
        {
            ReplaceSelectedObjects();
        }
 

        EditorGUILayout.Space();
        EditorGUILayout.EndVertical();

        Repaint();
    }

    void GetSelection()
    {
        currentSelectionCount = 0;
        currentSelectionCount = Selection.gameObjects.Length;
    }

    void ReplaceSelectedObjects()
    {

        if(currentSelectionCount == 0)
        {
            CustomDialogue("Atleast one objects needs to be selected");
            return;
        }

        if (!wantedObject)
        {
            CustomDialogue("The replace Object is empty");
            return;
        }

        GameObject[] selectedObjects = Selection.gameObjects;
        for( int i = 0; i < selectedObjects.Length; i++)
        {
            Transform selectTransform = selectedObjects[i].transform;
            GameObject newObject = Instantiate(wantedObject, selectTransform.position, selectTransform.rotation);
            newObject.transform.localScale = selectTransform.localScale;

            DestroyImmediate(selectedObjects[i]);
        }

    }

    void CustomDialogue(string aMessage)
    {
        EditorUtility.DisplayDialog("Replace Objects Warning", aMessage, "OK");
    }
}
