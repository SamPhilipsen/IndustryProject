using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour, IScore,IInteractable
{
    [SerializeField]
    ParticleSystem HitParticles;
    [SerializeField]
    Material DefaultMaterial;
    [SerializeField]
    Material ClearedMaterial;

    [SerializeField]
    [Range(0,45)]
    float ResetDelay = 5f;

    const string PlayerTag = "Player";

    void Start()
    {
        DefaultMaterial = GetComponent<MeshRenderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerTag))
        {
            Trigger();
        }
    }

    public int Score()
    {
        return 5;
    }
    private void SpawnParticles()
    {
        ParticleSystem system = Instantiate(HitParticles, gameObject.transform);
        system.Play();
        Destroy(system, ResetDelay);
    }
    public void Trigger()
    {
        ScoreManager.Add(Score());
        GetComponent<AudioSource>().Play();
        SpawnParticles();
        StartCoroutine(PrepareReset());
    }
    public IEnumerator PrepareReset()
    {
            yield return new WaitForSeconds(ResetDelay);
            Reset();
    }

    public void Reset()
    {
        animator.Play(IdleAnimation);
    }
}
