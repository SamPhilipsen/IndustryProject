using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMover : MonoBehaviour
{
    [SerializeField] int speed = 1;
    [SerializeField] public bool activateArrow;
    float incrementer;
    Renderer rend;
    Vector3 startPosition;

    private void Start()
    {
        incrementer = 0;
        rend = GetComponent<Renderer>();
        startPosition = transform.position;
        rend.material.color = Color.gray;
        rend.material.SetColor("_EmissionColor", Color.black);

    }
    private void Update()
    {

        if(activateArrow)
        {
            rend.material.color = Color.red;
            rend.material.SetColor("_EmissionColor", Color.red);
            incrementer = Mathf.PingPong(Time.time * speed, 1) * 6 - 3;
            gameObject.transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y + incrementer, transform.position.z), Time.deltaTime * 3);
        } else
        {
            transform.position = startPosition;
            incrementer = 0;
            rend.material.color = Color.black;
            rend.material.SetColor("_EmissionColor", Color.black);
        }
    }
}