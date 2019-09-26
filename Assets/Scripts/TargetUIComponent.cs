using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetUIComponent : MonoBehaviour
{
    public bool isActive;
    public bool isVisible;
    public bool haveSpawned;
    public bool haveAdded;
    public bool playerCanSeeUs;

    PlayerShoot playerShoot;

    private void Update()
    {
        if (isVisible && !haveAdded)
        {
            playerShoot = FindObjectOfType<PlayerShoot>();
            playerShoot.enemyInRange.Add(gameObject);
            haveAdded = true;
        }
        else if (!isVisible && haveAdded)
        {
            haveAdded = false;
            if (playerShoot)
                playerShoot.enemyInRange.Remove(gameObject);
        }
    }

    private void OnBecameVisible()
    {
        playerCanSeeUs = true;
    }

    private void OnBecameInvisible()
    {
        playerCanSeeUs = false;
    }

}
