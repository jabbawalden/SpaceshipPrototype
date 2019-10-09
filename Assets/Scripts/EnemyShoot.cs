using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    private DetectPlayer detectPlayer;
    private UIManager uiManager;
    private TargetUIComponent targetUIComp;
    private GameManager gameManager;

    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform origin1;
    [SerializeField] private float projSpeed;
    [SerializeField] private float fireRate;
    [SerializeField] private float newTime;
    float min, max;

    public GameObject[] enemyMesh;
    public bool isAlive;
    private Vector3 playerDirection;
    private Vector3 targetDirection;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] bool playerInView;
    [SerializeField] private GameObject target;

    //public bool hasUiPointer;
    //public bool isVisible;
    //public bool haveSpawnedVisible;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        targetUIComp = GetComponent<TargetUIComponent>();
        uiManager = FindObjectOfType<UIManager>();
        target = GameObject.Find("ShipMover");
        isAlive = true;
        detectPlayer = GetComponentInChildren<DetectPlayer>();
        float r = Random.Range(2, 7);
        max = r;
        min = -r;
    }

    private void Start()
    {
        targetUIComp.isActive = true;
        uiManager.AddTurretTotal();    
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameState == GameState.Playing)
        {
            if (detectPlayer.player && isAlive)
            {
                ShootProjectile();
            }
            else if (!isAlive)
            {
                targetUIComp.isActive = false;
            }
            targetDirection = target.transform.position - origin1.transform.position;
        }
    }

    private void FixedUpdate()
    {
        PlayerView();
    }

    public void ShootProjectile() 
    {
        if (newTime <= Time.time)
        {
            newTime = Time.time + fireRate;

            float rX = Random.Range(min, max);
            float rY = Random.Range(min, max);
            float rZ = Random.Range(min, max);

            Vector3 targetPosition = new Vector3(detectPlayer.player.transform.position.x + rX, detectPlayer.player.transform.position.y + rY, detectPlayer.player.transform.position.z + rZ);
            playerDirection = targetPosition - transform.position;
            playerDirection = playerDirection.normalized;

            //Ray ray = new Ray(origin1.transform.position, direction);

            if (playerInView)
            {
                GameObject obj1 = Instantiate(projectile, origin1.position, Quaternion.identity);

                //vector3 is world space, use transform forward instead to get objects forward rotation
                //direction = transform.forward;
                float newSpeed = projSpeed;
                obj1.GetComponent<Rigidbody>().velocity = playerDirection * newSpeed * Time.deltaTime;
                obj1.transform.rotation = Quaternion.LookRotation(transform.forward);
            }
        }
    }

    public void PlayerView()
    {
        RaycastHit hit;

        if (Physics.Raycast(origin1.position, targetDirection, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.gameObject.layer == 9)
            {
                playerInView = true;
                targetUIComp.isVisible = true;
            }
            else
            {
                playerInView = false;
                targetUIComp.isVisible = false;
            }
        }
        else
        {
            playerInView = false;
        }

        //Debug.DrawRay(origin1.position, targetDirection);
    }
    

    public void DestroyEnemy()
    {
        if (isAlive)
        {
            foreach (GameObject mesh in enemyMesh)
            {
                mesh.GetComponent<Rigidbody>().isKinematic = false;
                mesh.layer = 14;
            }
            uiManager.AddTurretKillCount();
            uiManager.UIHitColorSize();
            isAlive = false;
        }

    }

    //check if cam can see them while player is true
    private void OnBecameVisible()
    {
        //isVisible = true;
    }

    private void OnBecameInvisible()
    {
        //isVisible = false;
        //haveSpawnedVisible = false;
    }
}
