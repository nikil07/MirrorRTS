using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class UnitMovement : NetworkBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Targetter targetter;
    [SerializeField] float chaseRange = 10f;

    private Camera mainCamera;


    #region Server

    [ServerCallback]
    private void Update()
    {
        Targetable target = targetter.getTarget();
        if (target != null) {
            // Distance is using square roots to calculate and is pretty slow
            //So can also use below code to check distance
            // (target.transform.position - transform.position).squareMagnitute > chaseRnage * chaseRnage
            if (Vector3.Distance(target.transform.position,transform.position) > chaseRange) 
            {
                // Chase
                agent.SetDestination(target.gameObject.transform.position);
            } else if (agent.hasPath) {
                // stop chasing
                agent.ResetPath();
            }
            return;
        }
        if (!agent.hasPath)
            return;

        if (agent.remainingDistance <= agent.stoppingDistance)
            agent.ResetPath();
    }

    [Command]
    public void cmdMove(Vector3 position)
    {
        targetter.clearTarget();

        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1, NavMesh.AllAreas))
            return;

        agent.SetDestination(hit.position);
    }

    #endregion

    #region Client

    #endregion
}
