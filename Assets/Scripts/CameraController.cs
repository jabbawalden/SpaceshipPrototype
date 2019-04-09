using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject shipPosition;
    public Transform shipPitch;
    public Camera cam;
    [SerializeField] float smoothZ;
    [SerializeField] float smoothY;
    [SerializeField] float smoothX;
    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate() 
    {
        /*
        float xPos = Mathf.SmoothDamp(transform.position.x, target.position.x, ref velocity.x, sX);
        float yPos = Mathf.SmoothDamp(transform.position.y, target.position.y, ref velocity.y, sY);
        */

        float xRot = Mathf.SmoothDampAngle(transform.localEulerAngles.x, shipPitch.transform.localEulerAngles.x, ref velocity.x, smoothX);
        float yRot = Mathf.SmoothDampAngle(transform.localEulerAngles.y, shipPosition.transform.localEulerAngles.y, ref velocity.y, smoothY);
        float zRot = Mathf.SmoothDampAngle(transform.localEulerAngles.z, shipPosition.transform.localEulerAngles.z, ref velocity.z, smoothZ);

        transform.position = shipPosition.transform.position;


        transform.rotation = Quaternion.Euler(xRot, yRot, transform.localEulerAngles.z);

        //transform.RotateAround(shipPosition.transform.position, new Vector3(xRot, yRot, transform.localRotation.z),0);
        
        
        //transform.rotation = Quaternion.Euler(shipPosition.rotation.x, shipPosition.rotation.y, shipPosition.rotation.z);
    }
}
