using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoop : MonoBehaviour, IObjective
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject particles;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Complete();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Complete();
        }
    }

    public void Complete()
    {
        Debug.Log(gameObject.name + " completed");
        SpawnParticles();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void SpawnParticles()
    {
        Instantiate(particles, gameObject.transform.parent);
    }
}
