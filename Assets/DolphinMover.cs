using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DolphinMover : MonoBehaviour
{
    public Transform leftPos;
    public Transform rightPos;
    public Transform basePos;

    public float speed;
    private Transform target;

    public CinemachineDollyCart cart;

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            target = leftPos;
        }

        else if (Input.GetKey(KeyCode.D))
        {
            target = rightPos;
        }

        else
        {
            target = basePos;
        }

        if (Input.GetKey(KeyCode.W))
        {
            cart.m_Speed = 30f;
        }
        else if (Input.GetKey(KeyCode.S))
        {

            cart.m_Speed = -20f;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            cart.m_Speed = 0f;
        }
        else
        {
            cart.m_Speed = 15f;
        }

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}
