using UnityEngine;

namespace Controls
{
    public class ControlsManager : MonoBehaviour
    {
        public static ControlsManager Instance { get; private set; }
        
        [SerializeField] private KeyCode spellKey = KeyCode.F;
        [SerializeField] private KeyCode interactionKey = KeyCode.E;
        
        public delegate void ControlsManagerEvent();
        public static event ControlsManagerEvent OnSpellPress;
        public static event ControlsManagerEvent OnInteractionPress;

        public KeyCode SpellKey => spellKey;

        public KeyCode InteractionKey => interactionKey;


        private ControlsManager() => Instance = this;
        
        private void Update()
        {
            if (Input.GetKeyDown(spellKey))
                OnSpellPress?.Invoke();
            
            if (Input.GetKeyDown(interactionKey))
                OnInteractionPress?.Invoke();
        }
    }
}