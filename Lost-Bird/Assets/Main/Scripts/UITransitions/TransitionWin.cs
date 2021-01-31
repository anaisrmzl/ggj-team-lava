using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionWin : MonoBehaviour
{
    Coffee.UIExtensions.UIDissolve transition;
    WaitForSeconds transitionTime;
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

    public void TransitionActivate()
    {
        transition.reverse = true;
        transition.Play();
        transitionTime = new WaitForSeconds(transition.duration);
        StartCoroutine(TransitionTimer());
    }


    public void TransitionCredits()
    {
        transition.reverse = true;
        transition.Play();
        transitionTime = new WaitForSeconds(transition.duration);
        StartCoroutine(TransitionTimerCredits());
    }

    public void TransitionCreditsOut()
    {
        creditsTransition.reverse = true;
        creditsTransition.Play();
        transitionTime = new WaitForSeconds(transition.duration);
        StartCoroutine(TransitionTimerCreditsOut());
    }

    IEnumerator TransitionTimer()
    {
        yield return transitionTime;
        SceneManager.LoadScene("Menu");
    }

    IEnumerator TransitionTimerCredits()
    {
        yield return transitionTime;

        credits.SetActive(true);
        creditsTransition.reverse = false;
        creditsTransition.Play();
    }

    IEnumerator TransitionTimerCreditsOut()
    {
        yield return transitionTime;
        credits.SetActive(false);
        transition.reverse = false;
        transition.Play();

    }
}
