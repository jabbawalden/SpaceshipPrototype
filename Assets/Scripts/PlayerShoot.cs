using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShoot : MonoBehaviour
{
    private PlayerInput playerInput;
    private UIManager uiManager;
    private GameManager gameManager;
    private PlayerController playerController;

    public Image crossHair;
    public Image enemyPointUI;
    public List<GameObject> enemyInRange = new List<GameObject>();
    public GameObject AimerObj;
    public GameObject uiCanvas;

    [SerializeField] private LayerMask layerMask;
    public Transform rayOrigin;
    [SerializeField] private Camera cam;
    [SerializeField] float rayDistance;

    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform origin1, origin2;
    [SerializeField] private float projSpeed;
    [SerializeField] private float fireRate;
    [SerializeField] private float newTime;
    [SerializeField] private float regenRate;
    [SerializeField] private float newRegenTime; 
    Vector3 direction;
    [SerializeField] float maxHeat;
    [SerializeField] private float currentHeat;


    [System.NonSerialized] public bool haveReachedLimit;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        uiManager = FindObjectOfType<UIManager>();
        playerInput = FindObjectOfType<PlayerInput>();
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        currentHeat = maxHeat;
    }

    private void Update()
    {
        if (gameManager.gameState == GameState.Playing)
        {
            Aimer();
            EnemySight();
        }

        if (newRegenTime <= Time.time)
        {
            newRegenTime = Time.time + regenRate;
            EnergyManagement();

            
            if (haveReachedLimit && currentHeat > 15)
            {
                haveReachedLimit = false;
                uiManager.ShootMeterColorWhite();
            }
        }
    }

    public void ShootProjectile(float shipMomentum, Vector3 direction)
    {
        if (newTime <= Time.time && currentHeat > 0)
        {
            newTime = Time.time + fireRate;

            GameObject obj1 = Instantiate(projectile, origin1.position, Quaternion.identity);
            GameObject obj2 = Instantiate(projectile, origin1.position, Quaternion.identity);

            //vector3 is world space, use transform forward instead to get objects forward rotation
            //direction = transform.forward;
            float newSpeed = projSpeed + shipMomentum;

            obj1.GetComponent<Rigidbody>().velocity = direction * newSpeed * Time.deltaTime;
            obj1.transform.rotation = Quaternion.LookRotation(transform.forward);
            obj2.GetComponent<Rigidbody>().velocity = direction * newSpeed * Time.deltaTime;
            obj2.transform.rotation = Quaternion.LookRotation(transform.forward);

            currentHeat--;
        }
    }

    public void EnergyManagement()
    {
        if (!playerInput.isShooting && currentHeat < maxHeat)
            currentHeat += 0.5f;

        if (currentHeat == 0)
            haveReachedLimit = true;

        if (haveReachedLimit)
        {
            uiManager.ShootMeterColorRed();
        }
    }

    public float HeatPercent()
    {
        float percent = currentHeat / maxHeat;
        return percent;
    }

    public void Aimer()
    {
        Ray ray = new Ray(rayOrigin.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, layerMask))
        {

            if (hit.collider.gameObject.layer == 13 || hit.collider.gameObject.layer == 10 || hit.collider.gameObject.layer == 16 || hit.collider.gameObject.layer == 17)
            {
                AimerObj.transform.position = hit.point;
            }
            else
            {
                AimerObj.transform.position = ray.origin + ray.direction * rayDistance;
            }
        }
        else
        {
            AimerObj.transform.position = ray.origin + ray.direction * rayDistance;
        }

        Vector2 aimWorldPos = cam.WorldToScreenPoint(AimerObj.transform.position);
        crossHair.rectTransform.position = new Vector2(aimWorldPos.x, aimWorldPos.y);
    }

    private void EnemySight()
    {
        foreach (GameObject obj in enemyInRange)
        {
            //if they are in sight do this
            if (obj)
            {
                Vector3 direction = obj.transform.position - transform.position;
                Ray ray = new Ray(rayOrigin.position, direction);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, rayDistance, layerMask))
                {
                    if (hit.collider.GetComponentInParent<TargetUIComponent>())
                    {
                        hit.collider.GetComponentInParent<TargetUIComponent>().isVisible = true;

                        if (!hit.collider.GetComponentInParent<TargetUIComponent>().haveSpawned && hit.collider.GetComponentInParent<TargetUIComponent>().playerCanSeeUs)
                        {
                            SetEnemyUIPoint(obj);
                            hit.collider.GetComponentInParent<TargetUIComponent>().haveSpawned = true;
                            print("spawn UI");
                            //spawn
                        }
                    }
                }
            }

        }
    }

    private void SetEnemyUIPoint(GameObject enemyObj)
    {
        Image enemyPoint = Instantiate(enemyPointUI);
        enemyPoint.transform.SetParent(uiCanvas.transform);

        if (enemyPoint.GetComponent<EnemyPointUI>())
        {
            enemyPoint.GetComponent<EnemyPointUI>().targetRef = enemyObj;

            if (playerController)
                enemyPoint.GetComponent<EnemyPointUI>().playerRef = playerController.shipMesh;

        }


    }
}
