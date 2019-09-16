using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    DetectPlayer detectPlayer;

    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform origin1;
    [SerializeField] private float projSpeed;
    [SerializeField] private float fireRate;
    [SerializeField] private float newTime;
    float min, max;

    public GameObject[] enemyMesh;
    private bool isAlive;

    private void Awake()
    {
        isAlive = true;
        detectPlayer = GetComponentInChildren<DetectPlayer>();
        float r = Random.Range(0, 8);
        max = r;
        min = -r;
    }

    // Update is called once per frame
    void Update()
    {
        if (detectPlayer.player && isAlive)
        {
            ShootProjectile();
        }
    }

    public void ShootProjectile()
    {


        if (newTime <= Time.time)
        {
            float rX = Random.Range(min, max);
            float rY = Random.Range(min, max);
            float rZ = Random.Range(min, max);

            Vector3 targetPosition = new Vector3(detectPlayer.player.transform.position.x + rX, detectPlayer.player.transform.position.y + rY, detectPlayer.player.transform.position.z + rZ);
            Vector3 direction = targetPosition - transform.position;

            newTime = Time.time + fireRate;

            //Ray ray = new Ray(origin1.transform.position, direction);

            GameObject obj1 = Instantiate(projectile, origin1.position, Quaternion.identity);

            //vector3 is world space, use transform forward instead to get objects forward rotation
            //direction = transform.forward;
            float newSpeed = projSpeed;

            obj1.GetComponent<Rigidbody>().velocity = direction * newSpeed * Time.deltaTime;
            obj1.transform.rotation = Quaternion.LookRotation(transform.forward);
        }
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
