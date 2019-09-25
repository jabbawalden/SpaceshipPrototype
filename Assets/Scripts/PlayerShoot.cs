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
    [SerializeField] private float currentHeat;

    private PlayerInput playerInput;
    private UIManager uiManager;
    [System.NonSerialized] public bool haveReachedLimit;

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        playerInput = FindObjectOfType<PlayerInput>();
        currentHeat = maxHeat;
    }

    private void Update()
    {
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
}
