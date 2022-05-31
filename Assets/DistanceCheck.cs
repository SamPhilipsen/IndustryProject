using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCheck : MonoBehaviour
{
    [SerializeField] Transform pos1;
    [SerializeField] Transform pos2;
    [SerializeField] Transform posToAdd;

    public void Awake()
    {
        float distance1To2 = Mathf.Pow(Mathf.Pow(pos2.position.x - pos1.position.x, 2) + Mathf.Pow(pos2.position.y - pos1.position.y, 2) + Mathf.Pow(pos2.position.z - pos1.position.z, 2), 0.5f);
        float distance1To2Distance = Vector3.Distance(pos1.position, pos2.position);
        Debug.Log(distance1To2);
        Debug.Log(distance1To2Distance);
    }
}
