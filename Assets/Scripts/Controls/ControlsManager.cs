using UnityEngine;

namespace Controls
{
    public class ControlsManager : MonoBehaviour
    {
        public static ControlsManager Instance { get; private set; }
        
        [SerializeField] private KeyCode interactionKey = KeyCode.E;
        [SerializeField] private KeyCode nextDialogKey = KeyCode.Space;
        
        public delegate void ControlsManagerEvent();
        public static event ControlsManagerEvent OnInteractionPress;
        public static event ControlsManagerEvent OnNextDialog;

        public delegate void NumberPressEvent(int index);
        public static event NumberPressEvent OnNumberPress;


        public KeyCode InteractionKey => interactionKey;


        private ControlsManager() => Instance = this;
        
        private void Update()
        {
            if (Input.GetKeyDown(interactionKey))
                OnInteractionPress?.Invoke();
            
            if (Input.GetKeyDown(nextDialogKey))
                OnNextDialog?.Invoke();

            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
                OnNumberPress?.Invoke(1);
            
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
                OnNumberPress?.Invoke(2);
            
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad3))
                OnNumberPress?.Invoke(3);
        }
    }
}