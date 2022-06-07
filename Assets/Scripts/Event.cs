using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Animator animator;

    [SerializeField] private float positionTrack;
    [SerializeField] private int triggerAmount = 5;
    [SerializeField] private List<string> animationNames = new List<string>();

    private float startArea = 15;
    private int totalEncounter = 0;
    private bool passedPosition = false;

    private void Update()
    {
        CheckPlayerPosition();
    }

    private void CheckPlayerPosition()
    {
        if (passedPosition == false)
        {
            if (positionTrack < player.GetComponent<Cinemachine.CinemachineDollyCart>().m_Position)
            {
                passedPosition = true;
                totalEncounter++;
                ControlEncounters();
            }
        }
        else if (startArea  > player.GetComponent<Cinemachine.CinemachineDollyCart>().m_Position)
        {
            passedPosition = false;
        }
        
    }

    private void ControlEncounters()
    {
        if (totalEncounter == triggerAmount)
        {
            //PlayAnimation();
            totalEncounter = 0;
        }
    }

    private void PlayAnimation()
    {
        foreach (string animationName in animationNames)
        {
            animator.Play(animationName, 0, 0.0f);
        }
    }
}
