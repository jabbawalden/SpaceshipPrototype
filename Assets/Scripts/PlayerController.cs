using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpeedState {normal, boost}

public class PlayerController : MonoBehaviour
{
    public SpeedState speedState;
    public Transform shipMesh;
    public Transform rayOrigin;
    public GameObject AimerCubeTest;
    public float engineMomentum;
    public float currentEngineMomentum;
    public float maxEngineMomentum;
    public float boostEngineMomentum;
    [SerializeField] private float speedLerp;
    [SerializeField] private float engineMomentumChangeLerp;

    public float inputTurn;
    public float inputPitch;
    public float inputRoll;

    [SerializeField] private float roll;
    [SerializeField] private float yaw;
    [SerializeField] private float pitch; 
    [SerializeField] private float mainRoll;

    public float turnSpeed;
    public float rotateSpeed;
    public float pitchSpeed;

    public float smoothPitch;
    public float smoothYaw;
    public float smoothRoll;

    Vector3 velocity;
    private Rigidbody rb;
    Transform mT;

    [SerializeField] float rayDistance = 500;

    private void Awake()
    {
        mT = transform;
        rb = GetComponent<Rigidbody>();    
    }

    void Start()
    {
        //default engine start
        engineMomentum = maxEngineMomentum * 0.75f;
        currentEngineMomentum = maxEngineMomentum * 0.75f;
    }

    void FixedUpdate() 
    {
        ForwardMomentum(speedState);
        ShipDirection(inputTurn, inputPitch, inputRoll);
        
    }

    private void Update()
    {
        Aimer();
    }

    private void ForwardMomentum(SpeedState speedState)
    {
        rb.velocity = transform.forward * engineMomentum;

        if (speedState == SpeedState.normal)
        {
            engineMomentum = Mathf.Lerp(engineMomentum, currentEngineMomentum, speedLerp);
        }
        else if (speedState == SpeedState.boost)
        {
            engineMomentum = Mathf.Lerp(engineMomentum, boostEngineMomentum, speedLerp);
        }
    }

    public void ChangeMomentum(float newValue)
    {
        if (speedState == SpeedState.normal)
        {
            if (currentEngineMomentum >= 0 && currentEngineMomentum < maxEngineMomentum)
            {
                currentEngineMomentum = Mathf.Lerp(currentEngineMomentum, currentEngineMomentum += newValue, engineMomentumChangeLerp);
            }
            else if (currentEngineMomentum >= maxEngineMomentum)
            {
                currentEngineMomentum = maxEngineMomentum - 1;
            }
            else if (currentEngineMomentum <= 0)
            {
                currentEngineMomentum = 0.5f;
            }
        }

        /*
        if (currentEngineMomentum > 0 && currentEngineMomentum < maxEngineMomentum)
        {
            engineMomentum = Mathf.Lerp(currentEngineMomentum, currentEngineMomentum += newValue, engineMomentumChangeLerp);
        }
        else if (currentEngineMomentum > maxEngineMomentum)
            currentEngineMomentum = maxEngineMomentum;
        else if (currentEngineMomentum < 0)
            currentEngineMomentum = 0;
            */
    }

    private void ShipDirection(float inputH, float inputV, float inputR)
    {
        yaw = inputH * turnSpeed;
        pitch = inputV * pitchSpeed;
        roll = inputH * rotateSpeed;
        mainRoll = inputR * rotateSpeed;

        float lerpPitch = Mathf.LerpAngle(mT.rotation.x, pitch, smoothPitch); 
        float lerpYaw = Mathf.LerpAngle(transform.rotation.y, yaw, smoothYaw);
        float lerpRoll = Mathf.LerpAngle(mT.rotation.z, roll, smoothRoll);
        float lerpMainRoll = Mathf.LerpAngle(mT.rotation.z, mainRoll, smoothRoll);

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

        
        if (inputR != 0)
        {
            mT.Rotate(0, 0, lerpMainRoll);
        }
        else
        {
            mT.Rotate(0, 0, 0);
        }
        
    }

    public void Aimer()
    {
        Ray ray = new Ray(rayOrigin.position, transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, rayDistance))
        {
            //Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 2);
        }
        else
        {
            //Debug.DrawLine(ray.origin, ray.origin + ray.direction * rayDistance, Color.green, 2);
        }

        AimerCubeTest.transform.position = ray.origin + ray.direction * rayDistance;
        
    }

}
