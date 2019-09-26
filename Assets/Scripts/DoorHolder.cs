using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHolder : MonoBehaviour
{

    public GameObject doorWhole, doorBroken;
    public bool isVisible;
    public bool haveSpawnedVisible;
    public bool hasUiPointer;
    PlayerShoot playerShoot;

    private void Awake()
    {
        playerShoot = FindObjectOfType<PlayerShoot>();
    }

    private void Start()
    {
        doorBroken.SetActive(false);
    }

    public void BreakDoors()
    {
        doorWhole.SetActive(false);
        doorBroken.SetActive(true);
        Destroy(gameObject);
    }

    private void OnBecameVisible()
    {
        isVisible = true;
        if (!hasUiPointer && !haveSpawnedVisible)
        {
            playerShoot.enemyInRange.Add(this.gameObject);
            hasUiPointer = true;
            haveSpawnedVisible = true;
        }

    }

    private void OnBecameInvisible()
    {
        isVisible = false;
        haveSpawnedVisible = false;
        hasUiPointer = false;
        playerShoot.enemyInRange.Remove(this.gameObject);
    }
}
