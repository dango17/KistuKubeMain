using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCollect : MonoBehaviour
{
    public int KeysAmount = 1;
    public GameObject collectEffect;

    public void OnTriggerEnter(Collider other)
    {
        CollectManager.instance.UpdateKeysUI(KeysAmount);
        CollectManager.instance.currentkeys += KeysAmount;

        Instantiate(collectEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
