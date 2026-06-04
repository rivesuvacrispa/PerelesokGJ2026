using System;
using Controls;
using Player;
using UnityEngine;

namespace Interaction
{
    public class InteractionManager : MonoBehaviour
    {
        [SerializeField] private LayerMask interactionMask;
        [SerializeField] private float interactionDistance = 1.5f;
        public static InteractionManager Instance { get; private set; }

        private InteractionManager() => Instance = this;
        private Transform camTransform = null;
        public IInteractable CurrentInteractable { get; private set; }

        public delegate void InteractionManagerEvent(IInteractable target);

        public static event InteractionManagerEvent OnEnterInteractable;
        public static event InteractionManagerEvent OnExitInteractable;
        
        
        

        private void Awake()
        {
            camTransform = PlayerCamera.Instance.transform;
        }

        private void OnEnable()
        {
            ControlsManager.OnInteractionPress += OnInteractionKeyPress;
        }

        private void OnDisable()
        {
            ControlsManager.OnInteractionPress -= OnInteractionKeyPress;
        }

        private void OnInteractionKeyPress()
        {
            if (CurrentInteractable is not null && CurrentInteractable.CanInteract())
                CurrentInteractable.Interact();
        }

        private Ray CreateRay()
        {
            if (camTransform is null) return default;

            Vector3 origin = camTransform.position;
            Vector3 direction = camTransform.forward;
            return new Ray(origin, direction);
        }

        private void Update()
        {
            IInteractable interactable = null;

            Ray ray = CreateRay();
            bool intersect = Physics.Raycast(ray, out RaycastHit info, interactionDistance, interactionMask);
            bool hasInteractable = intersect && info.collider.gameObject.TryGetComponent(out interactable);
            
            if (hasInteractable && interactable.CanInteract())
            {
                if (CurrentInteractable != interactable) OnEnterInteractable?.Invoke(interactable);
                CurrentInteractable = interactable;
            }
            else
            {
                if (CurrentInteractable != null) OnExitInteractable?.Invoke(CurrentInteractable);
                CurrentInteractable = null;
            }
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(CreateRay());
        }
    }
}