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

    public bool ButtonDown = false;
    public string ButtonPressed;

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
        //check if current build is android
        if (Application.platform == RuntimePlatform.Android)// || EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
        {
            //if a move button has been pressed and the player is not moving
            if (ButtonDown && moving == false) //Fix for mobile. 
            {
                //work out direction to move
                ChooseDirectionFromButton();
                //Get move info from target position
                GetInfoFromTarget();
                //reset button bool
                ButtonDown = false;
                return true;
            }
            //if player has been told to move
            if (moving == true)
            {
                //ensure player is not child of cube
                this.transform.parent = null;
                //move
                Move();
                return true;
            }
            // player has finsihed moving and is not child of the cube
            else if (this.transform.parent == null)
            {
                //attach to closest call cube to allow for cube rotation
                AttachToNearest();
                return false;
            }
            return true;
        }
        else
        {
            //PC controls
            if (Input.GetKeyDown("space") && moving == false)
            {
                //Get move infro from targeted square
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
    }

    void ChooseDirectionFromButton()
    {
        //Depending on the string passed in by the button figure out direction
        if (ButtonPressed == "Left")
        {
            transform.LookAt(transform.position + transform.right * -1, transform.up);
        }
        else if (ButtonPressed == "Right")
        {
            transform.LookAt(transform.position + transform.right, transform.up);
        }
        else if (ButtonPressed == "Up")
        {
            transform.LookAt(transform.position + transform.forward, transform.up);
        }
        else if (ButtonPressed == "Down")
        {
            transform.LookAt(transform.position + transform.forward * -1, transform.up);
        }
    }

    void GetInfoFromTarget()
    {
        //raycast forwards to check for obstructions
        RaycastHit hit;
        Physics.Raycast(transform.position + (transform.up * 0.1f), transform.forward, out hit, 0.6f);
        //if obstruction stay at current position
        if (hit.transform != null)
        {
            //Debug.Log(hit.transform.gameObject.name);
            //Debug.Log("Player Stay");
            targetPosition = transform.position;
            moving = true;
        }
        //if no obstruction - raycast down a step ahead to check for tile
        else if (Physics.Raycast(transform.position + transform.forward, transform.up * -1, out hit, 1.0f))
        {
            Debug.Log(hit.transform.gameObject.name);
            Vector3 rayHitNorm = hit.normal;
            Vector3 ObjectNorm = transform.up;

            //Check if the hit point is facing the correct way
            if ((Vector3.Dot(rayHitNorm, ObjectNorm) > 0.9f) && (hit.transform.gameObject.tag == "SmallCube"))
            {
                targetPosition = hit.point;
                //Round the traget position based on the orrientation of the player, to ensure it moves to centre of point.
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
                    //if issue with rounding stay (should not trigger)
                    targetPosition = transform.position;
                }
                moving = true;
            }
        }
        else
        {
            //If they are trying to move off cube stay
            targetPosition = transform.position;
            moving = true;
        }
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
