using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(CustomHandle))]

public class HandleEditor : Editor
{

    CustomHandle targetObject;


    private void OnEnable()
    {
        targetObject = target as CustomHandle;
    }
    private void OnSceneGUI()
    {

        CustomHandle customHandle = target as CustomHandle;
        if(Event.current.type == EventType.Repaint)
        {
            Handles.color = Handles.zAxisColor;

            CreateHandleCap(11, customHandle.transform.position + customHandle.transform.forward * customHandle.handleOffsets.offset,
                                customHandle.transform.rotation * Quaternion.LookRotation(Vector3.forward),
                                customHandle.handleOffsets.size, EventType.Repaint);

            Handles.color = Handles.xAxisColor;

            CreateHandleCap(11, customHandle.transform.position + customHandle.transform.right * customHandle.handleOffsets.offset,
                                customHandle.transform.rotation * Quaternion.LookRotation(Vector3.right),
                                customHandle.handleOffsets.size, EventType.Repaint);

            Handles.color = Handles.yAxisColor;

            CreateHandleCap(11, customHandle.transform.position + customHandle.transform.up * customHandle.handleOffsets.offset,
                                customHandle.transform.rotation * Quaternion.LookRotation(Vector3.up),
                                customHandle.handleOffsets.size, EventType.Repaint);
        }
    }

    void CreateHandleCap(int id, Vector3 position, Quaternion rotation, float size, EventType eventType)
    {
        Handles.ArrowHandleCap(id, position, rotation, size, eventType);
    }
}
