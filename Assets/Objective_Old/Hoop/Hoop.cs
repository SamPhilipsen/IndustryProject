using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoop : MonoBehaviour, IObjective
{
    //[SerializeField] GameObject player;
    [SerializeField] ParticleSystem particles;
    [SerializeField] Material normalMaterial;
    [SerializeField] Material completeMaterial;

    private void Awake()
    {
        normalMaterial = GetComponent<MeshRenderer>().material;
    }

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
        Debug.Log(gameObject.name + " completed");
        GetComponent<MeshRenderer>().material = completeMaterial; 
        SpawnParticles();
        //particles.Play();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void SpawnParticles()
    {
        Instantiate(particles, gameObject.transform.parent);
    }

    public void Reset()
    {
        gameObject.SetActive(true);
        GetComponent<MeshRenderer>().material = normalMaterial;
        Triggerable triggerable = GetComponentInParent<Triggerable>();
        if (triggerable != null)
        {
            triggerable.Reset(gameObject);
        }
    }
}
