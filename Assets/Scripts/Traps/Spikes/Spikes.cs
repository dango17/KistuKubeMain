using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField]
    private GameObject spikes;

    private GameObject levelControler;
    private int currentTurn;

    // Start is called before the first frame update
    void Start()
    {
        levelControler = GameObject.Find("LevelController");
        spikes.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(spikes.transform.position + new Vector3(0.0f, 0.1f, 0.0f), transform.forward, Color.red);
        currentTurn = levelControler.GetComponent<TurnManagerScript>().GetCurrentTurn();
        if (currentTurn%3 == 0)
        {
            spikes.SetActive(true);
        }
        else
        {
            spikes.SetActive(false);
        }
        if (spikes.activeInHierarchy)
        {
            RaycastHit hit;
            if (Physics.Raycast(spikes.transform.position + new Vector3(0.0f, 0.1f, 0.0f), transform.forward, out hit, 1.0f))
            {
                Debug.Log(hit.transform.gameObject.name);
                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    hit.transform.gameObject.GetComponent<PlayerController>().DealDamage();
                }
                if (hit.transform.gameObject.CompareTag("Hostile"))
                {
                    hit.transform.gameObject.GetComponent<HostileMoveScript>().DealDamage();
                }
            }
        }
    }
}
