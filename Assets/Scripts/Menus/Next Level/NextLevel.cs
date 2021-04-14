using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NextLevel : MonoBehaviour
{
    [SerializeField]
    GameObject m_nextMenu;

    [SerializeField]
    GameObject m_levelHander;

    public void OnTriggerEnter()
    {
        m_nextMenu.SetActive(true);
        m_levelHander.GetComponent<ChallengeManager>().SaveChallengeStatus();
    }
}
