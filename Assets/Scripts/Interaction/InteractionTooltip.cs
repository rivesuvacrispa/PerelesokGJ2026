using Controls;
using Player;
using TMPro;
using UnityEngine;

namespace Interaction
{
    public class InteractionTooltip : MonoBehaviour
    {
        [SerializeField] private TMP_Text keyCodeText;
        [SerializeField] private TMP_Text labelText;


        private void Awake()
        {
            keyCodeText.SetText(ControlsManager.Instance.InteractionKey.ToString());
            InteractionManager.OnEnterInteractable += OnEnterInteractable;
            InteractionManager.OnExitInteractable += OnExitInteractable;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            InteractionManager.OnEnterInteractable -= OnEnterInteractable;
            InteractionManager.OnExitInteractable -= OnExitInteractable;
        }

        private void Update()
        {
            UpdateRotation();
        }

        private void UpdateRotation()
        {
            transform.rotation = Quaternion.LookRotation(
                (transform.position - PlayerCamera.Instance.transform.position).normalized,
                Vector3.up
            );
        }

        private void OnExitInteractable(IInteractable target)
        {
            gameObject.SetActive(false);
        }

        private void OnEnterInteractable(IInteractable target)
        {
            transform.position = ((Component)target).gameObject.transform.position;
            UpdateRotation();
            labelText.SetText(target.GetLabel());
            gameObject.SetActive(true);
        }
    }
}