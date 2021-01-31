using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region FIELDS

    [SerializeField] private Animator animator = null;
    [SerializeField] private bool hasBird = false;
    [SerializeField] private GameObject bird = null;

    #endregion

    #region PROPERTIES

    public bool IsActive { get; private set; } = true;
    public bool HasBird { get => hasBird; }

    #endregion

    #region BEHAVIORS

    private void Awake()
    {
        bird.SetActive(hasBird);
        animator.SetBool("active", IsActive);
    }

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
                bird.SetActive(hasBird);
                player.LooseBird();
            }
        }
    }

    private IEnumerator StayInactive()
    {
        hasBird = false;
        bird.SetActive(hasBird);
        IsActive = false;
        animator.SetBool("active", IsActive);
        yield return new WaitForSeconds(5.0f);
        IsActive = true;
        animator.SetBool("active", IsActive);
    }

    #endregion
}
