using UnityEngine;

namespace Interaction.Objects
{
    public class Bed : MonoBehaviour, IInteractable
    {
        public static event System.Action OnSleep; 

        public string GetLabel()
        {
            return "Лечь спать";
        }

        public bool CanInteract()
        {
            return true;
        }

        public void Interact()
        {
            OnSleep?.Invoke();
        }
    }
}