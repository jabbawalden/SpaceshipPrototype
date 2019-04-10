using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerShoot playerShoot;

    [SerializeField] Camera cam;
    [SerializeField] private float defaultFOV;
    [SerializeField] private float boostFOV;
    [SerializeField] private float currentFOV;


    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerShoot = GetComponent<PlayerShoot>();
    }

    // Update is called once per frame
    void Update()
    {
        //gets screen center
        Vector3 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        //sends centre of mouse position to the center by offsetting it
        Vector2 mousePos = Input.mousePosition - screenCenter;
        //normalise values
        mousePos = mousePos.normalized;

        //old method using WASD
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");

        float h = mousePos.x;
        float v = mousePos.y;

        print(mousePos);

        playerController.inputTurn = h;
        playerController.inputPitch = -v;


        if (Input.GetKey(KeyCode.Space))
        {
            playerController.speedState = SpeedState.boost;
        }
        else 
        {
            playerController.speedState = SpeedState.normal;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            playerShoot.ShootProjectile(playerController.engineMomentum);
        }
    }

}
