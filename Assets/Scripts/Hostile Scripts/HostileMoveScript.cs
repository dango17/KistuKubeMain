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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().DealDamage();
        }
    }
    public void DealDamage()
    {
        Debug.Log("Hostile Dead!");
        Destroy(this.gameObject);
    }
}