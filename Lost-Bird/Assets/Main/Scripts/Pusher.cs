using UnityEngine;

public class Pusher : MonoBehaviour
{
    #region FIELDS

    [SerializeField] private PlayerMovement playerMovement;

    private Box collidingBox = null;

    #endregion

    #region PROPERTIES

    public Box CollidingBox { get => collidingBox; }

    #endregion

    #region BEHAVIOR

    private void OnTriggerEnter(Collider other)
    {
        if (!playerMovement.CanPush)
            return;

        if (other.GetComponent<Box>() == null)
            return;

        collidingBox = other.GetComponent<Box>();
        TryPush(collidingBox, playerMovement.MovementInput);
    }

    public void TryPush(Box box, Vector3 direction)
    {

        if (box.CanPush(direction))
            playerMovement.StartPushing(box);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Box>() == null)
            return;

        if (other.GetComponent<Box>() == collidingBox)
            return;

        collidingBox = other.GetComponent<Box>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Box>() == null)
            return;

        collidingBox = null;
    }

    #endregion
}
