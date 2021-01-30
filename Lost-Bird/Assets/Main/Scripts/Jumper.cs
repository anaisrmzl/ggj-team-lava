using UnityEngine;

public class Jumper : MonoBehaviour
{
    #region FIELDS

    [SerializeField] private PlayerMovement playerMovement;

    #endregion

    #region BEHAVIOR

    private void OnTriggerEnter(Collider other)
    {
        if (playerMovement.IsJumping)
            return;

        if (other.GetComponent<Edge>() == null)
            return;

        Edge edge = other.GetComponent<Edge>();
        if (!edge.AreThereObjects() && Vector3.Angle(edge.Direction, playerMovement.transform.forward) <= 45.0f)
            playerMovement.Jump(edge);
    }

    #endregion
}
