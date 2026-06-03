namespace Interaction
{
    public interface IInteractable
    {
        public string GetLabel();
        public bool CanInteract();
        public void Interact();
    }
}