using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggerable : MonoBehaviour
{
    public void Trigger()
    {
        ObjectiveGroup group = GetComponentInParent<ObjectiveGroup>();
        if (group != null)
        {
            group.ExecuteTrigger(gameObject);
        }
    }
    public void Reset(GameObject obj)
    {
        ObjectiveGroup group = GetComponentInParent<ObjectiveGroup>();
        if (group != null)
        {
            group.ResetObjective(obj);
        }
    }
}
