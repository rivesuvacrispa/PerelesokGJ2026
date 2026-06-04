using System.Collections;
using UI;
using UnityEngine;

namespace Interaction.Objects
{
    public class Bed : MonoBehaviour, IInteractable
    {
        public static int DayCounter { get; private set; } = 0;
        public static event System.Action OnSleep;

        private Coroutine currentRoutine;
        
        
        
        
        
        public string GetLabel()
        {
            return "Лечь спать";
        }

        public bool CanInteract()
        {
            return currentRoutine is null;
        }

        public void Interact()
        {
            currentRoutine = StartCoroutine(InteractRoutine());
        }

        private IEnumerator InteractRoutine()
        {
            yield return BlackScreenOverlay.Instance.FadeIn();
            OnSleep?.Invoke();
            DayCounter++;
            yield return new WaitForSeconds(1f);
            yield return BlackScreenOverlay.Instance.FadeOut();
            currentRoutine = null;
        }
    }
}