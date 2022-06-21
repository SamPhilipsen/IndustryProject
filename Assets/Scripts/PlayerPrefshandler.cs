using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class PlayerPrefshandler
{
    public static void SaveValues(int indexSliders, List<GameObject> values, List<string> playerPrefIndex)
    {
        int j = 0;
        for (int i = 0 + (indexSliders * 3); i < (indexSliders + 1) * 3; i++)
        {
            PlayerPrefs.SetFloat(playerPrefIndex[i], values[j].GetComponent<Slider>().value);
            j++;
        }
        PlayerPrefs.Save();
    }

    public static List<float> GetValues(List<string> playerPrefIndex)
    {
        List<float> values = new List<float>();
        for (int i = 0; i < 9; i++)
        {
            float value = PlayerPrefs.GetFloat(playerPrefIndex[i], -1f);
            values.Add(value);
        }

        for (int i = 0; i < values.Count; i++)
        {
            if (values[i] < 0f)
            {
                switch (i)
                {
                    case 0:
                        values[i] = 0f;
                        break;
                    case 1:
                        values[i] = 512f;
                        break;
                    case 2:
                        values[i] = 1023f;
                        break;
                    case 3:
                        values[i] = 0f;
                        break;
                    case 4:
                        values[i] = 512f;
                        break;
                    case 5:
                        values[i] = 1023f;
                        break;
                    case 6:
                        values[i] = 0f;
                        break;
                    case 7:
                        values[i] = 6f;
                        break;
                    case 8:
                        values[i] = 12f;
                        break;
                    default:
                        break;
                }
            }
        }
        return values;
    }

    public static void SetGlobalValues(List<string> playerPrefIndex)
    {
        List<float> allValues = GetValues(playerPrefIndex);

        //horizontal
        GlobalPotValues.horizontalValues = new DifferentPotValues(allValues[0], allValues[1], allValues[2]);
        //vertical
        GlobalPotValues.verticalValues = new DifferentPotValues(allValues[3], allValues[4], allValues[5]);
        //speed
        GlobalPotValues.speedValues = new DifferentPotValues(allValues[6], allValues[7], allValues[8]);
    }
}
