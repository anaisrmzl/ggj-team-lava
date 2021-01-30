using UnityEngine;

public class BoxMain : MonoBehaviour
{
    #region FIELDS

    [SerializeField] private Rigidbody boxRigidbody;

    #endregion

    #region BEHAVIORS

    public void FreezeConstraints(bool status)
    {
        if (status)
        {
            boxRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            boxRigidbody.constraints = RigidbodyConstraints.None;
            boxRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Ground")
        {
            Debug.Log("Water");
        }
    }

    #endregion
}
