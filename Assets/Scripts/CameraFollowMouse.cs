using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowMouse : MonoBehaviour
{
    public float inputTurn;
    public float inputPitch;

    [SerializeField] private float roll;
    [SerializeField] private float yaw;
    [SerializeField] private float pitch;
    public float turnSpeed;
    public float rotateSpeed;
    public float pitchSpeed;

    public float pitchSmooth;
    public float turnSmooth;
    Transform mT;

    private void Awake()
    {
        mT = transform;
    }

    private void FixedUpdate()
    {
        CameraDirection(inputTurn, inputPitch); 
    }

    private void CameraDirection(float inputH, float inputV)
    {
        yaw = inputH * turnSpeed;
        pitch = inputV * pitchSpeed;
        roll = inputH * rotateSpeed;

        float lerpPitch = Mathf.LerpAngle(mT.localRotation.x, pitch, pitchSmooth);
        float lerpYaw = Mathf.LerpAngle(mT.localRotation.y, yaw, turnSmooth);

        float lerpOPitch = Mathf.LerpAngle(mT.localRotation.x, 0, pitchSmooth);
        float lerpOYaw = Mathf.LerpAngle(mT.localRotation.y, 0, turnSmooth);

        if (inputH != 0 || inputV != 0)
        {
            //mT.Rotate(0, yaw, 0);
            mT.localRotation = Quaternion.Euler(lerpPitch, lerpYaw, mT.localRotation.z);
        }
        else
        {
            mT.localRotation = Quaternion.Euler(lerpOPitch, lerpOYaw, mT.localRotation.z);
        }

        /*
        if (inputH != 0)
        {
            //mT.Rotate(0, yaw, 0);
            mT.localRotation = Quaternion.Euler(mT.rotation.x, lerpYaw, mT.localRotation.z);
        }
        else
        {
            mT.localRotation = Quaternion.Euler(mT.rotation.x, lerpOYaw, mT.localRotation.z);
        }
        
        
        if (inputV != 0)
        {
            //mT.Rotate(pitch, 0, 0);
            mT.localRotation = Quaternion.Euler(lerpPitch, mT.rotation.y, mT.localRotation.z);
        }
        else
        {
            //mT.Rotate(0, 0, 0);
            mT.localRotation = Quaternion.Euler(lerpOPitch, mT.rotation.y, mT.localRotation.z);
        }*/
    }
    


}
