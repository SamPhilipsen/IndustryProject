using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
public class Crate : MonoBehaviour, IScore, IInteractable
{
    [SerializeField]
    ParticleSystem IdleParticle;
    [SerializeField]
    Animator animator;

    [Header("Crate Settings")]
    [Range(0, 45)]
    [SerializeField]
    float RespawnDelay = 20f;

    private bool Triggered = false;

    private const string IdleAnimation = "Idle";
    private const string CollideAnimation = "Collect";

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play(IdleAnimation);
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Player")) // Make sure this works properly with whoever is working on Dolfy
        //{
        //    Trigger();
        //}
        Trigger();
    }

    public int Score()
    {
        return 20;
    }

    public void Trigger()
    {
        Triggered = true;

        ScoreManager.Add(Score());
        animator.Play(CollideAnimation);
        SpawnParticles();
        GetComponent<AudioSource>().Play();

        StartCoroutine(PrepareReset());
    }

    private void SpawnParticles()
    {
        ParticleSystem system = Instantiate(IdleParticle, gameObject.transform);
    }

    public IEnumerator PrepareReset()
    {
        if (Triggered)
        {
            Triggered = false;
            Reset();
            yield return new WaitForSeconds(RespawnDelay);
        }
    }

    public void Reset()
    {
        animator.Play(IdleAnimation);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(4, 4, 4));
    }

    [ContextMenu("Trigger")]
    void Act()
    {
        Trigger();
    }
}
