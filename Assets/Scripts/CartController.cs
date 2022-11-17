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
            TrackSide direction = ProcessInput();
            foreach (var altTrack in altTracks)
            {
                if (CheckTrackSwitchable(altTrack) && cart.m_Path == altTrack.trackToSwitchFrom && altTrack.unlocked && direction == altTrack.trackSide)
                {
                    cart.m_Path = altTrack.track;
                    cart.m_Position = 0f;
                    currentSideTrack = altTrack;
                    break;
                }
            }
        }
        if (Input.GetButton("Jump")) cart.m_Speed = baseSpeed * boostModifier;
        else cart.m_Speed = baseSpeed;
        if (cart.m_Path != mainTrack && cart.m_Position == currentSideTrack.track.PathLength)
        {
            cart.m_Path = currentSideTrack.trackToSwitchBackTo;
            cart.m_Position = currentSideTrack.transferBackPos;
            if (cart.m_Path != mainTrack)
            {
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

    bool CheckTrackSwitchable(SideTrack sideTrack)
    {
        float transferMinPos = sideTrack.transferToPos - cart.m_Speed / trackSwitchSafety;
        if (cart.m_Position >= transferMinPos && cart.m_Position <= sideTrack.transferToPos) return true;
        return false;
    }
}
