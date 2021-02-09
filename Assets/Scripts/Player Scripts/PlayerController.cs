//PLAYER CONTROLLER SCRIPT
//Created By: Iain Farlow
//Created On: 20/11/2020
//Last Edited: 20/11/2020
//Edited By: Iain Farlow

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector3 targetPosition;
    bool moving = false;
    string movingToColour;

    public HealthBarScript healthBar;

    public float movementSpeed = 1.0f;
    public float playerHealth = 100.0f;
    public float playerScore = 0.0f;

    private void Start()
    {
        AttachToNearest();
    }

    public bool PlayerMove()
    {
        if (Input.GetKeyDown("space")) //Fix for mobile. 
        //Gunna need 2 finger controller or bottons for spinning
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

    void GetTargetInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2000))
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
        if(movingToColour == "Red")
        {
            Debug.Log("Heavy Damage!");
            playerHealth -= 25.0f;
            healthBar.SetHealth(playerHealth);
        }
        else if (movingToColour == "Orange")
        {
            Debug.Log("Light Damage!");
            playerHealth -= 10.0f;
            healthBar.SetHealth(playerHealth);
        }
        else if (movingToColour == "Purple")
        {
            Debug.Log("Miss a turn!");
        }
        else if (movingToColour == "Blue")
        {
            Debug.Log("Heal!");
            playerHealth += 25.0f;
            healthBar.SetHealth(playerHealth);
        }
        else if (movingToColour == "Green")
        {
            Debug.Log("Nothing Here!");
        }
        else if (movingToColour == "Yellow")
        {
            Debug.Log("Score!");
            playerScore += 5.0f;
        }
        else
        {
            Debug.Log("Tile Not Found");
        }
    }
}
