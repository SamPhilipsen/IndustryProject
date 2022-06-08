using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour, IObjective
{
    //[SerializeField] GameObject player;
    [SerializeField] ParticleSystem particles;
    [SerializeField] Animator animator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(7))
        {
            Complete();
            Triggerable triggerable = GetComponentInParent<Triggerable>();
            if (triggerable != null)
            {
                triggerable.Trigger();
            }
        }
    }

    public void Complete()
    {
        animator.Play("Collect");
        SpawnParticles();
    }

    public void Disable()
    {
        ///Debug.Log("Disabled");
        gameObject.SetActive(false);
    }

    public void SpawnParticles()
    {
        Instantiate(particles, gameObject.transform.parent);
    }

    public void Reset()
    {
        gameObject.SetActive(true);
        animator.Play("Idle");
        Triggerable triggerable = GetComponentInParent<Triggerable>();
        if (triggerable != null)
        {
            triggerable.Reset(gameObject);
        }
    }
}
