using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

using Zenject;
using Utilities.Sound;

public class Respawn : MonoBehaviour
{
    [Inject] private SoundManager soundManager = null;

    [SerializeField] private AudioClip splashSound = null;

    private bool died = false;

    private void OnTriggerEnter(Collider other)
    {
        if (died)
            return;

        soundManager.PlayVoice(splashSound);
        StartCoroutine(Revive());
    }

    private IEnumerator Revive()
    {
        died = true;
        yield return new WaitForSeconds(1.0f);
        ResetScene();
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
