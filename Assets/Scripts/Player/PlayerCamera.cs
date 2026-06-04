using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera Instance { get; private set; }

        private PlayerCamera() => Instance = this;



        public void RotateToLook(Vector3 point)
        {
            StopAllCoroutines();
            StartCoroutine(RotateRoutine(point, 1f));
        }

        private IEnumerator RotateRoutine(Vector3 point, float duration)
        {
            float t = 0f;

            Vector3 direction = (point - transform.position).normalized;
            Quaternion startRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

            while (t < duration)
            {
                t += Time.deltaTime;

                float progress = Mathf.Clamp01(t / duration);
                float easedProgress = 1f - Mathf.Pow(1f - progress, 3f);

                transform.rotation = Quaternion.Slerp(
                    startRotation,
                    targetRotation,
                    easedProgress);

                yield return null;
            }

            transform.rotation = targetRotation;
        }
    }
}