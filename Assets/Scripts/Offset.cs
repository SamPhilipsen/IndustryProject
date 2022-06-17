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
    [SerializeField]
    GameObject playerModel;

    [SerializeField] float offsetValue = 0.05f;
    [SerializeField] float maxOffset = 25f;

    [SerializeField] float rotateScale = 0.5f;
    [SerializeField] float maxRotation = 20f;

    [SerializeField] float sphereRadius = 1f;

    private float potXValue = 512f;
    private float potYValue = 512f;
    private float potSpeedValue = 5f;

    private float baseSpeed;

    public Vector3 newPositionPlayer = new Vector3();
    [SerializeField]
    private string layerName;

    private void Awake()
    {
        SetGlobalValues();

        newPositionPlayer = player.transform.localPosition;
        baseSpeed = trackCart.GetComponent<Cinemachine.CinemachineDollyCart>().m_Speed;
    }

    private void SetGlobalValues()
    {
        List<string> playerPrefIndex = new List<string>();
        for (int i = 0; i < 9; i++)
        {
            playerPrefIndex.Add(i.ToString());
        }

        PlayerPrefshandler.SetGlobalValues(playerPrefIndex);
    }
    
    void FixedUpdate()
    {
        //arduino controls
        //GetPotValues();
        //Movement();

        //arrow controls
        if (Input.GetKey("up") || Input.GetKey("down") || Input.GetKey("left") || Input.GetKey("right") || Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("a") || Input.GetKey("d") || Input.GetKey("space"))
        {
            Offsets();
        }

        CheckCollision();
    }

    //if Arduino is not implemented
    private void Offsets()
    {
        float offsetY = 0f;
        float offsetX = 0f;
        float targetSpeed = baseSpeed;
                
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

        ///////////////////////
        if (Input.GetKey("w") && Input.GetKey("a"))
        {
            offsetX -= offsetValue * Mathf.Sqrt(2) / 2;
            offsetY += offsetValue * Mathf.Sqrt(2) / 2;
        }
        else if (Input.GetKey("w") && Input.GetKey("d"))
        {
            offsetX += offsetValue * Mathf.Sqrt(2) / 2;
            offsetY += offsetValue * Mathf.Sqrt(2) / 2;
        }
        else if (Input.GetKey("s") && Input.GetKey("a"))
        {
            offsetX -= offsetValue * Mathf.Sqrt(2) / 2;
            offsetY -= offsetValue * Mathf.Sqrt(2) / 2;
        }
        else if (Input.GetKey("s") && Input.GetKey("d"))
        {
            offsetX += offsetValue * Mathf.Sqrt(2) / 2;
            offsetY -= offsetValue * Mathf.Sqrt(2) / 2;
        }
        else if (Input.GetKey("w"))
        {
            offsetY += offsetValue;
        }
        else if (Input.GetKey("s"))
        {
            offsetY -= offsetValue;
        }
        else if (Input.GetKey("d"))
        {
            offsetX += offsetValue;
        }
        else if (Input.GetKey("a"))
        {
            offsetX -= offsetValue;
        }
        /////////////////////

        if (Input.GetKey("space"))
        {
            targetSpeed *= 3f;
        }

        PlayerRotation(offsetX, offsetY);

        newPositionPlayer.x += offsetX;
        newPositionPlayer.y += offsetY;
        CheckOffset();

        player.transform.localPosition = new Vector3(newPositionPlayer.x, newPositionPlayer.y);
        trackCart.GetComponent<Cinemachine.CinemachineDollyCart>().m_Speed = targetSpeed;
    }

    //if Arduino is implemented
    public void Movement()
    {
        float offsetY = 0f;
        float offsetX = 0f;

        ArduinoValues.GetvaluePotXMovement(potXValue);
        ArduinoValues.GetvaluePotYMovement(potYValue);
        //ArduinoValues.CheckDirectionalSpeed();

        offsetX += offsetValue * ArduinoValues.xMovement;
        offsetY += offsetValue * ArduinoValues.yMovement;

        PlayerRotation(offsetX, offsetY);

        float targetSpeed = baseSpeed * ArduinoValues.GetValuePotSpeed(potSpeedValue);

        newPositionPlayer.x += offsetX;
        newPositionPlayer.y += offsetY;
        CheckOffset();

        player.transform.localPosition = new Vector3(newPositionPlayer.x, newPositionPlayer.y);
        trackCart.GetComponent<Cinemachine.CinemachineDollyCart>().m_Speed = targetSpeed;
    }

    private void PlayerRotation(float offsetX, float offsetY)
    {
        Quaternion newRotation = new Quaternion();
        newRotation.x = playerModel.transform.localRotation.x;
        newRotation.y = playerModel.transform.localRotation.y;
        newRotation.z = playerModel.transform.localRotation.z;

        //switch (offsetX)
        //{
        //    case < 0:
        //        if (playerModel.transform.localRotation.x < 0)
        //        {
        //            newRotation.x += 2 * rotateScale;
        //        }
        //        newRotation.x += rotateScale;
        //        break;
        //    case > 0:
        //        if (playerModel.transform.localRotation.x > 0)
        //        {
        //            newRotation.x += 2 * -rotateScale;
        //        }
        //        newRotation.x += -rotateScale;
        //        break;
        //    default:
        //        break;
        //}

        //switch (offsetY)
        //{
        //    case < 0:
        //        if (playerModel.transform.localRotation.z > 0)
        //        {
        //            newRotation.z += 2 * -rotateScale;
        //            break;
        //        }
        //        newRotation.z += -rotateScale;
        //        break;
        //    case > 0:
        //        if (playerModel.transform.localRotation.z < 0)
        //        {
        //            newRotation.z += 2 * rotateScale;
        //            break;
        //        }
        //        newRotation.z += rotateScale;
        //        break;
        //    default:
        //        break;
        //}

        //newRotation.x = Mathf.Clamp(newRotation.x, -maxRotation, maxRotation);
        //newRotation.z = Mathf.Clamp(newRotation.z, -maxRotation, maxRotation);

        //if (newRotation.x > maxRotation)
        //{
        //    newRotation.x = maxRotation;
        //}
        //if (newRotation.x < maxRotation)
        //{
        //    newRotation.x = -maxRotation;
        //}
        //if (newRotation.z > maxRotation)
        //{
        //    newRotation.z = maxRotation;
        //}
        //if (newRotation.z < maxRotation)
        //{
        //    newRotation.z = -maxRotation;
        //}

        if (offsetX > 0)
        {
            newRotation.z = -maxRotation;
        }
        if (offsetX < 0)
        {
            newRotation.z = maxRotation;
        }
        if (offsetY > 0)
        {
            newRotation.x = -maxRotation;
        }
        if (offsetY < 0)
        {
            newRotation.x = maxRotation;
        }

        playerModel.transform.localRotation = Quaternion.RotateTowards(playerModel.transform.localRotation, Quaternion.Euler(newRotation.x, newRotation.y, newRotation.z).normalized, 100f * Time.deltaTime);
        //Debug.Log(playerModel.transform.localRotation);

        //Vector3 rot = new Vector3(newRotation.x, newRotation.y, newRotation.z);
        //playerModel.transform.Rotate(rot, Space.Self);
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

    private void GetPotValues()
    {
        if (communicationManager.GetComponent<Unity_recive_data_from_Arduino>().isConnected)
        {
            potSpeedValue = communicationManager.GetComponent<Unity_recive_data_from_Arduino>().speed;
            potXValue = communicationManager.GetComponent<Unity_recive_data_from_Arduino>().direction;
            potYValue = communicationManager.GetComponent<Unity_recive_data_from_Arduino>().height;
        }
        else
        {
            potSpeedValue = GlobalPotValues.speedValues.turnoverValue;
            potXValue = GlobalPotValues.horizontalValues.turnoverValue;
            potYValue = GlobalPotValues.verticalValues.turnoverValue;
        }        
    }

    private void CheckCollision()
    {
        Debug.Log("check collision");
        if (Physics.CheckSphere(player.transform.position, sphereRadius, LayerMask.NameToLayer(layerName)))
        {
            Debug.Log("hit terrain");
        }
    }
}
