using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTdollyfollower : MonoBehaviour
{
    public Transform dolly;

    void Update()
    {
        transform.Translate(dolly.position * 0.0001f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
    }
}
