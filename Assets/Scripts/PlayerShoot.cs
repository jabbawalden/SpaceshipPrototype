using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform origin1, origin2;
    [SerializeField] private float projSpeed;
    [SerializeField] private float fireRate;
    [SerializeField] private float newTime;
    [SerializeField] private float regenRate;
    [SerializeField] private float newRegenTime; 
    Vector3 direction;
    [SerializeField] float maxHeat;
    float currentHeat;

    PlayerInput playerInput;

    private void Start()
    {
        playerInput = FindObjectOfType<PlayerInput>();
    }

    private void Update()
    {
        if (newRegenTime <= Time.time)
        {
            newRegenTime = Time.time + regenRate;
            ReduceHeat();
        }
    }

    public void ShootProjectile(float shipMomentum, Vector3 direction)
    {
        if (newTime <= Time.time && currentHeat < maxHeat)
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

            currentHeat++;
        }
    }

    public void ReduceHeat()
    {
        if (!playerInput.isShooting && currentHeat > 0)
            currentHeat--;
    }

    public float HeatPercent()
    {
        float percent = currentHeat / maxHeat;
        return percent;
    }
}
