using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevCameraMoverScript : MonoBehaviour
{
    public Transform targetObject;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private void FixedUpdate()
    {
        Vector3 desiredPosition = targetObject.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(targetObject);
    }
}
