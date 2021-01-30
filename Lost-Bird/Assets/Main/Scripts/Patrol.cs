using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    #region FIELDS

    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float startWaitTime;
    [SerializeField] private Transform[] moveSpots;
    [SerializeField] private Enemy enemy;

    private int spot;
    private int rotationSpot;
    private float waitTime;

    #endregion

    #region BEHAVIORS

    private void Start()
    {
        waitTime = startWaitTime;
        spot = rotationSpot = 0;
    }

    private void Update()
    {
        if (!enemy.IsActive)
            return;

        Vector3 targetDirection = moveSpots[rotationSpot].position - transform.position;
        float singleStep = rotationSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);

        transform.position = Vector3.MoveTowards(transform.position, moveSpots[spot].position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, moveSpots[spot].position) < 0.001f)
            if (waitTime <= 0)
            {
                spot = spot == moveSpots.Length - 1 ? 0 : spot + 1;
                waitTime = startWaitTime;
            }
            else
            {
                rotationSpot = spot == moveSpots.Length - 1 ? 0 : spot + 1;
                waitTime -= Time.deltaTime;
            }
    }

    #endregion
}
