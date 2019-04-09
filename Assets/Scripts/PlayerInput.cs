using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerShoot playerShoot;

    [SerializeField] float speedUpSmooth;
    [SerializeField] float camFOVSmooth;

    [SerializeField] private Camera cam;
    [SerializeField] private float defaultFOV;
    [SerializeField] private float boostFOV;
    [SerializeField] private float currentFOV;


    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerShoot = GetComponent<PlayerShoot>();
        currentFOV = defaultFOV;
        cam.fieldOfView = currentFOV;
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
            playerController.engineMomentum = Mathf.Lerp(playerController.engineMomentum, playerController.maxEngineMomentum, speedUpSmooth * Time.deltaTime);
            currentFOV = Mathf.Lerp(currentFOV, boostFOV, camFOVSmooth * Time.deltaTime);
            cam.fieldOfView = currentFOV;
        }
        else 
        {
            playerController.engineMomentum = Mathf.Lerp(playerController.engineMomentum, playerController.maxEngineMomentum / 2, speedUpSmooth * Time.deltaTime);
            currentFOV = Mathf.Lerp(currentFOV, defaultFOV, camFOVSmooth * Time.deltaTime);
            cam.fieldOfView = currentFOV;
            //move camera closer - so set initial z length and new z length when speeding, then Mathf.Lerp between them using camFOVSmooth
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            playerShoot.ShootProjectile(playerController.engineMomentum);
        }
    }

}
