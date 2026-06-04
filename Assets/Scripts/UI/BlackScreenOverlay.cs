using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BlackScreenOverlay : MonoBehaviour
    {
        public static BlackScreenOverlay Instance { get; private set; }
        private BlackScreenOverlay() => Instance = this;
        
        [SerializeField] private Image image;

        public IEnumerator FadeIn(float duration = 1f)
        {
            image.enabled = true;
            Color color = image.color;
            color.a = 0f;
            image.color = color;

            float t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                float progress = Mathf.Clamp01(t / duration);
                color.a = progress;
                image.color = color;
                yield return null;
            }

            color.a = 1f;
            image.color = color;
        }

        public IEnumerator FadeOut(float duration = 1f)
        {
            image.enabled = true;
            Color color = image.color;
            color.a = 1f;
            image.color = color;
            float t = 0f;
            
            while (t < duration)
            {
                t += Time.deltaTime;
                float progress = Mathf.Clamp01(t / duration);
                color.a = 1f - progress;
                image.color = color;
                yield return null;
            }

            color.a = 0f;
            image.color = color;
            image.enabled = false;
        }
    }
}