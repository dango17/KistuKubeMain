//HEALTH BAR SCRIPT
//Use: Set health value of healthbar
//Created By: Iain Farlow
//Created On: 07/02/2021
//Last Edited: 07/02/2021
//Edited By: Iain Farlow
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Slider slider;

    public void SetHealth(float health)
    {
        slider.value = health;
    }
}
