using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    public GameObject player;
    public GameObject enemyMain;
    private EnemyShoot enemyShoot;
    private bool haveAdded;

    private void Awake()
    {
        enemyShoot = GetComponentInParent<EnemyShoot>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9 && !haveAdded)
        {
            player = other.gameObject.transform.parent.gameObject;
            haveAdded = true;

            //if (other.gameObject.GetComponentInParent<PlayerShoot>())
            //{
            //    //send to list
            //    //other.gameObject.GetComponentInParent<PlayerShoot>().enemyInRange.Add(enemyMain.gameObject);
            //    //other.gameObject.GetComponentInParent<PlayerShoot>().SetEnemyUIPoint(enemyMain);
            //    //haveAdded = true;
            //}

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9 && haveAdded)
        {
            //other.gameObject.GetComponentInParent<PlayerShoot>().enemyInRange.Remove(enemyMain.gameObject);
            //enemyShoot.hasUiPointer = false;
            player = null;
            haveAdded = false;
        }
    }




}
