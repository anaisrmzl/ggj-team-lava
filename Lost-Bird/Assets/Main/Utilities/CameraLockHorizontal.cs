using UnityEngine;
using Cinemachine;

namespace CFLFramework.Utilities.Cameras
{
    public class CameraLockHorizontal : MonoBehaviour
    {
        #region FIELDS

        [Header("CONFIGURATIONS")]
        [SerializeField] private float desiredHorizontalSize = 10;
        [SerializeField] private bool frameCheck = false;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            SetCameraSize();
        }

        private void OnDrawGizmos()
        {
            SetCameraSize();
        }

        private void OnValidate()
        {
            SetCameraSize();
        }

        private void Update()
        {
            if (!frameCheck)
                return;

            SetCameraSize();
        }

        private void SetCameraSize()
        {
            CinemachineVirtualCamera camera = GetComponent<CinemachineVirtualCamera>();
            camera.m_Lens.OrthographicSize = (desiredHorizontalSize / 2) / camera.m_Lens.Aspect;
        }

        #endregion
    }
}
