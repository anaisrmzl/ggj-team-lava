using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Box>() == null)
            return;

        Box box = other.GetComponent<Box>();
        if (box.CanPush(playerController.MovementInput))
            playerController.PushObject(box);
    }
}
