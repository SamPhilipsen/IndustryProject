using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementCommunicator : MonoBehaviour
{
    [SerializeField] Offset offsetScript;
    [SerializeField] TrackManager trackManager;
    [SerializeField] Canvas canvas;

    private string offsetSide;
    void Start()
    {
        trackManager.nearingSwitch += NearingSwitch;
        canvas.gameObject.SetActive(false);
    }

    void NearingSwitch(bool nearingSwitch)
    {
        if (nearingSwitch)
            canvas.gameObject.SetActive(true);
        else
            canvas.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (offsetScript.newPositionPlayer.x >= 2) offsetSide = "right";
        else if (offsetScript.newPositionPlayer.x <= -2) offsetSide = "left";
        else offsetSide = "forward";

        trackManager.switchingTracks = offsetSide;

        if(offsetSide == "left")
          canvas.transform.Find("LeftArrow").GetComponent<RawImage>().color = new Color(255, 255, 0);
        else
            canvas.transform.Find("LeftArrow").GetComponent<RawImage>().color = new Color(0, 0, 0);

        if (offsetSide == "right")
            canvas.transform.Find("RightArrow").GetComponent<RawImage>().color = new Color(255, 255, 0);
        else
            canvas.transform.Find("RightArrow").GetComponent<RawImage>().color = new Color(0, 0, 0);

        //canvas.GetComponentInChildren<RawImage>().color = new Color(255, 255, 0);
    }
}
