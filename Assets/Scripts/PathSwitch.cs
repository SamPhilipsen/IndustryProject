using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSwitch : MonoBehaviour
{
    [SerializeField]
    private List<Cinemachine.CinemachinePathBase> Paths = new List<Cinemachine.CinemachinePathBase>();

    private Transform MainPath;

    private int i = 0;
    private void Start()
    {
        MainPath = Paths[0].transform;
    }
    private void Update()
    {
        if (Paths.Count != 1)
        {
            
        }
    }

    private void Switch()
    {
        this.gameObject.GetComponent<Cinemachine.CinemachineDollyCart>();

        if (i == Paths.Count - 1)
        {
            i = 0;
        }
        else
        {
            i++;
        }

        this.gameObject.GetComponent<Cinemachine.CinemachineDollyCart>().m_Path = Paths[i];
    }


    
}
