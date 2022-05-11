using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateAnimatorHandler : MonoBehaviour
{
    public void DisableChild()
    {
        GetComponentInChildren<Crate>().Disable();
    }
}
