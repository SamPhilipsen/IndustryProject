using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public void IsPressed()
    {
        PlayerPrefs.Save();
    }
}
