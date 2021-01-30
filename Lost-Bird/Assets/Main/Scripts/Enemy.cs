using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region FIELDS

    [SerializeField] private bool hasBird = false;

    #endregion

    #region PROPERTIES

    public bool IsActive { get; private set; } = true;
    public bool HasBird { get => hasBird; }

    #endregion

    #region BEHAVIORS

    private void OnCollisionEnter(Collision other)
    {
        if (!IsActive)
            return;

        if (other.gameObject.tag == "Player")
        {
            PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
            if (HasBird)
            {
                if (player.FreeBird())
                    StartCoroutine(StayInactive());
            }
            else
            {
                hasBird = true;
                player.LooseBird();
            }
        }
    }

    private IEnumerator StayInactive()
    {
        hasBird = false;
        IsActive = false;
        yield return new WaitForSeconds(5.0f);
        IsActive = true;
    }

    #endregion
}
