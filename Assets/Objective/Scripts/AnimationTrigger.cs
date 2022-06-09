using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    [SerializeField] Animator frontAnimator;
    [SerializeField] Animator backAnimator;

    void Awake()
    {
        //frontAnimator.Play("FrontCrash");
        //backAnimator.Play("BackCrash");
    }
}
