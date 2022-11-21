using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CartController : MonoBehaviour
{
    CinemachineDollyCart cart;
    float baseSpeed;
    [SerializeField] float boostModifier = 2.5f;
    [SerializeField] float trackSwitchSafety = 100f;
    [SerializeField] CinemachinePath mainTrack;
    [SerializeField] List<SideTrack> altTracks;
    private SideTrack currentSideTrack;

    void Start()
    {
        cart = GetComponent<CinemachineDollyCart>();
        baseSpeed = cart.m_Speed;
    }

    void Update()
    {
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            //set current direction the player is moving towards
            TrackSide direction = ProcessInput();

            //switch to an alternative track if there is one available in the direction the player is moving
            foreach (var altTrack in altTracks)
            {
                if (CheckTrackSwitchable(altTrack, direction))
                {
                    cart.m_Path = altTrack.track;
                    cart.m_Position = 0f;
                    currentSideTrack = altTrack;
                    break;
                }
            }
        }

        //set the speed of the cart
        if (Input.GetButton("Jump")) cart.m_Speed = baseSpeed * boostModifier;
        else cart.m_Speed = baseSpeed;

        //check if the player is at the end of a sidetrack
        if (cart.m_Path != mainTrack && cart.m_Position == currentSideTrack.track.PathLength)
        {
            //set followed path & position
            cart.m_Path = currentSideTrack.trackToSwitchBackTo;
            cart.m_Position = currentSideTrack.transferBackPos;
            if (cart.m_Path != mainTrack)
            {
                //set currentSideTrack to the new track
                foreach (var altTrack in altTracks)
                {
                    if (altTrack.track == cart.m_Path)
                    {
                        currentSideTrack = altTrack;
                    }
                }
            }
        }
    }

    TrackSide ProcessInput()
    {
        if (Input.GetAxis("Horizontal") > 0) return TrackSide.right;
        if (Input.GetAxis("Horizontal") < 0) return TrackSide.left;
        if (Input.GetAxis("Vertical") > 0) return TrackSide.up;
        return TrackSide.down;
    }

    //check if you can switch to the chosen sidetrack whilst moving in a given direction
    bool CheckTrackSwitchable(SideTrack sideTrack, TrackSide movementDirection)
    {
        float transferMinPos = sideTrack.transferToPos - cart.m_Speed / trackSwitchSafety;
        if (cart.m_Position >= transferMinPos && cart.m_Position <= sideTrack.transferToPos &&
            cart.m_Path == sideTrack.trackToSwitchFrom &&
            sideTrack.unlocked &&
            movementDirection == sideTrack.trackSide)
            return true;
        return false;
    }
}
