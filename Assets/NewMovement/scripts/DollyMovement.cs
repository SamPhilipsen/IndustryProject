using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollyMovement : MonoBehaviour
{
  

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * 10 * Time.deltaTime;
    }
}
