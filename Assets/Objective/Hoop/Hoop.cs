using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoop : MonoBehaviour, IObjective
{
    [SerializeField] GameObject player;
    [SerializeField] ParticleSystem particles;

    private void OnTriggerEnter(Collider other)
    {
        if (other.Equals(player.GetComponent<Collider>()))
        {
            Complete();
        }
    }

    public void Complete()
    {
        Debug.Log(gameObject.name + " completed");
        //particles.Play();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
