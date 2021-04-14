using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChallengeDisplayScript : MonoBehaviour
{
    [SerializeField]
    int m_challengeNum;
    // Start is called before the first frame update
    void Start()
    {
        GameData gameData = SaveSystem.LoadLevelData();

        if (gameData != null)
        {
            if (gameData.levels[SceneManager.GetActiveScene().buildIndex - 1].challenage[m_challengeNum - 1])
            {
                this.gameObject.GetComponent<TextMeshProUGUI>().color = Color.green;
            }
            else
            {
                this.gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;
            }
        }
        else
        {
            this.gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
