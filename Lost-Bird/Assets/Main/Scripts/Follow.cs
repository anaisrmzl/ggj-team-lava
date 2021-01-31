using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Follow : MonoBehaviour
{
    #region FIELDS

    [SerializeField] private NavMeshAgent navMeshAgent = null;
    [SerializeField] private Transform player = null;
    [SerializeField] private Enemy enemy;
    [SerializeField] private Transform[] moveSpots;

    private int randomSpot;
    private float waitTime;
    private bool chasingPlayer = false;

    #endregion

    #region BEHAVIOR

    private void Start()
    {
        randomSpot = Random.Range(0, moveSpots.Length);
        navMeshAgent.SetDestination(moveSpots[randomSpot].transform.position);
    }

    private void Update()
    {
        if (!enemy.IsActive)
            return;

        if (Vector3.Distance(transform.position, player.transform.position) < 4.0f)
        {
            if (!chasingPlayer)
                chasingPlayer = true;
        }
        else
        {
            if (chasingPlayer)
            {
                chasingPlayer = false;
                randomSpot = Random.Range(0, moveSpots.Length);
                navMeshAgent.SetDestination(moveSpots[randomSpot].transform.position);
            }

        }


        if (chasingPlayer)
        {
            navMeshAgent.SetDestination(player.transform.position);
        }
        else
        {
            if (navMeshAgent.remainingDistance < 0.1f)
            {
                randomSpot = Random.Range(0, moveSpots.Length);
                navMeshAgent.SetDestination(moveSpots[randomSpot].transform.position);
            }
        }


    }
    #endregion
}
