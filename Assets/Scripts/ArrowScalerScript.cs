using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScalerScript : MonoBehaviour
{
    public AnimationCurve heightGraph;
    public Transform playerPos;

    private float dist;

    void Update()
    {
        CalculatePlayerDistance();
    }

    private void CalculatePlayerDistance()
    {
        dist = Vector3.Distance(transform.position, playerPos.position);

        float size = heightGraph.Evaluate(dist * 0.01f);
        transform.localScale = new Vector3(size * 50, size * 50, size * 50);
    }
}