using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempController : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] Camera Camera;
    Rigidbody rb;
    Vector2 turn;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        turn.x += Input.GetAxis("Mouse X") * 2f;
        turn.y += Input.GetAxis("Mouse Y") * 2f;

        transform.localRotation = Quaternion.Euler(0, turn.x, 0);

        //Camera.transform.RotateAround(transform.position, Vector3.forward, Quaternion.Euler(-turn.y, 0, 0).eulerAngles.y);

        Vector3 movement = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * movementSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * movementSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * movementSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * movementSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            movement += Vector3.up;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movement += Vector3.down;
        }
        rb.velocity = movement * movementSpeed;
    }
}
