using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float DestroyAfterSeconds;

    void Start()
    {
        StartCoroutine(BeginCountdown());
    }

    IEnumerator BeginCountdown()
    {
        yield return new WaitForSeconds(DestroyAfterSeconds);
        Destroy(gameObject);
    }
}
