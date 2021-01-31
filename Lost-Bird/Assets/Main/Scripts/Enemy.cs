using System.Collections;
using UnityEngine;

using Zenject;
using Utilities.Sound;

public class Enemy : MonoBehaviour
{
    #region FIELDS

    [Inject] private SoundManager soundManager;

    [SerializeField] private Animator animator = null;
    [SerializeField] private bool hasBird = false;
    [SerializeField] private GameObject bird = null;
    [SerializeField] private GameObject key = null;
    [SerializeField] private AudioClip captureSound = null;
    [SerializeField] private AudioClip freeSound = null;

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
        key.SetActive(hasBird);
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
                if (player.HasBird)
                {
                    hasBird = true;
                    bird.SetActive(hasBird);
                    key.SetActive(hasBird);
                    soundManager.PlayEffectOneShot(captureSound);
                    player.LooseBird();
                }
            }
        }
    }

    private IEnumerator StayInactive()
    {
        soundManager.PlayEffectOneShot(freeSound);
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
