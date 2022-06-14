using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ObjectiveInfo
{
    public GameObject gameObject;
    public bool isCompleted;
}

[System.Serializable]
public class TriggerInfo
{
    public bool mustBeConsecutiveToTrigger;
    public int[] triggerIndexes;
    public UnityEvent triggerEvents;
}

[System.Serializable]
public class Group
{
    public string name;
    public List<ObjectiveInfo> objectives;
    [Header("Triggers")]
    public bool isEnabled;
    public List<TriggerInfo> triggers;
}

public class ObjectiveGroup : MonoBehaviour
{
    public List<Group> objectiveGroups;

    public void ResetObjective(GameObject obj)
    {
        foreach (Group group in objectiveGroups)
        {
            foreach (ObjectiveInfo oInfo in group.objectives)
            {
                if (oInfo.gameObject.Equals(obj))
                {
                    oInfo.isCompleted = false;
                    oInfo.gameObject.GetComponent<IObjective>().Reset();
                }
            }

        }
    }

    public void ResetObjectives()
    {
        foreach (Group group in objectiveGroups)
        {
            foreach (ObjectiveInfo oInfo in group.objectives)
            {
                oInfo.isCompleted = false;
                IObjective objective = oInfo.gameObject.GetComponent<IObjective>() != null ? oInfo.gameObject.GetComponent<IObjective>() : oInfo.gameObject.GetComponentInChildren<IObjective>(true);
                objective.Reset();
            }
        }
    }

    public void ExecuteTrigger(GameObject obj)
    {
        // Checking if the gameObject that is triggered is part of an objective group
        List<Group> groupsObjectiveIsPartOf = new List<Group>();
        foreach (Group group in objectiveGroups)
        {
            foreach (ObjectiveInfo oInfo in group.objectives)
            {
                if (oInfo.gameObject.Equals(obj))
                {
                    groupsObjectiveIsPartOf.Add(group);
                    oInfo.isCompleted = true;
                }
            }
            
        }

        // Guard clause for triggers and adding all groups with triggers to a list
        bool hasTrigger = false;
        List<Group> groupsWithTriggers = new List<Group>();
        foreach (Group group in groupsObjectiveIsPartOf)
        {
            if (group.isEnabled)
            {
                hasTrigger = true;
                groupsWithTriggers.Add(group);
            }
        }
        if (!hasTrigger || groupsWithTriggers.Count == 0)
        {
            return;
        }

        // Execute triggers in each group with triggers
        foreach(Group group in groupsWithTriggers)
        {
            // Check for every trigger in the group
            for (int i = 0; i < group.triggers.Count; i++)
            {
                TriggerInfo tInfo = group.triggers[i];
                // Search for the completed gameObject in all triggers
                for (int j = 0; j < tInfo.triggerIndexes.Length; j++)
                {
                    // Find index of gameobject in the objectives list
                    /// PUT THIS AFTER LINE 96 (AFTER DEBUG) TO PREVENT UNNECESSARY CHECKING LATER
                    /// KEEP IT HERE DURING DEVELOPMENT
                    int indexOfGameobject = -1;
                    foreach (ObjectiveInfo oInfo in group.objectives)
                    {
                        if (oInfo.gameObject.Equals(obj))
                        {
                            indexOfGameobject = group.objectives.IndexOf(oInfo);
                            break;
                        }
                    }
                    // If index wasn't found, abort
                    if (indexOfGameobject == -1)
                    {
                        continue;
                    }

                    // Check if the object that is hit is the one that should do the trigger
                    if (!obj.Equals(group.objectives[tInfo.triggerIndexes[j]].gameObject))
                    {
                        Debug.Log($"{indexOfGameobject} is not a trigger of {group.name} of trigger {i}");
                        continue;
                    }
                    Debug.Log($"{indexOfGameobject} is a trigger of {group.name} of trigger {i}");
                    // Check if it must be hit in order
                    ///Debug.Log(tInfo.mustBeConsecutiveToTrigger);
                    if (tInfo.mustBeConsecutiveToTrigger && tInfo.triggerIndexes.Length > 0)
                    {
                        // Check if all objectives with an index lower of gameobjects are hit in order
                        bool isConsecutive = true;
                        for(int k = 0; k < indexOfGameobject; k++)
                        {
                            ObjectiveInfo oInfo = group.objectives[k];
                            string status = oInfo.isCompleted ? "Completed" : "Not completed";
                            ///Debug.Log($"{group.objectives.IndexOf(oInfo)} was {status}");
                            if (!oInfo.isCompleted)
                            {
                                isConsecutive = false;
                            }
                        }
                        if (isConsecutive)
                        {
                            // Trigger all events listed in the trigger
                            tInfo.triggerEvents.Invoke();
                            break;
                        }
                    }
                    else
                    {
                        // Trigger all events listed in the trigger
                        tInfo.triggerEvents.Invoke();
                        break;
                    }
                }
            }
        }
    }
}
/*
[CustomEditor(typeof(ObjectiveGroup))]
public class ObjectiveGroupGUIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ObjectiveGroup group = (ObjectiveGroup)target;
        if (GUILayout.Button("Reset Objectives"))
        {
            group.ResetObjectives();
        }
    }
}
*/