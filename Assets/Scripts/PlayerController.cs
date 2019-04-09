using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform shipMesh;
    public Transform ShipRotate;
    [SerializeField] private float engineMomentum;
    [SerializeField] private float maxEngineMomentum;

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
        rb = GetComponentInParent<Rigidbody>();    
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

        float newPitch = Mathf.SmoothDampAngle(mT.localRotation.x, pitch, ref velocity.x, smoothPitch);
        float newRoll = Mathf.SmoothDampAngle(shipMesh.localRotation.z, roll, ref velocity.z, smoothRoll);
        float newYaw = Mathf.SmoothDampAngle(ShipRotate.localRotation.z, yaw, ref velocity.z, smoothRoll);

        shipMesh.localRotation = Quaternion.Euler(shipMesh.localRotation.x, shipMesh.localRotation.y, -newRoll);

        if (inputH != 0)
        {
            ShipRotate.Rotate(ShipRotate.rotation.x, newYaw, ShipRotate.rotation.z);
        }

        if (inputV != 0)
        {
            mT.Rotate(newPitch, 0, 0);
            print(newPitch);
        }
        else
        {
            mT.Rotate(0, 0, 0);
        }


        /*

        float testPitch = Mathf.Repeat(newPitch, 180);
        print(testPitch);

        Vector3 targetRotation = new Vector3(newPitch, yaw, newRoll);
        */
        //transform.localRotation = Quaternion.Euler(targetRotation);


        //targetRotation.x = Mathf.Repeat(targetRotation.x, 360f);
        //print(targetRotation.x);
        //transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z));

        //transform.localRotation = Quaternion.Euler(targetRotation);
        //transform.localRotation = Quaternion.Euler(targetRotation);
        //transform.Rotate(new Vector3(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z));
        //transform.Rotate(new Vector3(newPitch, yaw, newRoll));

        //transform.localRotation = Quaternion.Euler(newPitch, yaw, newRoll);
        //shipMesh.rotation = Quaternion.Euler(0, 0, roll);
        //transform.Rotate(transform.rotation.x, yaw, transform.rotation.z);
        //shipMesh.Rotate(transform.rotation.x, transform.rotation.y, roll);

        //float zSmoothRotate = Mathf.SmoothDampAngle(transform.eulerAngles.z, x, ref velocityZ, smoothRotation);
        //shipMesh.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, zSmoothRotate);

    }


}
