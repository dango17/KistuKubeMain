//PlAYER CONTROLLER SCRIPT
//Use: Controls for the player
//Created By: Iain Farlow
//Created On: 20/11/2020
//Last Edited: 07/04/2021
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
        //Ensure timescale is normal 
        Time.timeScale = 1;
        //attach to nearest cube to all for rotations
        AttachToNearest();
    }

    public bool PlayerMove()
    {
        //Get move infro from targeted square
        if (Input.GetKeyDown("space") && moving == false) //Fix for mobile. 
        {
            GetTargetInfo();
            return true;
        }
        //if the player has been told to move
        if (moving == true)
        {
            //ensure the player is not child of cube
            this.transform.parent = null;
            //move
            Move();
            return true;
        }//if the player has stopped moving and is not child of cube
        else if (this.transform.parent == null)
        {
            //make parent the cube
            AttachToNearest();
            return false;
        }
        return true;
    }

    void GetTargetInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //raycast to check for the players clicked point
        if (Physics.Raycast(ray, out hit, 2000))
        {
            Vector3 rayHitNorm = hit.normal;
            Vector3 playerObjectNorm = transform.up;
            //check if the point is within distance of the player, prevents diagonal and multi-square movement
            if (Vector3.Distance(hit.transform.position, transform.position) <= 1.1f)
            {
                //check the face is oerientated correctly 
                if ((Vector3.Dot(rayHitNorm, playerObjectNorm) > 0.9f) && (hit.transform.gameObject.tag == "Face"))
                {
                    targetPosition = hit.point;
                    //round the target position to move to centre of tile
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
                    //face player to position
                    transform.LookAt(targetPosition, transform.up);
                    moving = true;
                }
            }
        }
    }

    void Move()
    {
        //Play walk sound
        playerWalk.Play();
        //move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

        //stop when at target position
        if (Vector3.Dot(transform.position.normalized, targetPosition.normalized) > 0.999999f)
        {
            moving = false;
        }
    }

    void AttachToNearest()
    {
        Vector3 position = transform.position;
        //get all of the samll cubes - use linq orderby to order each based onf distance to gameobject - gets first (closest)
        GameObject attachTo = GameObject.FindGameObjectsWithTag("SmallCube")
            .OrderBy(o => (o.transform.position - position).sqrMagnitude).FirstOrDefault();
        //set this objects parent to closest cube
        this.transform.parent = attachTo.transform;
    }

    public void DealDamage()
    {
        //Show the dead sign and set timescale to 0
        Debug.Log("Player Dead!");
        deadSign.SetActive(true);
        Time.timeScale = 0;
    }
}
