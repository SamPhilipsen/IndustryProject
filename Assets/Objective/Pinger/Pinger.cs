using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinger : MonoBehaviour, IObjective
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject pingerObject;
    [SerializeField] ParticleSystem confettiParticles;
    [SerializeField] ParticleSystem pingerParticles;
    [SerializeField] Animator animator;

    bool completed = false;

    private void OnTriggerEnter(Collider other)
    {
        // Will check if pinger has already been placed
        if (completed)
        {
            return;
        }

        // If not, continue to check for collision
        if (other.Equals(player.GetComponent<Collider>()))
        {
            Complete();
        }
    }

    public void Complete()
    {
        pingerObject.SetActive(true);
        completed = true;
        SpawnParticles();
    }

    public void Disable()
    {
        ///Debug.Log("Disabled");
        gameObject.SetActive(false);
        pingerObject.SetActive(false);
        completed = false;
    }

    public void SpawnParticles()
    {
        Instantiate(confettiParticles, gameObject.transform);
        Instantiate(pingerParticles, gameObject.transform);
    }

    public void Reset()
    {
        
    }
}
    