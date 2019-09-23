using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider boostMeter;
    [SerializeField] private Slider shootMeter;
    [SerializeField] private Image shootFill;
    [SerializeField] private Image boostFill;

    private PlayerController playerController;
    private PlayerShoot playerShoot;

    [SerializeField] private Color overHeat;

     private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        playerShoot = FindObjectOfType<PlayerShoot>();
    }

    // Update is called once per frame
    void Update()
    {
        BoostMeter();
        ShootingMeter();
    }

    private void BoostMeter()
    {
        boostMeter.value = playerController.EnergyPercent();
    }

    private void ShootingMeter()
    {
        shootMeter.value = playerShoot.HeatPercent();
    }

    public void ShootMeterColorRed()
    {
        shootFill.color = overHeat;
    }

    public void ShootMeterColorWhite() 
    {
        shootFill.color = Color.white;
    }

    public void BoostMeterRed()
    {
        boostFill.color = overHeat;
    }

    public void BoostMeterWhite()
    {
        boostFill.color = Color.white;
    }
}
