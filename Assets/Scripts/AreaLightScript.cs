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
    Beach, CoralReef, Cave, SunkenShip, KelpForest, Tornado
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
                StopCoroutine("LightRoutine");

                StartCoroutine(LightRoutine(area.lightIntensity, area.lightColor, area.fogColor));
            }
        }
    }

    public void Update()
    {
        timeElapsed += Time.deltaTime;
        if(timeElapsed > lightChangeSpeed + 1f)
        {
            StopCoroutine("LightRoutine");
        }
    }

    private IEnumerator LightRoutine(float lightIntensity, Color lightColor, Color fogColor)
    {
        timeElapsed = 0f;
        while (timeElapsed < lightChangeSpeed)
        {
            dirLight.color = Color.Lerp(dirLight.color, lightColor, timeElapsed / lightChangeSpeed);
            dirLight.intensity = Mathf.Lerp(dirLight.intensity, lightIntensity, timeElapsed / lightChangeSpeed);
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, fogColor, timeElapsed / lightChangeSpeed);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        Debug.Log("Loop Done");
        yield return null;
    }
}
