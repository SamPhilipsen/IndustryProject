using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CartController : MonoBehaviour
{
    CinemachineDollyCart cart;
    float baseSpeed;
    private float boostModifier;
    [SerializeField] float trackSwitchSafety = 100f;
    [SerializeField] CinemachinePath mainTrack;
    [SerializeField] List<SideTrack> altTracks;
    private SideTrack currentSideTrack;
    private TrackSide cartDirection;

    void Start()
    {
        cart = GetComponent<CinemachineDollyCart>();
        baseSpeed = cart.m_Speed;
        boostModifier = 1f;
    }

    void Update()
    {
        //switch to an alternative track if there is one available in the direction the player is moving
        foreach (var altTrack in altTracks)
        {
            if (CheckTrackSwitchable(altTrack, cartDirection))
            {
                cart.m_Path = altTrack.track;
                cart.m_Position = 0f;
                currentSideTrack = altTrack;
                break;
            }
        }

        //set the speed of the cart
        cart.m_Speed = baseSpeed * boostModifier;

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

    //function to set the direction the cart is going
    public void SetDirection(TrackSide newCartdirection)
    {
        cartDirection = newCartdirection;
    }

    //function to set the boost modifier
    public void SetBoostModifier(float newBoostModifier)
    {
        boostModifier = newBoostModifier;
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
