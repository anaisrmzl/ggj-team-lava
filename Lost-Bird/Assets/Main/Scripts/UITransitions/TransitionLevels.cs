using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionLevels : MonoBehaviour
{
    Coffee.UIExtensions.UIDissolve transition;
    WaitForSeconds transitionTime;
    [SerializeField]
    private UIDisplayLeve UI;
    [SerializeField]
    private GameObject mainCanvas, nextLevelScreen;


    void Start()
    {
        transition = gameObject.GetComponent<Coffee.UIExtensions.UIDissolve>();
        transition.Play();
        //NextLevelScreenTransition();

    }

    public void NextLevelScreenTransition()
    {
        mainCanvas.SetActive(false);
        transition.reverse = true;
        transition.Play();
        transitionTime = new WaitForSeconds(transition.duration);
        StartCoroutine(LevelScreenTransitionAppear());
    }

    public void TransitionActivate()
    {
        mainCanvas.SetActive(false);
        transition.reverse = true;
        transition.Play();

        transitionTime = new WaitForSeconds(transition.duration);
        StartCoroutine(TransitionTimer());
    }

    IEnumerator TransitionTimer()
    {
        yield return transitionTime;

        UI.Next();
    }

    IEnumerator LevelScreenTransitionAppear()
    {
        yield return transitionTime;

        transition.reverse = false;
        transition.Play();
        nextLevelScreen.SetActive(true);

    }
}
