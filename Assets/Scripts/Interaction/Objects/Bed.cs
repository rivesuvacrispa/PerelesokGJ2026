using UnityEngine;

namespace Interaction.Objects
{
    public class Bed : MonoBehaviour, IInteractable
    {
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
            Debug.Log("Interact with bed");
        }
    }
}