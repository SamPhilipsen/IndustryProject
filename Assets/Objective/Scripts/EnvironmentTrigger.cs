using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentTrigger : MonoBehaviour, IObjective
{
    [SerializeField] List<Animator> animators;

    public void Complete()
    {
        
    }

    public void Disable()
    {
        
    }

    public void Reset()
    {
        foreach(Animator animator in animators)
        {
            animator.Rebind();
            animator.Update(0f);
        }
    }

    public void SpawnParticles()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(7))
        {
            Triggerable triggerable = GetComponent<Triggerable>();
            if (triggerable != null)
            {
                triggerable.Trigger();
            }
        }
    }
}
