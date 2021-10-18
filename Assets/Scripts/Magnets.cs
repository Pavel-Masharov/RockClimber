using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Magnets : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private NavMeshAgent agent;
    private Rigidbody rb;
    [SerializeField] private GameObject Head;

    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        agent.updateRotation = false;
    }

    void FixedUpdate()
    {
        rb.isKinematic = false;
    }

    private void OnMouseDrag()
    {
        Move();
    }

    private void Move()
    {
        if(GameManager.Instance.isActiveGame)
        {
            rb.isKinematic = true;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                agent.SetDestination(hit.point);
            }
            Head.transform.LookAt(hit.point);
        } 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            GameManager.Instance.Completed();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            GameManager.Instance.coins++;
        }
    }
}
