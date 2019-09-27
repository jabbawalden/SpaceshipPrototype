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
    private bool haveHit;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == targetLayer && targetLayer == 10)
        {
            if (other.gameObject.GetComponentInParent<EnemyShoot>())
            {
                other.gameObject.GetComponentInParent<EnemyShoot>().DestroyEnemy();
                print("Destroy enemy");
                Destroy(gameObject);
            }
        }
        else if (other.gameObject.layer == targetLayer && targetLayer == 9)
        {
            //print("Hit Player");
            if (!haveHit && playerController)
            {
                print("projectile hits player");
                playerController.DamagePlayer();
                haveHit = true;
                Destroy(gameObject);
            }
        }
        else if (other.gameObject.layer == 17 && targetLayer == 10)
        {
            other.GetComponent<DoorHolder>().BreakDoors();
        }
        else if (other.gameObject.layer == 13)
        {
            Destroy(gameObject);
        }


    }

}
