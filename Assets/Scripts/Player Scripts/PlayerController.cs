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

    public bool ButtonDown = false;
    public string ButtonPressed;

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
        if (Application.platform == RuntimePlatform.Android || EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
        {
            if (ButtonDown && moving == false) //Fix for mobile. 
            {
                ChooseDirectionFromButton();
                GetInfoFromTarget();
                ButtonDown = false;
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

    void ChooseDirectionFromButton()
    {
        if (ButtonPressed == "Left")
        {
            transform.LookAt(transform.position + transform.right * -1);
        }
        else if (ButtonPressed == "Right")
        {
            transform.LookAt(transform.position + transform.right);
        }
        else if (ButtonPressed == "Up")
        {
            transform.LookAt(transform.position + transform.forward);
        }
        else if (ButtonPressed == "Down")
        {
            transform.LookAt(transform.position + transform.forward * -1);
        }
    }

    void GetInfoFromTarget()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position + (transform.up * 0.1f), transform.forward, out hit, 0.6f);
        if (hit.transform != null)
        {
            Debug.Log(hit.transform.gameObject.name);
            Debug.Log("Player Stay");
            targetPosition = transform.position;
            moving = true;
        }
        else if (Physics.Raycast(transform.position + transform.forward, transform.up * -1, out hit, 1.0f))
        {
            Debug.Log(hit.transform.gameObject.name);
            Vector3 rayHitNorm = hit.normal;
            Vector3 ObjectNorm = transform.up;

            if ((Vector3.Dot(rayHitNorm, ObjectNorm) > 0.9f) && (hit.transform.gameObject.tag == "SmallCube"))
            {
                targetPosition = hit.point;
                if (Vector3.Dot(ObjectNorm, Vector3.up) > 0.9f || Vector3.Dot(ObjectNorm, Vector3.down) > 0.9f)
                {
                    targetPosition = new Vector3(Mathf.Round(targetPosition.x), transform.position.y, Mathf.Round(targetPosition.z));
                }
                else if (Vector3.Dot(ObjectNorm, Vector3.left) > 0.9f || Vector3.Dot(ObjectNorm, Vector3.right) > 0.9f)
                {
                    targetPosition = new Vector3(transform.position.x, Mathf.Round(targetPosition.y), Mathf.Round(targetPosition.z));
                }
                else if (Vector3.Dot(ObjectNorm, Vector3.forward) > 0.9f || Vector3.Dot(ObjectNorm, Vector3.back) > 0.9f)
                {
                    targetPosition = new Vector3(Mathf.Round(targetPosition.x), Mathf.Round(targetPosition.y), transform.position.z);
                }
                else
                {
                    targetPosition = transform.position;
                }
                moving = true;
            }
        }
        else
        {
            targetPosition = transform.position;
            moving = true;
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
