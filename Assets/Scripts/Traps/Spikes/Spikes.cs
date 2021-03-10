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
        currentTurn = levelControler.GetComponent<TurnManagerScript>().GetCurrentTurn();
        if (currentTurn%3 == 0)
        {
            spikes.SetActive(true);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.up, out hit, 1.0f))
            {
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
        else
        {
            spikes.SetActive(false);
        }
    }
}
