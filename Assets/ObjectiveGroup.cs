using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ObjectiveInfo
{
    public GameObject gameObject;
    public bool isTriggered;
}

public class ObjectiveGroup : MonoBehaviour
{
    [Space]
    public List<ObjectiveInfo> objectives;
    [Header("Trigger Settings")]
    public bool hasTrigger;
    public int[] triggerIndexes;    
    [Space]
    public UnityEvent triggerEvents;
    [Tooltip("")]
    public bool mustBeConsecutive;

    public void ExecuteTrigger(GameObject obj)
    {
        if (!hasTrigger)
        {
            return;
        }

        if (objectives.Count <= 0)
        {
            return;
        }

        ObjectiveInfo match = null;
        foreach(ObjectiveInfo info in objectives)
        {
            if (obj.Equals(info.gameObject))
            {
                match = info;
            }
        }
        Debug.Log(match);
        match.isTriggered = true;

        for(int i = 0; i < triggerIndexes.Length; i++)
        {
            if (obj.Equals(objectives[triggerIndexes[i]].gameObject))
            {
                if (mustBeConsecutive)
                {
                    bool isConsecutive = true;
                    foreach (ObjectiveInfo info in objectives)
                    {
                        if (!info.isTriggered)
                        {
                            isConsecutive = false;
                        }
                    }
                    if (isConsecutive)
                    {
                        triggerEvents.Invoke();
                    }
                }
                else
                {

                    triggerEvents.Invoke();
                }
            }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ObjectiveGroup))]
public class ObjectiveGroupInspectorEditor : Editor
{
    SerializedProperty objectives;
    SerializedProperty triggerOnAmount;

    void OnEnable()
    {
        // Fetch the objects from the GameObject script to display in the inspector
        objectives = serializedObject.FindProperty("objectives");
        triggerOnAmount = serializedObject.FindProperty("triggerIndexes");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        bool showWarning = false;
        for(int i = 0; i < triggerOnAmount.arraySize; i++)
        {
            if (triggerOnAmount.GetArrayElementAtIndex(i).intValue > objectives.arraySize - 1)
            {
                showWarning = true;
            }
        }
        if (showWarning)
        {
            GUIStyle style = new(EditorStyles.textField);
            style.normal.textColor = Color.red;
            GUILayout.Label("This index doesn't exist in the objectives list.", style);
        }
    }
}
#endif