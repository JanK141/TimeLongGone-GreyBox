using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyWalk : MonoBehaviour
{
    [SerializeField] private Camera cam; //for testing
    [SerializeField] private LayerMask groundMask; //for testing

    private NavMeshAgent agent;

    private bool isTesting = false; //for testing


    private bool isWalking = false;
    public bool IsWalking{get=>isWalking;}
    private Vector3 currDestination;
    public Vector3 CurrDestination{get=>currDestination;}

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // -----------FOR TESTING ONLY --------------
        if (Input.GetKeyDown(KeyCode.T)) isTesting = !isTesting;
        if (isTesting && Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, groundMask))
            {
                WalkTo(hit.point);
            }
        }
        // -----------------------------------------------

        if (isWalking && Vector3.Distance(transform.position, currDestination) <= agent.stoppingDistance+0.5f){
            isWalking = false;
            agent.isStopped = true;
            agent.velocity /= 2;
        }

    }

    public void WalkTo(Vector3 destination)
    {
        agent.isStopped = false;
        isWalking = true;
        currDestination = destination;
        agent.SetDestination(destination);
    }
}
