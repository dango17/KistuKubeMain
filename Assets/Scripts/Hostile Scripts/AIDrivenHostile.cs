//HOSTILE MOVE SCRIPT
//Use: Brain of the hostile using AStar
//Created By: Iain Farlow
//Created On: 10/04/2021
//Last Edited: 16/04/2021
//Edited By: Iain Farlow
//Due to time restriction this was an attempt to quickly convert a c++ AStar method I had previously written into c#
//Was abandoned due to time restictions and is NOT in the current build of the game
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIDrivenHostile : MonoBehaviour
{
    private char[] m_map;

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
                //Scan the current face of the cube (starting at 0th position (own position))
                int scanDist = ScanCube(0);
                //generate a map from scan
                m_map = GenMap(scanDist);
                //new astar
                ASTAR Astar = new ASTAR();
                //initilaise astar 
                Astar.Initilise(m_map, scanDist);
                //solve
                Astar.Solve(scanDist);
                //get end node
                Node endNode = Astar.GetEndNode();
                //Check for end node
                if (endNode != null)
                {
                    Node p = endNode;
                    //Loop through parents untill read end of chain
                    while (p.parent != null)
                    {
                        //Compare positions of parent and child to determine N/E/S/W
                        if (p.x > p.parent.x)
                        {
                            Debug.Log("Face West");
                        }
                        if (p.x < p.parent.x)
                        {
                            Debug.Log("Face East");
                        }
                        if (p.y > p.parent.y)
                        {
                            Debug.Log("Face North");
                        }
                        if (p.y < p.parent.y)
                        {
                            Debug.Log("Face South");
                        }
                
                        //assign parent as the new child
                        p = p.parent;
                    }
                }
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

    private int ScanCube(int a_scanDist)
    {
        bool spotFound = false;
        int dimentions = (a_scanDist * 2);
        for (int x = 0; x <= dimentions; x++)
        {
            RaycastHit hit;
            //racast at point along the outer set of points
            //EG. 5 x5 square s for scan o for possible points
            //oosoo
            //ooooo
            //ooooo
            //ooooo
            //ooooo
            //Will only scan the outer most points (border)
            Vector3 scanPos = new Vector3(transform.position.x + dimentions - x, transform.position.y, transform.position.z + a_scanDist);
            if (Physics.Raycast(scanPos, transform.up * -1, out hit, 1.0f))
            {
                //if hits face found a point
                if (hit.transform.gameObject.tag == "Face")
                {
                    spotFound = true;
                }
            }
            // oposite side of outer points 
            //EG. 5 x5 square s for scan o for possible points
            //ooooo
            //ooooo
            //ooooo
            //ooooo
            //oosoo
            scanPos = new Vector3(transform.position.x + dimentions - x, transform.position.y, transform.position.z - a_scanDist);
            if (Physics.Raycast(scanPos, transform.up * -1, out hit, 1.0f))
            {
                if (hit.transform.gameObject.tag == "Face")
                {
                    spotFound = true;
                }
            }
        }
        for (int y = 0; y <= dimentions; y++)
        {
            RaycastHit hit;
            //scan other two sides
            //EG. 5 x5 square s for scan o for possible points
            //ooooo
            //ooooo
            //oooos
            //ooooo
            //ooooo
            Vector3 scanPos = new Vector3(transform.position.x + a_scanDist, transform.position.y, transform.position.z + dimentions - y);
            if (Physics.Raycast(scanPos, transform.up * -1, out hit, 1.0f))
            {
                if (hit.transform.gameObject.tag == "Face")
                {
                    spotFound = true;
                }
            }
            //EG. 5 x5 square s for scan o for possible points
            //ooooo
            //ooooo
            //soooo
            //ooooo
            //ooooo
            scanPos = new Vector3(transform.position.x - a_scanDist, transform.position.y, transform.position.z + dimentions - y);
            if (Physics.Raycast(scanPos, transform.up * -1, out hit, 1.0f))
            {
                if (hit.transform.gameObject.tag == "Face")
                {
                    spotFound = true;
                }
            }
        }

        //If a sport has been found
        if (spotFound == true)
        {
            //increment the scan distance to check for more points
            //the further the out it finds points the larger the array (later on) made will be 
            //Debug.Log(a_scanDist);
            //call self with increased scan distance
            a_scanDist = ScanCube(a_scanDist + 1);
            return a_scanDist;
        }
        else
        {
            //If no points are found reduce the scan distance and return it (the further point found was in the later iteration)
            //Debug.Log((a_scanDist - 1) + "DONE");
            return a_scanDist - 1;
        }
    }

    private char[] GenMap(int a_scanDist)
    {
        //use previosuly made dimentions to create an array
        int dimentions = (a_scanDist * 2);
        char[] tempMap = new char[(dimentions + 1) * (dimentions + 1)];
        //scan the face arround the hostile
        for (int x = 0; x < dimentions; x++)
        {
            for (int y = 0; y < dimentions; y++)
            {
                RaycastHit hit;
                Vector3 scanPos = new Vector3(transform.position.x + dimentions - x, transform.position.y, transform.position.z + dimentions - y);
                if (Physics.Raycast(scanPos, transform.up * -1, out hit, 1.0f))
                {
                    //if it finds a face, this is a walkable point
                    if (hit.transform.gameObject.tag == "Face")
                    {
                        tempMap[y * dimentions + x] = '.';
                    }
                    //if it finds player this will be end point
                    else if (hit.transform.gameObject.tag == "Player")
                    {
                        tempMap[y * dimentions + x] = 'B';
                    }
                    else
                    {
                        //anything else is an obstacle 
                        tempMap[y * dimentions + x] = 'X';
                    }
                }
                else
                {
                    //if it finds nothing also obstacle
                    tempMap[y * dimentions + x] = 'X';
                }
            }
        }
        //middle point of map (hostiles position is start point)
        tempMap[a_scanDist * dimentions + a_scanDist] = 'A';
        return tempMap;
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

    public void DealDamage()
    {
        //destoy the hostile
        Debug.Log("Hostile Dead!");
        Destroy(this.gameObject);
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
}
