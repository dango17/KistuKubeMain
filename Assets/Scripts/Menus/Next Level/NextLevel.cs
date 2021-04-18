//NEXT LEVEL SCRIPT
//Use: Shows the next level menu
//Created By: Iain Farlow
//Created On: 17/03/2021
//Edited By: Iain Farlow

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
        //show next level menu
        m_nextMenu.SetActive(true);
        //save any completed challenges 
        m_levelHander.GetComponent<ChallengeManager>().SaveChallengeStatus();
    }
}
