using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHolder : MonoBehaviour
{
    public GameObject doorWhole, doorBroken;
    PlayerShoot playerShoot;
    TargetUIComponent targetUIComp;

    private void Awake()
    {
        playerShoot = FindObjectOfType<PlayerShoot>();
        targetUIComp = GetComponent<TargetUIComponent>();
    }

    private void Start()
    {
        doorBroken.SetActive(false);
        if (targetUIComp)
            targetUIComp.isActive = true;
    }

    public void BreakDoors()
    {
        doorWhole.SetActive(false);
        doorBroken.SetActive(true);
        if (targetUIComp)
            targetUIComp.isActive = false;
        Destroy(gameObject);
    }

    private void OnBecameVisible()
    {
        if (targetUIComp)
            targetUIComp.isVisible = true;
    }
    private void OnBecameInvisible()
    {
        if (targetUIComp)
            targetUIComp.isVisible = false;
    }

}
