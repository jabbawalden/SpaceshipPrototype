﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform shipMover;
    [SerializeField] float positionLerp; 
    [SerializeField] float rotationLerp;

    // Update is called once per frame
    void FixedUpdate() 
    {
        //transform.position = shipMover.transform.position;

        //transform.position.x = Mathf.Lerp(transform.position.x, shipMover.transform.position.x, positionLerp);

        transform.position = new Vector3(Mathf.Lerp(transform.position.x, shipMover.transform.position.x, positionLerp), Mathf.Lerp(transform.position.y, shipMover.transform.position.y, positionLerp), shipMover.position.z); 
        transform.rotation = Quaternion.Lerp(transform.rotation, shipMover.transform.rotation, rotationLerp);
    }

}
