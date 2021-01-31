using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionCredits : MonoBehaviour
{
    Coffee.UIExtensions.UIDissolve transition;
    WaitForSeconds transitionTime;
    [SerializeField]
    private UIDisplay UI;

    [SerializeField]
    private GameObject credits;
    [SerializeField]
    private Coffee.UIExtensions.UIDissolve creditsTransition;
    [SerializeField]

    void Start()
    {
        transition = gameObject.GetComponent<Coffee.UIExtensions.UIDissolve>();
        transition.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
