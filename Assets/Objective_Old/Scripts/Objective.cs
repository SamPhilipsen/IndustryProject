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
    /// Spawn confetti particles at object position
    /// </summary>
    void SpawnParticles();

    /// <summary>
    /// Can be used in animations or triggers to disable the objective
    /// </summary>
    void Disable();

    /// <summary>
    /// Reset the objective to its original state
    /// </summary>
    void Reset();
}
