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
        AttachToNearest();
        player = GameObject.Find("Player");
    }

    public bool HostileMove()
    {
        Vector3 hostileObjectNorm = this.gameObject.transform.up;
        Vector3 playerObjectNorm = player.transform.up;
        if (Vector3.Dot(hostileObjectNorm, playerObjectNorm) > 0.9f)
        {
            if (moving == false && this.transform.parent != null)
            {
                this.transform.parent = null;
                int scanDist = ScanCube(0);
                m_map = GenMap(scanDist);
                ASTAR Astar = new ASTAR();
                Astar.Initilise(m_map, scanDist);
                Astar.Solve(scanDist);
                
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

                GetTargetInfo();
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
            Vector3 scanPos = new Vector3(transform.position.x + dimentions - x, transform.position.y, transform.position.z + a_scanDist);
            if (Physics.Raycast(scanPos, transform.up * -1, out hit, 1.0f))
            {
                if (hit.transform.gameObject.tag == "Face")
                {
                    spotFound = true;
                }
            }
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
            Vector3 scanPos = new Vector3(transform.position.x + a_scanDist, transform.position.y, transform.position.z + dimentions - y);
            if (Physics.Raycast(scanPos, transform.up * -1, out hit, 1.0f))
            {
                if (hit.transform.gameObject.tag == "Face")
                {
                    spotFound = true;
                }
            }
            scanPos = new Vector3(transform.position.x - a_scanDist, transform.position.y, transform.position.z + dimentions - y);
            if (Physics.Raycast(scanPos, transform.up * -1, out hit, 1.0f))
            {
                if (hit.transform.gameObject.tag == "Face")
                {
                    spotFound = true;
                }
            }
        }

        if (spotFound == true)
        {
            Debug.Log(a_scanDist);
            a_scanDist = ScanCube(a_scanDist + 1);
            return a_scanDist;
        }
        else
        {
            Debug.Log((a_scanDist - 1) + "DONE");
            return a_scanDist - 1;
        }
    }

    private char[] GenMap(int a_scanDist)
    {
        int dimentions = (a_scanDist * 2);
        char[] tempMap = new char[(dimentions + 1) * (dimentions + 1)];
        for (int x = 0; x < dimentions; x++)
        {
            for (int y = 0; y < dimentions; y++)
            {
                RaycastHit hit;
                Vector3 scanPos = new Vector3(transform.position.x + dimentions - x, transform.position.y, transform.position.z + dimentions - y);
                if (Physics.Raycast(scanPos, transform.up * -1, out hit, 1.0f))
                {
                    if (hit.transform.gameObject.tag == "Face")
                    {
                        tempMap[y * dimentions + x] = '.';
                    }
                    else if (hit.transform.gameObject.tag == "Player")
                    {
                        tempMap[y * dimentions + x] = 'B';
                    }
                    else
                    {
                        tempMap[y * dimentions + x] = 'X';
                    }
                }
                else
                {
                    tempMap[y * dimentions + x] = 'X';
                }
            }
        }
        tempMap[a_scanDist * dimentions + a_scanDist] = 'A';
        return tempMap;
    }

    void GetTargetInfo()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, 1.0f);
        if (hit.transform != null && hit.transform.gameObject.CompareTag("Hostile"))
        {
            Debug.Log("Hostile Stay");
            targetPosition = transform.position;
            moving = true;
        }
        else if (Physics.Raycast(transform.position + transform.forward + (transform.up * -0.1f), transform.up * -1, out hit, 1.0f))
        {
            Debug.Log(hit.transform.gameObject.name);
            Vector3 rayHitNorm = hit.normal;
            Vector3 ObjectNorm = transform.up;

            if ((Vector3.Dot(rayHitNorm, ObjectNorm) > 0.9f) && (hit.transform.gameObject.tag == "Face"))
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

    void Move()
    {
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
        Debug.Log("Hostile Dead!");
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().DealDamage();
        }
    }
}
