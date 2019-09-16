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
    [SerializeField] private int targetLayer;

    PlayerController playerController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerShip = GameObject.Find("ShipMover");
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        Invoke("DestroyObject", 5.5f);
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.layer == targetLayer)
    //    {
    //        print("Hit Player");

    //        if (targetLayer == 9 && playerController)
    //        {
    //            playerController.health -= 1;
    //            print("Player lose health");
    //            Destroy(gameObject);
    //        }

    //    }

    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == targetLayer && targetLayer == 10)
        {
            if (other.gameObject.GetComponentInParent<EnemyShoot>())
            {
                other.gameObject.GetComponentInParent<EnemyShoot>().DestroyEnemy();
                print("Destroy enemy");
            }
        }
        else if (other.gameObject.layer == targetLayer && targetLayer == 9)
        {
            print("Hit Player");

            if (targetLayer == 9 && playerController)
            {
                playerController.health -= 1;
                print("Player lose health");
                Destroy(gameObject);
            }
        }

        if (other.CompareTag("MainShip"))
        {
            print("Main Ship");
            Destroy(gameObject);
        }

    }

}
