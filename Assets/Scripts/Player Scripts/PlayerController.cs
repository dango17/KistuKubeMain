//PLAYER CONTROLLER SCRIPT
//Created By: Iain Farlow
//Created On: 20/11/2020
//Last Edited: 20/11/2020
//Edited By: Iain Farlow

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;

public class PlayerController : MonoBehaviour
{
    Vector3 targetPosition;
    bool moving = false;

    public HealthBarScript healthBar;

    public float movementSpeed = 1.0f;
    public float playerHealth = 100.0f;
    public float playerScore = 0.0f;

    public AudioSource playerWalk; 

    [SerializeField]
    GameObject deadSign;

    private void Start()
    {
        Time.timeScale = 1;
        AttachToNearest();
    }

    public bool PlayerMove()
    {
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
        {
            if (Input.GetMouseButtonDown(0) && moving == false) //Fix for mobile. 
            {
                GetTargetInfo();
                return true;
            }
            if (moving == true)
            {
                this.transform.parent = null;
                Move();
                return true;
            }
            else if (this.transform.parent == null)
            {
                AttachToNearest();
                return false;
            }
            return true;
        }
        else
        {
            if (Input.GetKeyDown("space") && moving == false) //Fix for mobile. 
            {
                GetTargetInfo();
                return true;
            }
            if (moving == true)
            {
                this.transform.parent = null;
                Move();
                return true;
            }
            else if (this.transform.parent == null)
            {
                AttachToNearest();
                return false;
            }
            return true;
        }
    }

    void GetTargetInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2000))
        {
            Vector3 rayHitNorm = hit.normal;
            Vector3 playerObjectNorm = transform.up;
            if (Vector3.Distance(hit.transform.position, transform.position) <= 1.1f)
            {
                if ((Vector3.Dot(rayHitNorm, playerObjectNorm) > 0.9f) && (hit.transform.gameObject.tag == "Face"))
                {
                    targetPosition = hit.point;
                    if (Vector3.Dot(playerObjectNorm, Vector3.up) > 0.9f || Vector3.Dot(playerObjectNorm, Vector3.down) > 0.9f)
                    {
                        targetPosition = new Vector3(Mathf.Round(targetPosition.x), transform.position.y, Mathf.Round(targetPosition.z));
                    }
                    else if (Vector3.Dot(playerObjectNorm, Vector3.left) > 0.9f || Vector3.Dot(playerObjectNorm, Vector3.right) > 0.9f)
                    {
                        targetPosition = new Vector3(transform.position.x, Mathf.Round(targetPosition.y), Mathf.Round(targetPosition.z));
                    }
                    else if (Vector3.Dot(playerObjectNorm, Vector3.forward) > 0.9f || Vector3.Dot(playerObjectNorm, Vector3.back) > 0.9f)
                    {
                        targetPosition = new Vector3(Mathf.Round(targetPosition.x), Mathf.Round(targetPosition.y), transform.position.z);
                    }
                    transform.LookAt(targetPosition, transform.up);
                    moving = true;
                }
            }
        }
    }

    void Move()
    {
        playerWalk.Play();
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
        
        if (Vector3.Dot(transform.position.normalized, targetPosition.normalized) > 0.999999f)
        {
            moving = false;
        }
    }

    void AttachToNearest()
    {
        Vector3 position = transform.position;
        GameObject attachTo = GameObject.FindGameObjectsWithTag("SmallCube")
            .OrderBy(o => (o.transform.position - position).sqrMagnitude).FirstOrDefault();
        this.transform.parent = attachTo.transform;
    }

    public void DealDamage()
    {
        Debug.Log("Player Dead!");
        deadSign.SetActive(true);
        Time.timeScale = 0;
    }
}
