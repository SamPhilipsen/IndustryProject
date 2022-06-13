using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;

    public void PlayParticle()
    {
        particle.Play();
    }
}
