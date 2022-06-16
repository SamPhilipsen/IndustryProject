using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Area
{
    public areaNames name;
    [Header("Light Values")]
    public float lightIntensity;
    public Color lightColor;
    public Color fogColor;
}

public enum areaNames
{
    Beach, CoralReef, Cave, SunkenShip, KelpForest
}

public class AreaLightScript : MonoBehaviour
{
    public Light dirLight;
    public float lightChangeSpeed;

    private float timeElapsed = 0f;

    public List<Area> areas;

    public void ChangeLighting(areaNames areaName)
    {
        foreach (Area area in areas)
        {
            if(areaName == area.name)
            {
                dirLight.color = area.lightColor;
                dirLight.intensity = area.lightIntensity;
                RenderSettings.fogColor = area.fogColor;

                //StartCoroutine(LightRoutine(area.lightIntensity, area.lightColor));
            }
        }
    }

    //private IEnumerator LightRoutine(float lightIntensity, Color lightColor)
    //{
    //    while (timeElapsed < lightChangeSpeed)
    //    {
    //        dirLight.color = Color.Lerp(dirLight.color, lightColor, timeElapsed / lightChangeSpeed);
    //        dirLight.intensity = Mathf.Lerp(dirLight.intensity, lightIntensity, timeElapsed / lightChangeSpeed);
    //        timeElapsed += Time.deltaTime;
    //    }
    //    return null;
    //}
}
