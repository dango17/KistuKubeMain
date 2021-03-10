using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField]
    private GameObject spikes;

    // Start is called before the first frame update
    void Start()
    {
        spikes.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Spiked");
        spikes.SetActive(true);
        if (other.transform.gameObject.CompareTag("Player"))
        {
            other.transform.gameObject.GetComponent<PlayerController>().DealDamage();
        }
        if (other.transform.gameObject.CompareTag("Hostile"))
        {
            other.transform.gameObject.GetComponent<HostileMoveScript>().DealDamage();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        spikes.SetActive(false);
    }
}
