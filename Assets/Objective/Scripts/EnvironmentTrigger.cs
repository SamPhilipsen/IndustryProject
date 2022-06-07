using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentTrigger : MonoBehaviour
{
    [SerializeField] GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.Equals(player.GetComponent<Collider>()))
        {
            Triggerable triggerable = GetComponent<Triggerable>();
            if (triggerable != null)
            {
                triggerable.Trigger();
            }
        }
    }
}
