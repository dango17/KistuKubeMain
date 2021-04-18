//HOSTILE MOVE SCRIPT
//Use: Brain of the hostile
//Created By: Iain Farlow
//Created On: 09/02/2021
//Last Edited: 17/03/2021
//Edited By: Iain Farlow
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HostileMoveScript : MonoBehaviour
{
    Vector3 targetPosition;
    GameObject player;
    bool moving = false;

    public HealthBarScript healthBar;

    public float movementSpeed = 1.0f;
    public float hostileHealth = 100.0f;

    private void Start()
    {
        //Attach to nearest cube (becomes cubes child to all for rotation of cube)
        AttachToNearest();
        player = GameObject.Find("Player");
    }

    //called in turn manager
    public bool HostileMove()
    {
        Vector3 hostileObjectNorm = this.gameObject.transform.up;
        Vector3 playerObjectNorm = player.transform.up;
        //Check if hostile and player have same up direction (same face)
        if (Vector3.Dot(hostileObjectNorm, playerObjectNorm) > 0.9f)
        {
            // if hsotile is not moving and is child of cube
            if (moving == false && this.transform.parent != null)
            {
                //Remove the hostile from cubes child
                this.transform.parent = null;
                //Decide direction to move
                ChooseDirection();
                //Get information of spot to move too
                GetTargetInfo();
            }
            //if hostile has been told to move
            if (moving == true)
            {
                //ensure hostile is not parented to cube
                this.transform.parent = null;
                //triger move function
                Move();
                return true;
            }
            //if not moving
            else if (this.transform.parent == null)
            {
                //attach to closest cube
                AttachToNearest();
                //return false to show end of hostile move
                return false;
            }
        }
        return false;
    }

    void ChooseDirection()
    {
        //Decide direction to face based on players position relative to hostile
        if (player.transform.position.x > transform.position.x + 0.5f)
        {
            //look at direction
            transform.LookAt(new Vector3(transform.position.x + 1, transform.position.y, transform.position.z), transform.up);
        }
        else if (player.transform.position.x < transform.position.x - 0.5f)
        {
            transform.LookAt(new Vector3(transform.position.x - 1, transform.position.y, transform.position.z), transform.up);
        }
        else if (player.transform.position.z > transform.position.z + 0.5f)
        {
            transform.LookAt(new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), transform.up);
        }
        else if (player.transform.position.z < transform.position.z - 0.5f)
        {
            transform.LookAt(new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), transform.up);
        }
    }

    void GetTargetInfo()
    {
        RaycastHit hit;
        //Ray out forwards to check for obstruction
        Physics.Raycast(transform.position, transform.forward, out hit, 1.0f);
        //Check if ray hit non hostile object
        if (hit.transform != null && hit.transform.gameObject.CompareTag("Hostile"))
        {
            //if it did, stay
            //Debug.Log("Hostile Stay");
            targetPosition = transform.position;
            moving = true;
        }
        //if nothing infront of hostile
        //Ray out 1 forards facing down to get tile info
        else if (Physics.Raycast(transform.position + transform.forward + (transform.up * -0.1f), transform.up * -1, out hit, 1.0f))
        {
            //Debug.Log(hit.transform.gameObject.name);
            Vector3 rayHitNorm = hit.normal;
            Vector3 ObjectNorm = transform.up;

            //Check if it it face and that orintation is correct
            if ((Vector3.Dot(rayHitNorm, ObjectNorm) > 0.9f) && (hit.transform.gameObject.tag == "Face"))
            {
                targetPosition = hit.point;
                //Depending on orientation of hostile round move to coordinates to middle of face
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
                //set mobing to true
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

    void Move()
    {
        //move towards the target postion
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

        //if the player is at the wanted postion end movement section
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

    private void OnCollisionEnter(Collision collision)
    {
        //on collision check if with player. If it is deal damage to player
        if (collision.gameObject.CompareTag("Player"))
        {
            //deal damage changes to one shot
            collision.gameObject.GetComponent<PlayerController>().DealDamage();
        }
    }
    public void DealDamage()
    {
        //destoy the hostile
        Debug.Log("Hostile Dead!");
        Destroy(this.gameObject);
    }
}