using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPointUI : MonoBehaviour
{
    public GameObject enemyRef;
    private Camera cam;
    [SerializeField] private float rotateSpeed;

    private void Start()
    {
        cam = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        if (enemyRef.GetComponent<EnemyShoot>())
        {
            if (enemyRef.GetComponent<EnemyShoot>().isAlive)
            {
                if (enemyRef.GetComponent<EnemyShoot>().hasUiPointer && enemyRef.GetComponent<EnemyShoot>().isVisible)
                {
                    Vector2 aimWorldPos = cam.WorldToScreenPoint(enemyRef.transform.position);
                    transform.position = new Vector2(aimWorldPos.x, aimWorldPos.y);

                    transform.Rotate(0, 0, rotateSpeed);
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        else if (enemyRef.GetComponent<DoorHolder>())
        {
            if (enemyRef.GetComponent<DoorHolder>().hasUiPointer && enemyRef.GetComponent<DoorHolder>().isVisible)
            {
                Vector2 aimWorldPos = cam.WorldToScreenPoint(enemyRef.transform.position);
                transform.position = new Vector2(aimWorldPos.x, aimWorldPos.y);

                transform.Rotate(0, 0, rotateSpeed);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

    }
}
