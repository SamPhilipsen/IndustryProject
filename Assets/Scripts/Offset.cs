using Cinemachine;
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

    [SerializeField] float raycastDistance = 1f;

    private float potXValue = 512f;
    private float potYValue = 512f;
    private float potSpeedValue = 5f;

    private float baseSpeed;
    [SerializeField]
    private float avoidSpeed = 1f;
    private Vector3 positionLastFrame;
    private Vector3 speedLastFrame;

    public Vector3 newPositionPlayer = new Vector3();
    [SerializeField]
    private string layerName;
    [SerializeField]
    private PauseHandler pauseHandler;

    [Header("Obstacle avoidance")]
    [SerializeField] [Tooltip("The avoiding speed is multiplied by this 1 + colliders hit / multiplier")] float correctionSpeedMultiplier;
    [SerializeField] [Tooltip("Directions the raycast check in on the x axis")] int[] x;
    [SerializeField] [Tooltip("Directions the raycast check in on the y axis")] int[] y;
    [SerializeField] [Tooltip("Directions the raycast check in on the z axis")] int[] z;

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
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseHandler.Pause();
        }
        if (speedLastFrame == null || positionLastFrame == null)
        {
            speedLastFrame = Vector3.zero;
            positionLastFrame = Vector3.zero;
        }
        else
        {
            speedLastFrame = positionLastFrame - player.transform.position;
        }
        positionLastFrame = player.transform.position;

        if (!CheckCollision())
        {
            //arduino controls
            GetPotValues();
            Movement();
            //arrow controls
            if (Input.GetKey("up") || Input.GetKey("down") || Input.GetKey("left") || Input.GetKey("right") || Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("a") || Input.GetKey("d") || Input.GetKey("space"))
            {
                Offsets();
            }
        }

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

        trackCart.GetComponent<Cinemachine.CinemachineDollyCart>().m_Speed = targetSpeed;
        SetMovement();
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

        trackCart.GetComponent<Cinemachine.CinemachineDollyCart>().m_Speed = targetSpeed;
        SetMovement();
    }

    private void SetMovement()
    {
        player.transform.localPosition = Vector3.MoveTowards(player.transform.localPosition, new Vector3(newPositionPlayer.x, newPositionPlayer.y), offsetValue);
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

        playerModel.transform.localRotation = Quaternion.Slerp(playerModel.transform.localRotation, Quaternion.Euler(newRotation.x, newRotation.y, newRotation.z).normalized, 10f * Time.deltaTime);
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

    private bool CheckCollision()
    {
        int layerMask = LayerMask.GetMask(layerName);
        List<RaycastHit> colliding = new();

        float sphereRadius = raycastDistance;
        float newRadius = sphereRadius / Mathf.Sqrt(2);
        float V3Radius = newRadius / Mathf.Sqrt(2);

        Vector3 moveSpeed = Vector3.zero;

        // Check all directions for a collision
        foreach(int x in x)
        {
            foreach(int y in y)
            {
                foreach(int z in z)
                {
                    if (x == 0 && y == 0 && z == 0)
                    {
                        continue;
                    }
                    RaycastHit hitInfo;
                    if (Physics.Raycast(player.transform.position, player.transform.TransformDirection(new Vector3(x * sphereRadius, y * sphereRadius, z * sphereRadius)), out hitInfo, sphereRadius, layerMask))
                    {
                        colliding.Add(hitInfo);
                        moveSpeed.x += x;
                        moveSpeed.y += y;
                        moveSpeed.z += z;
                    }
                }
            }
        }

        Debug.DrawRay(player.transform.position, player.transform.TransformDirection(new Vector3(sphereRadius, 0, 0)), Color.red);
        Debug.DrawRay(player.transform.position, player.transform.TransformDirection(new Vector3(0, sphereRadius, 0)), Color.red);
        Debug.DrawRay(player.transform.position, player.transform.TransformDirection(new Vector3(0, 0, sphereRadius)), Color.red);
        Debug.DrawRay(player.transform.position, player.transform.TransformDirection(new Vector3(-sphereRadius, 0, 0)), Color.red);
        Debug.DrawRay(player.transform.position, player.transform.TransformDirection(new Vector3(0, -sphereRadius, 0)), Color.red);
        Debug.DrawRay(player.transform.position, player.transform.TransformDirection(new Vector3(sphereRadius, sphereRadius, 0)), Color.red);
        Debug.DrawRay(player.transform.position, player.transform.TransformDirection(new Vector3(-sphereRadius, -sphereRadius, 0)), Color.red);
        Debug.DrawRay(player.transform.position, player.transform.TransformDirection(new Vector3(-sphereRadius, sphereRadius, 0)), Color.red);
        Debug.DrawRay(player.transform.position, player.transform.TransformDirection(new Vector3(sphereRadius, -sphereRadius, 0)), Color.red);
        Debug.DrawRay(player.transform.position, player.transform.TransformDirection(new Vector3(sphereRadius, sphereRadius, sphereRadius)), Color.red);
        Debug.DrawRay(player.transform.position, player.transform.TransformDirection(new Vector3(-sphereRadius, sphereRadius, sphereRadius)), Color.red);
        Debug.DrawRay(player.transform.position, player.transform.TransformDirection(new Vector3(sphereRadius, -sphereRadius, sphereRadius)), Color.red);
        Debug.DrawRay(player.transform.position, player.transform.TransformDirection(new Vector3(-sphereRadius, -sphereRadius, sphereRadius)), Color.red);
        Debug.DrawRay(player.transform.position, player.transform.TransformDirection(new Vector3(sphereRadius, 0, sphereRadius)), Color.red);
        Debug.DrawRay(player.transform.position, player.transform.TransformDirection(new Vector3(-sphereRadius, 0, sphereRadius)), Color.red);

        if (colliding.Count > 0)
        {
            Vector3 correctionDirection = Vector3.zero;
            foreach (RaycastHit hit in colliding)
            {
                correctionDirection += player.transform.InverseTransformPoint(hit.point);
            }
            correctionDirection.x = correctionDirection.x / colliding.Count * -1;
            correctionDirection.y = correctionDirection.y / colliding.Count * -1;
            correctionDirection.z = 0;

            Vector3 lastFrameCorrection;
            lastFrameCorrection.x = -speedLastFrame.x;
            lastFrameCorrection.y = -speedLastFrame.y;
            lastFrameCorrection.z = -speedLastFrame.z;
            //Debug.Log(lastFrameCorrection);

            correctionDirection += lastFrameCorrection;

            float speed = moveSpeed.magnitude * (1 + colliding.Count / correctionSpeedMultiplier) + (trackCart.GetComponent<CinemachineDollyCart>().m_Speed + speedLastFrame.magnitude) / baseSpeed;

            newPositionPlayer = Vector3.MoveTowards(player.transform.localPosition, player.transform.localPosition + correctionDirection, speed * Time.deltaTime);
            SetMovement();
            ///Debug.Log("Collision");
            return true;
        }
        ///Debug.Log("No Collision");
        return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.matrix = player.transform.localToWorldMatrix;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(player.transform.position, player.transform.position + new Vector3(raycastDistance, 0, 0));
        Gizmos.DrawLine(player.transform.position, player.transform.position + new Vector3(0, raycastDistance, 0));
        Gizmos.DrawLine(player.transform.position, player.transform.position + new Vector3(0, 0, raycastDistance));
        Gizmos.DrawLine(player.transform.position, player.transform.position + new Vector3(-raycastDistance, 0, 0));
        Gizmos.DrawLine(player.transform.position, player.transform.position + new Vector3(0, -raycastDistance, 0));
        Gizmos.DrawLine(player.transform.position, player.transform.position + new Vector3(raycastDistance, raycastDistance, 0));
        Gizmos.DrawLine(player.transform.position, player.transform.position + new Vector3(-raycastDistance, -raycastDistance, 0));
        Gizmos.DrawLine(player.transform.position, player.transform.position + new Vector3(raycastDistance, -raycastDistance, 0));
        Gizmos.DrawLine(player.transform.position, player.transform.position + new Vector3(-raycastDistance, raycastDistance, 0));
        Gizmos.color = Color.blue;
        float newRadius = raycastDistance / Mathf.Sqrt(2);
        Gizmos.DrawLine(player.transform.position, player.transform.position + new Vector3(newRadius, newRadius, newRadius));
        Gizmos.DrawLine(player.transform.position, player.transform.position + new Vector3(-newRadius, newRadius, newRadius));
        Gizmos.DrawLine(player.transform.position, player.transform.position + new Vector3(newRadius, -newRadius, newRadius));
        Gizmos.DrawLine(player.transform.position, player.transform.position + new Vector3(-newRadius, -newRadius, newRadius));

        Gizmos.DrawLine(player.transform.position, player.transform.position + new Vector3(0, newRadius, newRadius));
        Gizmos.DrawLine(player.transform.position, player.transform.position + new Vector3(0, -newRadius, newRadius));
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(player.transform.position, player.transform.position + new Vector3(raycastDistance, 0, raycastDistance));
        Gizmos.DrawLine(player.transform.position, player.transform.position + new Vector3(-raycastDistance, 0, raycastDistance));

    }
}

