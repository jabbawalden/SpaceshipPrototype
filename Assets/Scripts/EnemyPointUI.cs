using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPointUI : MonoBehaviour
{
    [SerializeField] private Image mainIcon;
    public GameObject targetRef;
    public GameObject playerRef;
    private Camera cam;
    [SerializeField] private float rotateSpeed;
    private GameManager gameManager;
    public float xClamp;
    public float yClamp;
    Vector3 screenCenter;
    public float angle;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        cam = FindObjectOfType<Camera>();
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

    }

    private void Update()
    {


        if (gameManager.gameState == GameState.Playing)
        {
            if (targetRef)
            {
                //Vector3 targetDir = targetRef.transform.position - playerRef.transform.position;
                //float otherAngle = Vector3.Angle(targetDir, playerRef.transform.forward);
                 
                if (targetRef.GetComponent<TargetUIComponent>())
                {
                    if (targetRef.GetComponent<TargetUIComponent>().isActive && targetRef.GetComponent<TargetUIComponent>().isVisible && targetRef.GetComponent<TargetUIComponent>().playerCanSeeUs)
                    {
                        Vector2 aimWorldPos = cam.WorldToScreenPoint(targetRef.transform.position);

                        //xClamp = Mathf.Clamp(aimWorldPos.x, 0, Screen.width);
                        //yClamp = Mathf.Clamp(aimWorldPos.y, 0, Screen.height);

                        ////angle = GetAngle(targetRef);
                        //angle = Mathf.Atan2(yClamp - screenCenter.y, xClamp - screenCenter.x) * Mathf.Rad2Deg;
                        //transform.position = new Vector2(xClamp, yClamp);

                        transform.position = new Vector2(aimWorldPos.x, aimWorldPos.y);
                        transform.Rotate(0, 0, rotateSpeed);
                    }
                    else
                    {
                        //Vector2 aimWorldPos = cam.WorldToScreenPoint(targetRef.transform.position);

                        //xClamp = Mathf.Clamp(aimWorldPos.x, 0, Screen.width);
                        //yClamp = Mathf.Clamp(aimWorldPos.y, 0, Screen.height);

                        ////angle = GetAngle(targetRef);
                        //angle = Mathf.Atan2(aimWorldPos.y - screenCenter.y, aimWorldPos.x - screenCenter.x) * Mathf.Rad2Deg;

                        ////if (angle < 0) angle = -angle;
                        ////the issue is that when the object is rotated around, the angles are back to how they were when facing the object.
                        //if (angle < 0)
                        //{
                        //    yClamp = 0;
                        //}
                        //else if (angle > 0)
                        //{
                        //    yClamp = Screen.height;
                        //}

                        //transform.position = new Vector2(xClamp, yClamp);

                        //transform.Rotate(0, 0, rotateSpeed);

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
        else
        {
            Destroy(gameObject);
        }
    }

    /*
    private float GetAngle(GameObject target)
    {
        float angle;
        float xDiff = target.transform.position.x - playerRef.transform.position.x;
        float zDiff = target.transform.position.z - playerRef.transform.position.z;

        angle = Mathf.Atan(xDiff / zDiff) * 180f / Mathf.PI;
        // tangent only returns an angle from -90 to +90.  we need to check if its behind us and adjust.
        if (zDiff < 0)
        {
            if (xDiff >= 0)
                angle += 180f;
            else
                angle -= 180f;
        }

        // this is our angle of rotation from 0->360
        float playerAngle = playerRef.transform.eulerAngles.y;
        // we  need to adjust this to our -180->180 system.
        if (playerAngle > 180f)
            playerAngle = 360f - playerAngle;

        // now subtract the player angle to get our relative angle to target.
        angle -= playerAngle;

        // Make sure we didn't rotate past 180 in either direction
        if (angle < -180f)
            angle += 360;
        else if (angle > 180f)
            angle -= 360;

        // now we have our correct relative angle to the target between -180 and 180
        // Lets clamp it between -135 and 135
        return angle;
    }*/
}
