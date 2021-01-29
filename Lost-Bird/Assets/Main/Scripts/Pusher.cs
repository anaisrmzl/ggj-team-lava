using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Box>() == null)
            return;

        Box box = other.GetComponent<Box>();
        if (box.CanPush(playerMovement.MovementInput))
            playerMovement.PushObject(box);
    }
}
