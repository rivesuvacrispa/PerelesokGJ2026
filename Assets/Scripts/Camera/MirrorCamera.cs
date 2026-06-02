using Player;
using UnityEngine;

namespace Camera
{
    [ExecuteInEditMode]
    public class MirrorCamera : MonoBehaviour
    {
        [SerializeField] private Transform plane;
        [SerializeField] private bool move;
        [SerializeField] private Vector3 movementAxis;

        private UnityEngine.Camera cam;

        private void Awake()
        {
            cam = GetComponent<UnityEngine.Camera>();
        }

        private void LateUpdate()
        {
            FitRotation();
            if (move) FitPosition();
        }

        void FitRotation()
        {
            Vector3 camPos = cam.transform.position;
            Vector3 planePos = plane.position;
            Vector3 dd = (planePos - camPos).normalized;

            float yy =
                Mathf.Atan2(dd.x, dd.z) *
                Mathf.Rad2Deg;

            float pp =
                Mathf.Asin(dd.y) *
                Mathf.Rad2Deg;

            Quaternion targetRotation =
                Quaternion.Euler(
                    -pp,
                    yy,
                    0f
                );

            cam.transform.rotation = targetRotation;
        }

        private void FitPosition()
        {
            Vector3 playerPos = PlayerInstance.Instance.transform.position;
            Vector3 pos = transform.position;
            if (movementAxis.x != 0) pos.x = playerPos.x;
            if (movementAxis.y != 0) pos.y = playerPos.y + 1;
            if (movementAxis.z != 0) pos.z = playerPos.z;
            transform.position = pos;
        }
    }
}
