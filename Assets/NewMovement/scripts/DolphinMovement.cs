using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DolphinMovement : MonoBehaviour
{
    [SerializeField] public float horizontalMovmentSpeed = 5;
    [SerializeField] public float verticalMovmentSpeed = 5;

    private float horizontalInput;
    private float verticalInput;

    // Update is called once per frame
    void Update()
    {
        GetInput();
        AvoidColisions();
        Movement();
    }

    public void GetInput()
    {

        //TODO check if ardunio is online
        if (false)
        {

        }
        else
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
        }
        
    }


    private void Movement()
    {
        //Add yaw pitch and roll to the dolphin
        float yaw = Mathf.Lerp(0, 20, Mathf.Abs(horizontalInput)) * Mathf.Sign(horizontalInput);
        float pitch = Mathf.Lerp(0, 20, Mathf.Abs(verticalInput)) * Mathf.Sign(verticalInput);
        float roll = Mathf.Lerp(0, 30, Mathf.Abs(horizontalInput)) * -Mathf.Sign(horizontalInput);
        transform.localRotation = Quaternion.Euler(Vector3.up * yaw + Vector3.right * pitch + Vector3.forward * roll);

        //Move the dolphin
        //transform.position += transform.up * verticalInput * Time.deltaTime;
        //transform.position += transform.right * horizontalInput * Time.deltaTime;
        transform.position += new Vector3(horizontalInput * Time.deltaTime * horizontalMovmentSpeed, (verticalInput * -1f) * Time.deltaTime * verticalMovmentSpeed, 0);
    }

    private void AvoidColisions()
    {
        
    }

    private void OnDrawGizmos()//used to see Ray in editor without update function
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 10, Color.red);
        Vector3 leftRay = Quaternion.AngleAxis(45, transform.up) * transform.forward;
        Vector3 rightRay = Quaternion.AngleAxis(-45, transform.up) * transform.forward;
        
        Debug.DrawRay(transform.position, leftRay * 10, Color.red);
        Debug.DrawRay(transform.position, rightRay * 10, Color.red);
    }
}
