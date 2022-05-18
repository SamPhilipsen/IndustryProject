using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Offset : MonoBehaviour
{
    [SerializeField]
    GameObject trackCart;
    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject communicationManager;

    [SerializeField] float offsetValue = 0.05f;
    [SerializeField] float maxOffset = 25f;

    private float potXValue;
    private float potYValue;
    private float potSpeedValue;

    private float baseSpeed;

    private Vector3 newPositionPlayer = new Vector3();
    private void Awake()
    {
        newPositionPlayer = player.transform.localPosition;
        baseSpeed = trackCart.GetComponent<Cinemachine.CinemachineDollyCart>().m_Speed;
    }
    
    void FixedUpdate()
    {
        GetValues();

        Movement();

        //if (Input.GetKey("up") || Input.GetKey("down") || Input.GetKey("left") || Input.GetKey("right"))
        //{
        //    Offsets();
        //}
    }
    
    //if Arduino is not implemented
    private void Offsets()
    {
        float offsetY = 0f;
        float offsetX = 0f;
                
        if (Input.GetKey("up") && Input.GetKey("left"))
        {
            offsetX -= offsetValue * Mathf.Sqrt(2) / 2;
            offsetY += offsetValue * Mathf.Sqrt(2) / 2;
        }
        else if (Input.GetKey("up") && Input.GetKey("right"))
        {
            offsetX += offsetValue * Mathf.Sqrt(2) / 2;
            offsetY += offsetValue * Mathf.Sqrt(2) / 2;
        }
        else if(Input.GetKey("down") && Input.GetKey("left"))
        {
            offsetX -= offsetValue * Mathf.Sqrt(2) / 2;
            offsetY -= offsetValue * Mathf.Sqrt(2) / 2;
        }
        else if(Input.GetKey("down") && Input.GetKey("right"))
        {
            offsetX += offsetValue * Mathf.Sqrt(2) / 2;
            offsetY -= offsetValue * Mathf.Sqrt(2) / 2;
        }
        else if(Input.GetKey("up"))
        {
            offsetY += offsetValue;
        }
        else if(Input.GetKey("down"))
        {
            offsetY -= offsetValue;
        }
        else if(Input.GetKey("right"))
        {
            offsetX += offsetValue;
        }
        else if(Input.GetKey("left"))
        {
            offsetX -= offsetValue;
        }
                
        newPositionPlayer.x += offsetX;
        newPositionPlayer.y += offsetY;

        CheckOffset();

        player.transform.localPosition = new Vector3(newPositionPlayer.x, newPositionPlayer.y);
    }

    //if Arduino is implemented
    public void Movement()
    {
        float offsetY = 0f;
        float offsetX = 0f;

        ArduinoValues.GetvaluePotXMovement(potXValue);
        ArduinoValues.GetvaluePotYMovement(potYValue);
        ArduinoValues.CheckDirectionalSpeed();

        offsetX += offsetValue * ArduinoValues.xMovement;
        offsetY += offsetValue * ArduinoValues.yMovement;

        float targetSpeed = baseSpeed * ArduinoValues.GetValuePotSpeed(potSpeedValue);

        newPositionPlayer.x += offsetX;
        newPositionPlayer.y += offsetY;

        CheckOffset();

        player.transform.localPosition = new Vector3(newPositionPlayer.x, newPositionPlayer.y);
        trackCart.GetComponent<Cinemachine.CinemachineDollyCart>().m_Speed = targetSpeed;
    }

    //check if the max offset has been reached else set the offset to the max offset
    private void CheckOffset()
    {
        if (newPositionPlayer.x < -maxOffset)
        {
            newPositionPlayer.x = -maxOffset;
        }
        if (newPositionPlayer.x > maxOffset)
        {
            newPositionPlayer.x = maxOffset;
        }
        if (newPositionPlayer.y < -maxOffset)
        {
            newPositionPlayer.y = -maxOffset;
        }
        if (newPositionPlayer.y > maxOffset)
        {
            newPositionPlayer.y = maxOffset;
        }
    }

    private void GetValues()
    {
        potSpeedValue = communicationManager.GetComponent<Unity_recive_data_from_Arduino>().speed;
        potXValue = communicationManager.GetComponent<Unity_recive_data_from_Arduino>().direction;
        potYValue = communicationManager.GetComponent<Unity_recive_data_from_Arduino>().height;
    }

}
