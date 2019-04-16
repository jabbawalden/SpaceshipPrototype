using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public float projectileSpeed;
    public float currentMomentum;
    public float newSpeed;
    Rigidbody rb;
    private GameObject playerShip;
    public float damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerShip = GameObject.Find("ShipMover");
    }

    private void Start()
    {
        Invoke("DestroyObject", 6);
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
            Destroy(other.gameObject);
    }
    /*
    private void Start()
    {
        newSpeed = projectileSpeed + currentMomentum;
        transform.rotation = Quaternion.Euler(playerShip.transform.rotation.x, playerShip.transform.rotation.y, playerShip.transform.rotation.z); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //rb.velocity = Vector3.forward * newSpeed;
    }
    */
}
