using System.Collections;
using UnityEngine;

using Zenject;
using Utilities.Sound;

public class BoxMain : MonoBehaviour
{
    #region FIELDS

    [Inject] private SoundManager soundManager;

    [SerializeField] private AudioClip splashSound;
    [SerializeField] private Rigidbody boxRigidbody;
    [SerializeField] private bool silenceOnStart = false;

    private bool inWater = false;
    private bool waterFlow = false;
    public bool Colliding { get; set; }

    #endregion

    #region BEHAVIORS

    private void Update()
    {
        if (waterFlow && !Colliding)
            boxRigidbody.velocity = new Vector3(-5.0f, 0.0f, 0.0f);
        /*boxRigidbody.MovePosition(boxRigidbody.position + Vector3.left * Time.fixedDeltaTime);*/
    }

    public void FreezeConstraints(bool status)
    {
        if (status)
        {
            if (waterFlow)
                return;

            boxRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            boxRigidbody.velocity = Vector3.zero;
            boxRigidbody.angularDrag = 0.0f;
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
            if (!silenceOnStart)
                soundManager.PlayEffect(splashSound);

            StartCoroutine(WaterFlow());
        }
    }

    private IEnumerator WaterFlow()
    {
        yield return new WaitForSeconds(1.5f);
        waterFlow = true;
        FreezeConstraints(false);
    }

    #endregion
}
