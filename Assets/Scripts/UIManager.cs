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
    [SerializeField] private Slider lifeMeter;
    [SerializeField] private Image shootFill;
    [SerializeField] private Image boostFill;
    [SerializeField] private Image sideBars;
    [SerializeField] private Image lifeDots; 

    private PlayerController playerController;
    private PlayerInput playerInput;
    private PlayerShoot playerShoot;
    private GameManager gameManager;
    //private GameObject shipMover;

    [SerializeField] private Color redColor;
    [SerializeField] private Color whiteColor;

    [SerializeField] private GameObject innerCircle;
    [SerializeField] private GameObject outerCircle;
    [SerializeField] private GameObject uiPivot;
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
    [SerializeField] private TextMeshProUGUI missionState;
    [SerializeField] private GameObject endMissionPanel;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject startPanel;

    Vector2 originalPos;
    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        playerController = FindObjectOfType<PlayerController>();
        playerShoot = FindObjectOfType<PlayerShoot>();
        gameManager = FindObjectOfType<GameManager>();

        mission1.SetActive(false);
        mission1.GetComponent<RectTransform>().transform.localScale = startingScale;

        endMissionPanel.SetActive(false);
        mainPanel.SetActive(false);
    }

    private void Start()
    {
        originalPos = uiPivot.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        BoostMeter();
        ShootingMeter();
        LifeMeter();
        RotateUI();
    }

    public void PlayGame()
    {
        FadeOutStartMissionPanel();
        mainPanel.SetActive(true);
        StartCoroutine(SetMissionInstructions());
    }

    private void RotateUI()
    {
        Quaternion innerRot = new Quaternion(playerController.shipMesh.transform.localRotation.x / innerRotationDivider, playerController.shipMesh.transform.localRotation.y / innerRotationDivider, playerController.shipMesh.transform.localRotation.z / innerRotationDivider, playerController.shipMesh.transform.localRotation.w);
        Quaternion outerRot = new Quaternion(playerController.shipMesh.transform.localRotation.x / outerRotationDivider, playerController.shipMesh.transform.localRotation.y / outerRotationDivider, playerController.shipMesh.transform.localRotation.z / outerRotationDivider, playerController.shipMesh.transform.localRotation.w);

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
            uiPivot.GetComponent<RectTransform>().offsetMax = new Vector2(0, lerpVKey);
            uiPivot.GetComponent<RectTransform>().offsetMin = new Vector2(0, lerpVKey);
        }
        else if (playerInput.vKey != 0)
        {
            //continue calculate primary pitch 
            uiPivot.GetComponent<RectTransform>().offsetMax = new Vector2(0, lerpVKey);
            uiPivot.GetComponent<RectTransform>().offsetMin = new Vector2(0, lerpVKey);
        }
        else if (playerInput.vKey == 0 && playerInput.vKey2 != 0)
        {
            float lerpVKey2 = Mathf.Lerp(currentLerp, -playerInput.vKey2 * uiPivotMultiplier, lerpTime);
            currentLerp = lerpVKey2;
            uiPivot.GetComponent<RectTransform>().offsetMax = new Vector2(0, lerpVKey2);
            uiPivot.GetComponent<RectTransform>().offsetMin = new Vector2(0, lerpVKey2);
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

    private void LifeMeter()
    {
        lifeMeter.value = playerController.LifePercent();

        /*
        switch(playerController.currentHealth)
        {
            case 3:
                lifeMeter.value = 1;
                break;
            case 2:
                lifeMeter.value = 0.66f;
                break;
            case 1:
                lifeMeter.value = 0.33f;
                break;
        }
        */
    }

    public void ShootMeterColorRed()
    {
        shootFill.color = redColor;
    }

    public void ShootMeterColorWhite() 
    {
        shootFill.color = Color.white;
    }

    public void BoostMeterRed()
    {
        boostFill.color = redColor;
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
        if (turretsKilledCount >= turretsTotalCount)
        {
            gameManager.GameStateComplete();
        }
    }

    public void SetEndMissionPanel(bool complete)
    {
        endMissionPanel.SetActive(true);
        mission1.SetActive(false);
        mainPanel.SetActive(false);
        //StartCoroutine(MissionStateTextSize());

        if (complete)
        {
            missionState.text = "MISSION: SUCCESSFUL";
        }
        else
        {
            missionState.text = "MISSION: CRITICAL FAILURE";
        }
    }

    //IEnumerator MissionStateTextSize()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    missionState.GetComponent<RectTransform>().DOScale(new Vector3(1, 1, 1), 2f);
    //}

    public void FadeOutStartMissionPanel()
    {
        startPanel.GetComponent<Animator>().SetBool("GameStarted", true);
        StartCoroutine(AnimationTimer());
    }

    IEnumerator AnimationTimer()
    {
        yield return new WaitForSeconds(4f);
        startPanel.SetActive(false);
    }

    public void LifeMeterBop()
    {
        StartCoroutine(LifeBop());
    }

    IEnumerator LifeBop()
    {
        lifeMeter.GetComponent<RectTransform>().DOScale(new Vector3(1.4f, 0.6f, 1), 0.2f);
        yield return new WaitForSeconds(0.2f);
        lifeMeter.GetComponent<RectTransform>().DOScale(new Vector3(1.1f, 0.5f, 1), 0.3f);
    }

    public void UIDamageColor()
    {
        StartCoroutine(UIColorSequence());
    }

    IEnumerator UIColorSequence()
    {
        sideBars.DOColor(redColor, 0.35f);
        lifeDots.DOColor(redColor, 0.35f);
        yield return new WaitForSeconds(0.4f);
        sideBars.DOColor(whiteColor, 0.4f);
        lifeDots.DOColor(whiteColor, 0.45f);
    }

    public void UIDamageShake(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            magnitude *= 0.965f;
            float x = Random.Range(-1, 1) * magnitude;
            float y = Random.Range(-1, 1) * magnitude;
            uiPivot.transform.position = new Vector2(x + originalPos.x, y + originalPos.y);
            elapsed += Time.deltaTime;
            //wait until the next frame starts before updating
            yield return null;
        }

        uiPivot.transform.position = originalPos;
    }
}
