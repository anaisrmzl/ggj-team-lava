using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

using Zenject;
using Utilities.Sound;

public class Respawn : MonoBehaviour
{
    [Inject] private SoundManager soundManager = null;

    [SerializeField] private AudioClip splashSound = null;

    public bool Died { get; private set; } = false;

    private void OnCollisionEnter(Collision other)
    {
        if (Died)
            return;

        soundManager.PlayVoice(splashSound);
        StartCoroutine(Revive());
    }

    private IEnumerator Revive()
    {
        Died = true;
        yield return new WaitForSeconds(1.0f);
        ResetScene();
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
