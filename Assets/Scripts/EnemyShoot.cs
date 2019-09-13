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
    Vector3 direction;

    private void Awake()
    {
        detectPlayer = GetComponentInChildren<DetectPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (detectPlayer.player)
        {
            direction = transform.position - detectPlayer.player.transform.position;
            ShootProjectile(direction);
        }

    }


    public void ShootProjectile(Vector3 direction)
    {
        if (newTime <= Time.time)
        {
            newTime = Time.time + fireRate;

            GameObject obj1 = Instantiate(projectile, origin1.position, Quaternion.identity);

            //vector3 is world space, use transform forward instead to get objects forward rotation
            //direction = transform.forward;
            float newSpeed = projSpeed;

            obj1.GetComponent<Rigidbody>().velocity = direction * newSpeed * Time.deltaTime;
            obj1.transform.rotation = Quaternion.LookRotation(transform.forward);


        }

    }
}
