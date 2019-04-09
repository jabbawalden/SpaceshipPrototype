using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform origin1, origin2;
    [SerializeField] private float projSpeed;
    Vector3 direction;

    private void Update()
    {
        
        if (Physics.Raycast(origin1.position, transform.forward, 500))
            Debug.DrawRay(origin1.position, transform.forward);
        
        /*
        Physics.Raycast(origin1.position, transform.forward, 5000);
        Debug.DrawRay(origin1.position, transform.forward);
        */
    }

    public void ShootProjectile(float shipMomentum)
    {
        GameObject obj = Instantiate(projectile, origin1.position, Quaternion.identity);
        //vector3 is world space, use transform forward instead to get objects forward rotation
        direction = transform.forward;
        float newSpeed = projSpeed + shipMomentum;

        obj.GetComponent<Rigidbody>().velocity = direction * newSpeed * Time.deltaTime;
        obj.transform.rotation = Quaternion.LookRotation(transform.forward);

        print("Shot fired");  
    }
}
