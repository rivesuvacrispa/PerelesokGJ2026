using UnityEngine;

namespace Controls
{
    public class ControlsManager : MonoBehaviour
    {
        [SerializeField] private KeyCode spellKey;
        
        public delegate void ControlsManagerEvent();
        public static event ControlsManagerEvent OnSpellPress;

        
        
        private void Update()
        {
            if (Input.GetKeyDown(spellKey))
                OnSpellPress?.Invoke();
        }
    }
}