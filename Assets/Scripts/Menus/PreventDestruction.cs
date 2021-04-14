using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreventDestruction : MonoBehaviour
{
    private void Awake()
    {
        GameObject[] musicTagged = GameObject.FindGameObjectsWithTag("Music");
        if (musicTagged.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
