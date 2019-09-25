﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SpeedState {normal, boost}

public class PlayerController : MonoBehaviour
{

    public Image crossHair;
    private PlayerInput playerInput;
    private UIManager uiManager;
    private CameraShake cameraShake;
    private GameManager gameManager;

    [SerializeField] private LayerMask layerMask;
    public SpeedState speedState;
    public Transform shipMesh;
    public Transform rayOrigin;
    public GameObject AimerCubeTest;
    public float engineMomentum;
    public float currentEngineMomentum;
    public float maxEngineMomentum;
    public float boostEngineMomentum;
    [SerializeField] private float speedLerp;
    [SerializeField] private float engineMomentumChangeLerp;

    [System.NonSerialized] public float inputTurn;
    [System.NonSerialized] public float inputPitch;
    [System.NonSerialized] public float inputRoll;

    private float rollMesh;
    private float rollShip;
    private float yaw;
    private float pitch; 

    public float turnSpeed;
    public float rotateSpeed;
    public float pitchSpeed;

    public float smoothPitch;
    public float smoothYaw;
    public float smoothRoll;

    Vector3 velocity;
    private Rigidbody rb;
    Transform mT;

    [SerializeField] float rayDistance;


    private float current;
    private float lerpValue;
    [SerializeField] private float smoothTime;
    bool haveStoppedInput;

    public bool alive;
    public int health;
    [SerializeField] private bool sideSpinning;
    [SerializeField] private float spinSpeed;
    private float currentSpinSpeed;
    private float outputSpinSpeed;

    public GameObject shipNormal, shipDestroyed;
    //public GameObject[] shipParts;
    [SerializeField] private Camera cam;

    [SerializeField] private float maxEnergy;
    private float currentEnergy;
    [SerializeField] private float regenRate;
    private float newRegenTime;
    /*[System.NonSerialized] */public bool haveReachedLimit;


    private void Awake()
    {
        currentEnergy = maxEnergy;
        shipDestroyed.SetActive(false);
        shipNormal.SetActive(true);
        alive = true;
        mT = transform;
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        uiManager = FindObjectOfType<UIManager>();
        cameraShake = FindObjectOfType<CameraShake>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        //default engine start
        engineMomentum = maxEngineMomentum * 0.75f;
        currentEngineMomentum = maxEngineMomentum * 0.75f;
    }

    void FixedUpdate() 
    {
        if (alive && gameManager.gameState == GameState.Playing)
        {
            ForwardMomentum(speedState);
            ShipDirection(inputTurn, inputPitch, inputRoll);
        }
        else if (!alive)
        {
            Death();
            rb.velocity = new Vector3(0, 0, 0);
        }
        else if (gameManager.gameState == GameState.Restart || gameManager.gameState == GameState.Start)
        {
            rb.velocity = new Vector3(0, 0, 0);
        }
    }

    private void Update()
    {
        Aimer();
        EnergyRegenerate();
    }

    private void Death()
    {
        gameManager.GameStateDead();

        if (alive)
        {
            alive = false;

            shipDestroyed.SetActive(true);
            shipNormal.SetActive(false);
        }
    }

    public void EnergyRegenerate()
    {
        if (newRegenTime <= Time.time)
        {
            newRegenTime = Time.time + regenRate;

            if (playerInput.isBoosting && currentEnergy > 0)
                currentEnergy--;
            else if (!playerInput.isBoosting && currentEnergy < maxEnergy)
                currentEnergy++;
        }

        if (currentEnergy == 0)
        {
            haveReachedLimit = true;
            uiManager.BoostMeterRed();
        }

        if (haveReachedLimit && currentEnergy > maxEnergy / 2)
        {
            haveReachedLimit = false;
            uiManager.BoostMeterWhite();
        }
    }

    public float EnergyPercent()
    {
        float percent = currentEnergy / maxEnergy;
        return percent;
    }

    private void ForwardMomentum(SpeedState speedState)
    {
        rb.velocity = transform.forward * engineMomentum;

        if (speedState == SpeedState.normal)
        {
            engineMomentum = Mathf.Lerp(engineMomentum, currentEngineMomentum, speedLerp);
        }
        else if (speedState == SpeedState.boost)
        {
            engineMomentum = Mathf.Lerp(engineMomentum, boostEngineMomentum, speedLerp);
        }
    }

