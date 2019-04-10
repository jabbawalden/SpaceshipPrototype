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
    public float maxEngineMomentum;
    [SerializeField] private float speedLerp;

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

    [SerializeField] float rayDistance = 500;
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
        ForwardMomentum(speedState);
        ShipDirection(inputTurn, inputPitch);
        
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
            engineMomentum = Mathf.Lerp(engineMomentum, maxEngineMomentum / 2, speedLerp);
        }
        else if (speedState == SpeedState.boost)
        {
            engineMomentum = Mathf.Lerp(engineMomentum, maxEngineMomentum, speedLerp);
        }
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

    public void Aimer()
    {
        Ray ray = new Ray(rayOrigin.position, transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, rayDistance))
        {
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * rayDistance, Color.green);
        }

        AimerCubeTest.transform.position = ray.origin + ray.direction * rayDistance;
        
    }

}
