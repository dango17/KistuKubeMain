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
        hostileObjectNorm.x = Mathf.Round(hostileObjectNorm.x * 100.0f) / 100.0f;
        hostileObjectNorm.y = Mathf.Round(hostileObjectNorm.y * 100.0f) / 100.0f;
        hostileObjectNorm.z = Mathf.Round(hostileObjectNorm.z * 100.0f) / 100.0f;
        Vector3 playerObjectNorm = player.transform.up;
        playerObjectNorm.x = Mathf.Round(playerObjectNorm.x * 100.0f) / 100.0f;
        playerObjectNorm.y = Mathf.Round(playerObjectNorm.y * 100.0f) / 100.0f;
        playerObjectNorm.z = Mathf.Round(playerObjectNorm.z * 100.0f) / 100.0f;
        if (hostileObjectNorm == playerObjectNorm)
        {
            if (moving == false && this.transform.parent != null)
            {
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

    void GetTargetInfo()
    {
        RaycastHit hit;
        //this.gameObject.transform.LookAt(player.transform.position);
        Vector3 aimDir = transform.TransformDirection(0.0f, -0.4f, 1.0f);
        if (Physics.Raycast(transform.position, aimDir, out hit, 1.0f))
        {
            Vector3 rayHitNorm = hit.normal;
            rayHitNorm.x = Mathf.Round(rayHitNorm.x * 100.0f) / 100.0f;
            rayHitNorm.y = Mathf.Round(rayHitNorm.y * 100.0f) / 100.0f;
            rayHitNorm.z = Mathf.Round(rayHitNorm.z * 100.0f) / 100.0f;
            Vector3 playerObjectNorm = this.gameObject.transform.up;
            playerObjectNorm.x = Mathf.Round(playerObjectNorm.x * 100.0f) / 100.0f;
            playerObjectNorm.y = Mathf.Round(playerObjectNorm.y * 100.0f) / 100.0f;
            playerObjectNorm.z = Mathf.Round(playerObjectNorm.z * 100.0f) / 100.0f;

            if ((rayHitNorm == playerObjectNorm) && (hit.transform.gameObject.tag == "Face"))
            {
                targetPosition = hit.point;
                if (playerObjectNorm == Vector3.up || playerObjectNorm == Vector3.down)
                {
                    targetPosition = new Vector3(Mathf.Round(targetPosition.x), transform.position.y, Mathf.Round(targetPosition.z));
                }
                else if (playerObjectNorm == Vector3.left || playerObjectNorm == Vector3.right)
                {
                    targetPosition = new Vector3(transform.position.x, Mathf.Round(targetPosition.y), Mathf.Round(targetPosition.z));
                }
                else if (playerObjectNorm == Vector3.forward || playerObjectNorm == Vector3.back)
                {
                    targetPosition = new Vector3(Mathf.Round(targetPosition.x), Mathf.Round(targetPosition.y), transform.position.z);
                }
                movingToColour = hit.transform.name;
                moving = true;
            }
        }
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

        if (transform.position == targetPosition)
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