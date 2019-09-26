using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPointUI : MonoBehaviour
{
    public GameObject targetRef;
    private Camera cam;
    [SerializeField] private float rotateSpeed;


    private void Start()
    {
        cam = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        //if (targetRef.GetComponent<EnemyShoot>())
        //{
        //    if (targetRef.GetComponent<EnemyShoot>().isAlive)
        //    {
        //        if (targetRef.GetComponent<EnemyShoot>().hasUiPointer && targetRef.GetComponent<EnemyShoot>().isVisible)
        //        {
        //            Vector2 aimWorldPos = cam.WorldToScreenPoint(targetRef.transform.position);
        //            transform.position = new Vector2(aimWorldPos.x, aimWorldPos.y);

        //            transform.Rotate(0, 0, rotateSpeed);
        //        }
        //        else
        //        {
        //            Destroy(this.gameObject);
        //        }
        //    }
        //    else
        //    {
        //        Destroy(this.gameObject);
        //    }
        //}

        if (targetRef)
        {
            if (targetRef.GetComponent<TargetUIComponent>())
            {
                if (targetRef.GetComponent<TargetUIComponent>().isActive && targetRef.GetComponent<TargetUIComponent>().isVisible && targetRef.GetComponent<TargetUIComponent>().playerCanSeeUs)
                {
                    Vector2 aimWorldPos = cam.WorldToScreenPoint(targetRef.transform.position);
                    transform.position = new Vector2(aimWorldPos.x, aimWorldPos.y);

                    transform.Rotate(0, 0, rotateSpeed);
                }
                else
                {
                    targetRef.GetComponent<TargetUIComponent>().haveSpawned = false;
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }



    }
}
