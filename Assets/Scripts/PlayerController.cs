using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform shipMesh;
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
    public float smoothRoll;
    public float smoothPitch;

    Vector3 velocity;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();    
    }

    // Start is called before the first frame update
    void Start()
    {
        engineMomentum = maxEngineMomentum / 2;
        
    }

    // Update is called once per frame
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
        yaw += inputH * turnSpeed;
        pitch += inputV * pitchSpeed;
        roll = inputH * rotateSpeed;

        float newPitch = Mathf.SmoothDampAngle(transform.eulerAngles.x, pitch, ref velocity.x, smoothPitch);
        float newRoll = Mathf.SmoothDampAngle(transform.eulerAngles.z, roll, ref velocity.z, smoothRoll);

        Vector3 targetRotation = new Vector3();
        targetRotation.x += newPitch;
        targetRotation.y += yaw;
        targetRotation.z = newRoll;

        transform.localRotation = Quaternion.Euler(targetRotation);

        //transform.localRotation = Quaternion.Euler(newPitch, yaw, newRoll);
        //shipMesh.rotation = Quaternion.Euler(0, 0, roll);
        //transform.Rotate(transform.rotation.x, yaw, transform.rotation.z);
        //shipMesh.Rotate(transform.rotation.x, transform.rotation.y, roll);
        /*
        float zSmoothRotate = Mathf.SmoothDampAngle(transform.eulerAngles.z, x, ref velocityZ, smoothRotation);
        shipMesh.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, zSmoothRotate);
        */
    }


}
