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
    [SerializeField] private Transform camHolder;
    [SerializeField] private float defaultFOV;
    [SerializeField] private float boostFOV;
    [SerializeField] private float currentFOV;


    // Start is called before the first frame update
    void Start()
    {
        camHolder.localPosition = new Vector3(camHolder.localPosition.x, camHolder.localPosition.y, camHolder.localPosition.z + camDefaultOffsetZ);
        cam.fieldOfView = defaultFOV;
    }

    // Update is called once per frame
    void FixedUpdate() 
    {
        if (playerController.alive)
            CameraStates();
    }

    void CameraStates()
    {
        if (playerController.speedState == SpeedState.normal)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defaultFOV, camFOVLerp);
            camHolder.localPosition = new Vector3(camHolder.localPosition.x, camHolder.localPosition.y, Mathf.Lerp(camHolder.localPosition.z, camDefaultOffsetZ, camOffsetLerp));
        }
        else if (playerController.speedState == SpeedState.boost)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, boostFOV, camFOVLerp);
            camHolder.localPosition = new Vector3(camHolder.localPosition.x, camHolder.localPosition.y, Mathf.Lerp(camHolder.localPosition.z, camBoostOffsetZ, camOffsetLerp));
        }
    }

}
