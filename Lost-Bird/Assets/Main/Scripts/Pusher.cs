using UnityEngine;

public class Pusher : MonoBehaviour
{
    #region FIELDS

    [SerializeField] private PlayerMovement playerMovement;

    #endregion

    #region BEHAVIOR

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Box>() == null)
            return;

        Box box = other.GetComponent<Box>();
        if (box.CanPush(playerMovement.MovementInput))
            playerMovement.PushObject(box);
    }

    #endregion
}
