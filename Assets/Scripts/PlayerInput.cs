using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerShoot playerShoot;

    [SerializeField] float camFOVSmooth;

    [SerializeField] private Camera cam;
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
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        playerController.inputTurn = h;
        playerController.inputPitch = v;

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