    public void ChangeMomentum(float newValue)
    {
        if (speedState == SpeedState.normal)
        {
            if (currentEngineMomentum >= 0 && currentEngineMomentum < maxEngineMomentum)
            {
                currentEngineMomentum = Mathf.Lerp(currentEngineMomentum, currentEngineMomentum += newValue, engineMomentumChangeLerp);
            }
            else if (currentEngineMomentum >= maxEngineMomentum)
            {
                currentEngineMomentum = maxEngineMomentum - 1;
            }
            else if (currentEngineMomentum <= 0)
            {
                currentEngineMomentum = 0.5f;
            }
        }

    }

    private void ShipDirection(float inputH, float inputV, float inputR)
    {
        if (sideSpinning)
        {
            rb.velocity += transform.right * outputSpinSpeed;
            //rb.velocity = new Vector3 (rb.velocity.x * outputSpinSpeed, rb.velocity.y, rb.velocity.z);
        }
        else
        {
            yaw = inputH * turnSpeed;
            pitch = inputV * pitchSpeed;
            rollMesh = inputH * rotateSpeed;
            rollShip = inputR * rotateSpeed;


            lerpValue = Mathf.Lerp(current, -rollMesh, smoothTime);
            current = lerpValue;
            shipMesh.localRotation = Quaternion.Euler(shipMesh.localRotation.x, shipMesh.localRotation.y, current);

            float lerpPitch = Mathf.LerpAngle(mT.rotation.x, pitch, smoothPitch);
            float lerpYaw = Mathf.LerpAngle(transform.rotation.y, yaw, smoothYaw);
            float lerpRoll = Mathf.LerpAngle(mT.rotation.z, rollShip, smoothTime);


            if (inputH != 0)
            {
                mT.Rotate(0, lerpYaw, 0);
            }
            else
            {
                mT.Rotate(0, 0, 0);
            }

            if (inputV != 0)
            {
                mT.Rotate(lerpPitch, 0, 0);
            }
            else
            {
                mT.Rotate(0, 0, 0);
            }


            if (inputR != 0)
            {
                mT.Rotate(0, 0, lerpRoll);

            }
            else
            {
                mT.Rotate(0, 0, 0);
            }
        }
    }

    public void Aimer()
    {
        Ray ray = new Ray(rayOrigin.position, transform.forward);
        RaycastHit hitInfo;

        //Vector3 aimEndPos = transform.forward * rayDistance;
        //Vector3 crossHairPos = new Vector3(aimEndPos.x, aimEndPos.y, 0);

        if (Physics.Raycast(ray, out hitInfo, rayDistance, layerMask))
        {
            print(hitInfo.collider.gameObject.layer);

            if (hitInfo.collider.gameObject.layer == 13 || hitInfo.collider.gameObject.layer == 10)
            {
                AimerCubeTest.transform.position = hitInfo.point;
                print("hitting ship or enemy");
            }
            else
            {
                print("hitting something else");
                AimerCubeTest.transform.position = ray.origin + ray.direction * rayDistance;
            }

            //AimerCubeTest.transform.position = hitInfo.point;
            //print(hitInfo.collider.gameObject.name);
            //Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 2);
        }
        else
        {
            print("hitting nothing");
            AimerCubeTest.transform.position = ray.origin + ray.direction * rayDistance;
            //Debug.DrawLine(ray.origin, ray.origin + ray.direction * rayDistance, Color.green, 2);
        }

        Vector2 aimWorldPos = cam.WorldToScreenPoint(AimerCubeTest.transform.position);
        crossHair.rectTransform.position = new Vector2(aimWorldPos.x, aimWorldPos.y);
    }

    IEnumerator SideSpin()
    {
        sideSpinning = true;
        yield return new WaitForSeconds(0.35f);
        sideSpinning = false;
    }

    public void SpinShip(bool isRight)
    {
        if (!sideSpinning)
        {
            if (isRight)
                outputSpinSpeed = -spinSpeed;
            else
                outputSpinSpeed = spinSpeed;

            StartCoroutine(SideSpin());
        }
    }

    public void DamagePlayer()
    {
        health -= 1;
        //print("player hit");
        if (health <= 0)
        {
            Death();
        }
        else
        {
            cameraShake.CallCamShake(0.6f, 0.18f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 13)
        {
            Death();
        }
        else if (collision.gameObject.layer == 10)
        {
            Death();
        }
    }


}
