using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform shipMesh;
    public Transform ShipRotate;
    public float engineMomentum;
    public float maxEngineMomentum;

    public float inputTurn;
    public float inputPitch;

    public float roll; 
    public float yaw;
    public float pitch;

    public float turnSpeed;
    public float rotateSpeed;
    public float pitchSpeed;

    public float smoothPitch;
    public float smoothYaw;
    public float smoothRoll;

    Vector3 velocity;
    private Rigidbody rb;
    Transform mT;

    private void Awake()
    {
        mT = transform;
        rb = GetComponent<Rigidbody>();    
    }

    void Start()
    {
        engineMomentum = maxEngineMomentum / 2;
    }

    void FixedUpdate() 
    {
        ForwardMomentum();
        ShipDirection(inputTurn, inputPitch);
    }

    private void ForwardMomentum()
    {
        rb.velocity = transform.forward * engineMomentum;

    }

    private void ShipDirection(float inputH, float inputV)
    {
        yaw = inputH * turnSpeed;
        pitch = inputV * pitchSpeed;
        roll = inputH * rotateSpeed;


        float lerpPitch = Mathf.LerpAngle(mT.rotation.x, pitch, smoothPitch);
        float lerpYaw = Mathf.LerpAngle(mT.rotation.y, yaw, smoothYaw);
        float lerpRoll = Mathf.LerpAngle(mT.rotation.z, roll, smoothRoll);

        shipMesh.localRotation = Quaternion.Euler(shipMesh.localRotation.x, shipMesh.localRotation.y, -lerpRoll);

        if (inputH != 0)
        {
            mT.Rotate(0, lerpYaw, 0);
        }
        else
        {
            mT.Rotate(0, 0, 0);
        }

        if (inputV != 0)
        {
            mT.Rotate(lerpPitch, 0, 0);
        }
        else
        {
            mT.Rotate(0, 0, 0);
        }

    }



}
