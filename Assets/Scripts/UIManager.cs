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
    //private GameObject shipMover;

    [SerializeField] private Color overHeat;

    [SerializeField] private GameObject innerCircle;
    [SerializeField] private GameObject outerCircle;
    [SerializeField] private GameObject uiPivots;
    [SerializeField] private float innerRotationDivider;
    [SerializeField] private float outerRotationDivider;
    [SerializeField] private float uiPivotMultiplier; 

    bool rotateRoll;
    bool haveRotatedRoll;

    float currentLerp;
    [SerializeField] float lerpTime;

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
        //float rollValue = Mathf.Clamp(playerInput.rKey, -5, 5);

        Quaternion innerRot = new Quaternion(playerController.shipMesh.localRotation.x / innerRotationDivider, playerController.shipMesh.localRotation.y / innerRotationDivider, playerController.shipMesh.localRotation.z / innerRotationDivider, playerController.shipMesh.localRotation.w);
        Quaternion outerRot = new Quaternion(playerController.shipMesh.localRotation.x / outerRotationDivider, playerController.shipMesh.localRotation.y / outerRotationDivider, playerController.shipMesh.localRotation.z / outerRotationDivider, playerController.shipMesh.localRotation.w);


        float lerpVKey = Mathf.Lerp(currentLerp, -playerInput.vKey * uiPivotMultiplier, lerpTime);
        currentLerp = lerpVKey;

        innerCircle.transform.DORotateQuaternion(innerRot, 0.05f);
        outerCircle.transform.DORotateQuaternion(outerRot, 0.05f);


        if (playerInput.vKey == 0 && playerInput.vKey2 == 0)
        {
            //calculate primary pitch 
            uiPivots.GetComponent<RectTransform>().offsetMax = new Vector2(0, lerpVKey);
            uiPivots.GetComponent<RectTransform>().offsetMin = new Vector2(0, lerpVKey);
        }
        else if (playerInput.vKey != 0)
        {
            //continue calculate primary pitch 
            uiPivots.GetComponent<RectTransform>().offsetMax = new Vector2(0, lerpVKey);
            uiPivots.GetComponent<RectTransform>().offsetMin = new Vector2(0, lerpVKey);
        }
        else if (playerInput.vKey == 0 && playerInput.vKey2 != 0)
        {
            float lerpVKey2 = Mathf.Lerp(currentLerp, -playerInput.vKey2 * uiPivotMultiplier, lerpTime);
            currentLerp = lerpVKey2;
            uiPivots.GetComponent<RectTransform>().offsetMax = new Vector2(0, lerpVKey2);
            uiPivots.GetComponent<RectTransform>().offsetMin = new Vector2(0, lerpVKey2);
            //continue calculate secondary pitch 
        }


        print(uiPivots.GetComponent<RectTransform>().offsetMin);
        //uiPivots.GetComponent<RectTransform>().position = new Vector3(uiPivots.GetComponent<RectTransform>().position.x, 0, uiPivots.GetComponent<RectTransform>().position.z);

        /*(uiPivots.transform.position.y + -playerInput.vKey * 50, 0.05f);*/
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
