using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private float camFOVLerp;
    [SerializeField] private float camOffsetLerp;

    [SerializeField] private float camDefaultOffsetZ;
    [SerializeField] private float camBoostOffsetZ;

    [SerializeField] private PlayerController playerController;

    [SerializeField] private Camera cam;
    [SerializeField] private float defaultFOV;
    [SerializeField] private float boostFOV;
    [SerializeField] private float currentFOV;


    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, camDefaultOffsetZ);
        cam.fieldOfView = defaultFOV;
    }

    // Update is called once per frame
    void FixedUpdate() 
    {
        print(transform.localPosition);
        CameraStates();
    }

    void CameraStates()
    {
        if (playerController.speedState == SpeedState.normal)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defaultFOV, camFOVLerp);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Lerp(transform.localPosition.z, camDefaultOffsetZ, camOffsetLerp));
        }
        else if (playerController.speedState == SpeedState.boost)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, boostFOV, camFOVLerp);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Lerp(transform.localPosition.z, camBoostOffsetZ, camOffsetLerp));
        }
    }

}
