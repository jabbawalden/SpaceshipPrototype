﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerShoot playerShoot;
    private CameraFollowMouse camFollowMouse;
    private GameManager gameManager;

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
    public bool isShooting;
    public bool isBoosting;

    public float hKey;
    public float vKey;

    //Right joystick moves roll and pitch(normal)
    public float rKey;
    public float vKey2;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerShoot = GetComponent<PlayerShoot>();
        gameManager = FindObjectOfType<GameManager>();
        camFollowMouse = GameObject.Find("CameraFollowMouse").GetComponent<CameraFollowMouse>();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        //input functions called
        if (gameManager.gameState == GameState.Playing)
        {
            if (playerController.alive)
            {
                ShipMovementBasicInput();
                ShipSpeedControlInput();
                ShipBoostInput();
                ShipShootInput();
                CameraFollowInput();
            }
        }
        else if (gameManager.gameState == GameState.Restart)
        {
            RestartButton();
            QuitButton();
        }
        else if (gameManager.gameState == GameState.Start)
        {
            StartButton();
            QuitButton();
        }

    }

    private void RestartButton()
    {
        if (Input.GetKeyDown("joystick button 7"))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void StartButton()
    {
        if (Input.GetKeyDown("joystick button 7"))
        {
            gameManager.GameStatePlaying();
        }
    }

    private void QuitButton()
    {
        if (Input.GetKeyDown("joystick button 6"))
        {
            Application.Quit();
        }


    }

    private void ShipMovementBasicInput()
    {        
        //Left joystick moves yaw and pitch(inverted)
        hKey = Input.GetAxis("Horizontal");
        vKey = Input.GetAxis("Vertical");

        //Right joystick moves roll and pitch(normal)
        rKey = Input.GetAxis("Horizontal1");
        vKey2 = Input.GetAxis("Vertical1");
        //need input for rolling

        playerController.inputPitch = vKey;


        if (vKey != 0 && vKey2 == 0)
        {
            playerController.inputPitch = vKey;
        }
        else if (vKey == 0 && vKey2 != 0)
        {
            playerController.inputPitch = vKey2;
        }
        else if (vKey != 0 && vKey2 != 0)
        {
            playerController.inputPitch = vKey + vKey2;
        }

        playerController.inputTurn = hKey;
        playerController.inputRoll = rKey;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerController.SpinShip(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerController.SpinShip(false);
        }
    }

    private void ShipSpeedControlInput()
    {
        //print(Input.mouseScrollDelta);

        if (Input.mouseScrollDelta.y != 0)
            playerController.ChangeMomentum(Input.mouseScrollDelta.y * 15);
    }

    private void ShipBoostInput()
    {
        //set ship state to control behavior
        float bKey = Input.GetAxis("Boost");

        if (bKey != 0 && !playerController.haveReachedLimit)
        {
            playerController.speedState = SpeedState.boost;
            isBoosting = true;
        }
        else
        {
            playerController.speedState = SpeedState.normal;
            isBoosting = false;
        }
    }

    private void ShipShootInput()
    {

        //Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Ray ray = new Ray (shootDirection.transform.position, shootDirection.transform.forward);
        //Debug.DrawRay(cam.transform.position, ray.direction * 1000, Color.red, 2);

        ///below was intended for rotating the shoot direction object so that player could aim while moving at the same time.
        //float rightJoyH = Input.GetAxis("Horizontal1");
        //float rightJoyV = Input.GetAxis("Vertical1");
        //print(rightJoyH);
        //print(rightJoyV); 

        //float rotH = rightJoyH * rotateDestination;
        //float rotV = rightJoyV * rotateDestination;
        //lerpValueH = Mathf.Lerp(currentH, rotH, smoothTime);
        //lerpValueV = Mathf.Lerp(currentV, rotV, smoothTime);

        //currentH = lerpValueH;
        //currentV = lerpValueV;

        //shootDirection.transform.localRotation = Quaternion.Euler(-rotV, rotH + 90, 0);
        /*
        if (Input.GetKey(KeyCode.Mouse0))
        {
            playerShoot.ShootProjectile(playerController.engineMomentum, ray.direction);
        }
        */
        if (Input.GetAxis("Fire1") != 0 && !playerShoot.haveReachedLimit)
        {
            playerShoot.ShootProjectile(playerController.engineMomentum, ray.direction);
            isShooting = true;
        }
        else
        {
            isShooting = false;
        }

        //if (Input.GetKey(KeyCode.Mouse0) && !playerShoot.haveReachedLimit)
        //{
        //    playerShoot.ShootProjectile(playerController.engineMomentum, ray.direction);
        //    isShooting = true;
        //}
        //else
        //{
        //    isShooting = false;
        //}
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
