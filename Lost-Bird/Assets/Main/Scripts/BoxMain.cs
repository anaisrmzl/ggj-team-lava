using System.Collections;
using UnityEngine;

public class BoxMain : MonoBehaviour
{
    #region FIELDS

    [SerializeField] private Rigidbody boxRigidbody;

    private bool inWater = false;
    private bool waterFlow = false;
    public bool Colliding { get; set; }

    #endregion

    #region BEHAVIORS

    private void Update()
    {
        if (waterFlow && !Colliding)
            boxRigidbody.MovePosition(boxRigidbody.position + Vector3.left * Time.fixedDeltaTime);
    }

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
        if (other.transform.tag == "Ground" && !inWater)
        {
            inWater = true;
            StartCoroutine(WaterFlow());
        }
    }

    private IEnumerator WaterFlow()
    {
        yield return new WaitForSeconds(1.0f);
        waterFlow = true;
    }

    #endregion
}
