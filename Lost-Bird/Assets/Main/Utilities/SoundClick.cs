using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Utilities.Sound
{
    [RequireComponent(typeof(Button))]
    public class SoundClick : MonoBehaviour
    {
        #region FIELDS

        [Inject] protected SoundManager soundManager = null;

        [SerializeField] private AudioClip buttonSound = null;


#endregion

        #region BEHAVIORS

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(ClickSound);
        }

        public virtual void ClickSound()
        {
            soundManager.PlayEffect(buttonSound);
        }

        #endregion
    }
}
