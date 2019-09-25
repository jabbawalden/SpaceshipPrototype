using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

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
    float currentLerp2;
    [SerializeField] float lerpTime;

    [SerializeField] private GameObject mission1;
    [SerializeField] private Vector3 startingScale;
    [SerializeField] private Vector3 endingScale; 
    [SerializeField] private GameObject mp1FinalLoc;

    public int turretsKilledCount;
    public int turretsTotalCount;
    [SerializeField] private TextMeshProUGUI turretsTotal, turretsKilled;

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        playerController = FindObjectOfType<PlayerController>();
        playerShoot = FindObjectOfType<PlayerShoot>();

        mission1.SetActive(false);

        mission1.GetComponent<RectTransform>().transform.localScale = startingScale;
    }

    private void Start()
    {
        StartCoroutine(SetMissionInstructions());
    }

    // Update is called once per frame
    void Update()
    {
        BoostMeter();
        ShootingMeter();
        RotateUI();
    }

    private void RotateUI()
    {
        Quaternion innerRot = new Quaternion(playerController.shipMesh.localRotation.x / innerRotationDivider, playerController.shipMesh.localRotation.y / innerRotationDivider, playerController.shipMesh.localRotation.z / innerRotationDivider, playerController.shipMesh.localRotation.w);
        Quaternion outerRot = new Quaternion(playerController.shipMesh.localRotation.x / outerRotationDivider, playerController.shipMesh.localRotation.y / outerRotationDivider, playerController.shipMesh.localRotation.z / outerRotationDivider, playerController.shipMesh.localRotation.w);

        float lerpVKey = Mathf.Lerp(currentLerp, -playerInput.vKey * uiPivotMultiplier, lerpTime);
        currentLerp = lerpVKey;

        float lerpRKey = Mathf.Lerp(currentLerp, -playerInput.rKey * 5, lerpTime);
        currentLerp2 = lerpRKey;

        innerCircle.transform.DORotateQuaternion(innerRot, 0.05f);
        outerCircle.transform.DORotateQuaternion(outerRot, 0.05f);

        //for up and down movements
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
    }

    private void MissionTextSet(GameObject missionComp, GameObject finalLoc)
    {
        missionComp.GetComponent<RectTransform>().DOScale(endingScale, 0.9f);
        missionComp.GetComponent<RectTransform>().DOLocalMove(finalLoc.transform.localPosition, 0.9f);
    }

    private IEnumerator SetMissionInstructions()
    {
        yield return new WaitForSeconds(0.5f);
        mission1.SetActive(true);

        yield return new WaitForSeconds(0.4f);
        MissionTextSet(mission1, mp1FinalLoc);
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

    public void AddTurretKillCount()
    {
        turretsKilledCount += 1;
        turretsKilled.text = turretsKilledCount.ToString();
    }

    public void AddTurretTotal()
    {
        turretsTotalCount += 1;
        turretsTotal.text = turretsTotalCount.ToString();
    }
}
