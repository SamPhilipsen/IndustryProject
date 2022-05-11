using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjective
{
    /// <summary>
    /// Executes whenever the player hits the objective collider
    /// </summary>
    void Complete();

    /// <summary>
    /// Can be used in animations or triggers to disable the objective
    /// </summary>
    void Disable();
}
