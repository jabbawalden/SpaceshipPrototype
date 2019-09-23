using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; 

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider boostMeter;
    [SerializeField] private Slider shootMeter;
    [SerializeField] private Image shootFill;
    [SerializeField] private Image boostFill;

    private PlayerController playerController;
    private PlayerInput playerInput;
    private PlayerShoot playerShoot;

    [SerializeField] private Color overHeat;

    [SerializeField] private GameObject innerCircle;
    [SerializeField] private GameObject outerCircle;
    [SerializeField] private GameObject uiPivots;
    [SerializeField] private float innerRotationDivider;
    [SerializeField] private float outerRotationDivider;
    [SerializeField] private float uiRotationDivider; 

    bool rotateRoll;
    bool haveRotatedRoll;

    private void Awake()
    {

        playerInput = FindObjectOfType<PlayerInput>();
        playerController = FindObjectOfType<PlayerController>();
        playerShoot = FindObjectOfType<PlayerShoot>();
    }

    // Update is called once per frame
    void Update()
    {
        BoostMeter();
        ShootingMeter();

        //print(playerController.shipMesh.localRotation.z);

        //example.transform.DORotate(new Vector3(0, 0, playerController.shipMesh.localRotation.x * 50), 0.5f, RotateMode.LocalAxisAdd);
        //print(playerController.shipMesh.rotation.x * 50);
        float rollValue = Mathf.Clamp(playerInput.rKey, -5, 5);

        Quaternion innerRot = new Quaternion(playerController.shipMesh.localRotation.x / innerRotationDivider, playerController.shipMesh.localRotation.y / innerRotationDivider, playerController.shipMesh.localRotation.z / innerRotationDivider, playerController.shipMesh.localRotation.w);
        Quaternion outerRot = new Quaternion(playerController.shipMesh.localRotation.x / outerRotationDivider, playerController.shipMesh.localRotation.y / outerRotationDivider, playerController.shipMesh.localRotation.z / outerRotationDivider, playerController.shipMesh.localRotation.w);
        Quaternion uiRot = new Quaternion(uiPivots.transform.localRotation.x, uiPivots.transform.localRotation.y, rollValue, uiPivots.transform.localRotation.w);
        print(uiRot);

        innerCircle.transform.DORotateQuaternion(innerRot, 0.05f);
        outerCircle.transform.DORotateQuaternion(outerRot, 0.05f);
        uiPivots.transform.DORotateQuaternion(uiRot, 0.1f);

        //if (!rotateRoll)
        //{
        //    if (playerInput.rKey < 0)
        //    {
        //        rotateRoll = true;
        //        print("rKey has value");
        //        if (!haveRotatedRoll)
        //        {
        //            example.transform.DORotate(new Vector3(0, 0, -35), 1, RotateMode.LocalAxisAdd);
        //            haveRotatedRoll = true;
        //            print("start rotation");
        //        }
        //    }
        //    else if (playerInput.rKey > 0)
        //    {
        //        rotateRoll = true;
        //        print("rKey has value");
        //        if (!haveRotatedRoll)
        //        {
        //            example.transform.DORotate(new Vector3(0, 0, 35), 1, RotateMode.LocalAxisAdd);
        //            haveRotatedRoll = true;
        //            print("start rotation");
        //        }
        //    }
        //    else if (playerInput.rKey == 0)
        //    {
        //        haveRotatedRoll = false;
        //        rotateRoll = false;
        //    }
        //}
    }

    private void BoostMeter()
    {
        boostMeter.value = playerController.EnergyPercent();
    }

    private void ShootingMeter()
    {
        shootMeter.value = playerShoot.HeatPercent();
    }

    public void ShootMeterColorRed()
    {
        shootFill.color = overHeat;
    }

    public void ShootMeterColorWhite() 
    {
        shootFill.color = Color.white;
    }

    public void BoostMeterRed()
    {
        boostFill.color = overHeat;
    }

    public void BoostMeterWhite()
    {
        boostFill.color = Color.white;
    }
}
