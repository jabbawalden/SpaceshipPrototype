using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerShoot playerShoot;
    private CameraFollowMouse camFollowMouse;
    public GameObject shootDirection;

    [SerializeField] Camera cam;
    [SerializeField] private float defaultFOV;
    [SerializeField] private float boostFOV;
    [SerializeField] private float currentFOV;
    [SerializeField] private float xThresh;
    [SerializeField] private float yThresh;
    [SerializeField] private float xLerp;
    [SerializeField] private float yLerp;
    [SerializeField] private float zLerp;
    [SerializeField] private float xDivider;
    [SerializeField] private float yDivider;

    public float currentH;
    public float currentV;
    public float lerpValueH;
    public float lerpValueV;
    public float smoothTime;
    public float rotateDestination; 
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerShoot = GetComponent<PlayerShoot>();
        camFollowMouse = GameObject.Find("CameraFollowMouse").GetComponent<CameraFollowMouse>();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        //input functions called
        ShipMovementBasicInput();
        ShipSpeedControlInput();
        ShipBoostInput();
        ShipShootInput();
        CameraFollowInput();
    }

    private void ShipMovementBasicInput()
    {        
        
        //old method using WASD
        float hKey = Input.GetAxis("Horizontal");
        float vKey = Input.GetAxis("Vertical");

        playerController.inputTurn = hKey;
        playerController.inputPitch = vKey;

    }

    private void ShipSpeedControlInput()
    {
        //print(Input.mouseScrollDelta);

        if (Input.mouseScrollDelta.y != 0)
            playerController.ChangeMomentum(Input.mouseScrollDelta.y * 15);
    }

    private void ShipBoostInput()
    {
        //set ship state to control behaviour
        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerController.speedState = SpeedState.boost;
        }
        else
        {
            playerController.speedState = SpeedState.normal;
        }
    }

    private void ShipShootInput()
    {

        //Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Ray ray = new Ray (shootDirection.transform.position, shootDirection.transform.forward);
        //Debug.DrawRay(cam.transform.position, ray.direction * 1000, Color.red, 2);

        float rightJoyH = Input.GetAxis("Horizontal1");
        float rightJoyV = Input.GetAxis("Vertical1");
        print(rightJoyH);
        print(rightJoyV);

        float rotH = rightJoyH * rotateDestination;
        float rotV = rightJoyV * rotateDestination;
        lerpValueH = Mathf.Lerp(currentH, rotH, smoothTime);
        lerpValueV = Mathf.Lerp(currentV, rotV, smoothTime);

        currentH = lerpValueH;
        currentV = lerpValueV;

        shootDirection.transform.localRotation = Quaternion.Euler(-rotV, rotH + 90, 0);

        if (Input.GetKey(KeyCode.Mouse0))
        {
            playerShoot.ShootProjectile(playerController.engineMomentum, ray.direction);
        }

        if (Input.GetAxis("Fire1") != 0)
        {
            playerShoot.ShootProjectile(playerController.engineMomentum, ray.direction);
        }
    }

    private void CameraFollowInput ()
    {

        //gets screen center
        Vector3 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        //sends centre of mouse position to the center by offsetting it
        Vector2 mousePos = Input.mousePosition - screenCenter;
        //normalise values
        mousePos.x = mousePos.x / xDivider;
        mousePos.y = mousePos.y / yDivider;

        float h = 0;
        float v = 0;


        if (mousePos.x >= xThresh || mousePos.x <= -xThresh)
            h = Mathf.Lerp(h, mousePos.x, xLerp);
        else
            h = Mathf.Lerp(h, 0, xLerp);

        if (mousePos.y >= yThresh || mousePos.y <= -yThresh)
            v = Mathf.Lerp(h, mousePos.y, yLerp);
        else
            v = Mathf.Lerp(h, 0, yLerp);

        camFollowMouse.inputTurn = h;
        camFollowMouse.inputPitch = -v;

    }

}
