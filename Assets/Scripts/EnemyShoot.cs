using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    private DetectPlayer detectPlayer;

    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform origin1;
    [SerializeField] private float projSpeed;
    [SerializeField] private float fireRate;
    [SerializeField] private float newTime;
    float min, max;

    public GameObject[] enemyMesh;
    private bool isAlive;
    private Vector3 playerDirection;
    private Vector3 targetDirection;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] bool playerInView;
    [SerializeField] private GameObject target;

    private void Awake()
    {
        target = GameObject.Find("ShipMover");
        isAlive = true;
        detectPlayer = GetComponentInChildren<DetectPlayer>();
        float r = Random.Range(0, 8);
        max = r;
        min = -r;
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (detectPlayer.player && isAlive)
        {
            ShootProjectile();
        }
    }

    private void FixedUpdate()
    {
        targetDirection = target.transform.position - transform.position;
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
            if (hit.collider)
                playerInView = false;
        }
        else
        {
            playerInView = true;
        }

        Debug.DrawRay(origin1.position, targetDirection);
    }
    

    public void DestroyEnemy()
    {
        foreach(GameObject mesh in enemyMesh)
        {
            mesh.GetComponent<Rigidbody>().isKinematic = false;
        }
        isAlive = false;
    }
}
