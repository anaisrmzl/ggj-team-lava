using UnityEngine;
using UnityEngine.Events;

public class Collector : MonoBehaviour
{
    #region FIELDS

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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Feather")
        {
            feathers++;
            onUpdateUI?.Invoke();
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.tag == "Key" && !key)
        {
            key = true;
            onUpdateUI?.Invoke();
            other.gameObject.SetActive(false);
        }
    }

    public void UseKey()
    {
        key = false;
        onUpdateUI?.Invoke();
    }

    #endregion
}
