using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //float screenCX = Screen.width / 2;
        //float ScreenCY = Screen.height / 2;

        Vector3 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 mousePos = Input.mousePosition - screenCenter;
        mousePos = mousePos.normalized;

        print(mousePos);

        transform.LookAt(mousePos);
        //float distanceFromCenter = Vector3.Distance(screenCenter, Input.mousePosition);
        


    }
}
