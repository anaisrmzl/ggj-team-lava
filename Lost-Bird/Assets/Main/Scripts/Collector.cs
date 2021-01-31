using UnityEngine;
using UnityEngine.Events;

using Zenject;
using Utilities.Sound;

public class Collector : MonoBehaviour
{
    #region FIELDS

    [Inject] private SoundManager soundManager;

    [SerializeField] private AudioClip keySound = null;
    [SerializeField] private AudioClip featherSound = null;
    [SerializeField] private GameObject keyObject = null;

    private int feathers = default(int);
    private bool key = false;

    #endregion

    #region EVENTS

    public UnityAction onUpdateUI;

    #endregion

    #region PROPERTIES

    public int Feathers { get => feathers; }
    public bool HasKey { get => key; }

    #endregion

    #region BEHAVIORS

    private void Awake()
    {
        keyObject.SetActive(key);
    }

    private void Start()
    {
        onUpdateUI?.Invoke();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Feather")
        {
            feathers++;
            onUpdateUI?.Invoke();
            soundManager.PlayEffectOneShot(featherSound);
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.tag == "Key" && !key)
        {
            key = true;
            keyObject.SetActive(key);
            soundManager.PlayEffectOneShot(keySound);
            onUpdateUI?.Invoke();
            other.gameObject.SetActive(false);
        }
    }

    public void UseKey()
    {
        key = false;
        keyObject.SetActive(key);
        onUpdateUI?.Invoke();
    }

    #endregion
}
