using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HostileMoveScript : MonoBehaviour
{
    Vector3 targetPosition;
    GameObject player;
    bool moving = false;
    string movingToColour;

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
                ChooseDirection();
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

    void ChooseDirection()
    {
        Vector3 ObjectNorm = transform.up;

        if (Vector3.Dot(ObjectNorm, Vector3.up) > 0.9f || Vector3.Dot(ObjectNorm, Vector3.down) > 0.9f)
        {
            Debug.Log("Top/Bottom");
            if (player.transform.position.x > transform.position.x + 0.5f)
            {
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
        else if (Vector3.Dot(ObjectNorm, Vector3.left) > 0.9f || Vector3.Dot(ObjectNorm, Vector3.right) > 0.9f)
        {
            Debug.Log("Left/Right");
            if (player.transform.position.x > transform.position.x + 0.5f)
            {
                transform.LookAt(new Vector3(transform.position.x + 1, transform.position.y, transform.position.z), transform.up);
            }
            else if (player.transform.position.x < transform.position.x - 0.5f)
            {
                transform.LookAt(new Vector3(transform.position.x - 1, transform.position.y, transform.position.z), transform.up);
            }
            else if (player.transform.position.y > transform.position.y + 0.5f)
            {
                transform.LookAt(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.up);
            }
            else if (player.transform.position.y < transform.position.y - 0.5f)
            {
                transform.LookAt(new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), transform.up);
            }
        }
        else if (Vector3.Dot(ObjectNorm, Vector3.forward) > 0.9f || Vector3.Dot(ObjectNorm, Vector3.back) > 0.9f)
        {
            Debug.Log("Forward/Back");
            if (player.transform.position.y > transform.position.y + 0.5f)
            {
                transform.LookAt(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.up);
            }
            else if (player.transform.position.y < transform.position.y - 0.5f)
            {
                transform.LookAt(new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), transform.up);
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
    }

    void GetTargetInfo()
    {
        RaycastHit hit;
        Vector3 aimDir = transform.TransformDirection(0.0f, -0.4f, 1.0f);
        if (Physics.Raycast(transform.position, aimDir, out hit, 1.0f))
        {
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
                movingToColour = hit.transform.name;
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
            TileAffect();
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

    void TileAffect()
    {
        if (movingToColour == "Red")
        {
            Debug.Log("Heavy Damage!");
            hostileHealth -= 25.0f;
            healthBar.SetHealth(hostileHealth);
        }
        else if (movingToColour == "Orange")
        {
            Debug.Log("Light Damage!");
            hostileHealth -= 10.0f;
            healthBar.SetHealth(hostileHealth);
        }
        else if (movingToColour == "Purple")
        {
            Debug.Log("Miss a turn!");
        }
        else if (movingToColour == "Blue")
        {
            Debug.Log("Heal!");
            hostileHealth += 25.0f;
            healthBar.SetHealth(hostileHealth);
        }
        else if (movingToColour == "Green")
        {
            Debug.Log("Nothing Here!");
        }
        else if (movingToColour == "Yellow")
        {
            Debug.Log("Score!");
            hostileHealth += 5.0f;
        }
        else
        {
            Debug.Log("Tile Not Found");
        }
    }
}