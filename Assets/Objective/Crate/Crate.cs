using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour, IObjective
{
    [SerializeField] GameObject player;
    [SerializeField] Animator animator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.Equals(player.GetComponent<Collider>()))
        {
            Complete();
        }
    }

    public void Complete()
    {
        animator.Play("Collect");
    }

    public void Disable()
    {
        Debug.Log("Disabled");
        gameObject.SetActive(false);
    }
}
