using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    [SerializeField]
    GameObject nextMenu;

    public void OnTriggerEnter()
    {
        nextMenu.SetActive(true);
    }
}
