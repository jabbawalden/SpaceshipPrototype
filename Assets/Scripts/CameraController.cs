using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform shipMover;
    [SerializeField] float smoothLerp;

    // Update is called once per frame
    void FixedUpdate() 
    {   
        transform.position = shipMover.transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, shipMover.transform.rotation, smoothLerp);
    }

}
