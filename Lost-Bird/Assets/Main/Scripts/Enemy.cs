using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region FIELDS


    #endregion

    #region PROPERTIES

    public bool IsActive { get; private set; } = true;

    #endregion

    #region BEHAVIORS

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovement>().LooseBird();
        }
    }

    #endregion
}
