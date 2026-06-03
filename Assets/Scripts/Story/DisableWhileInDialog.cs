using System.Collections.Generic;
using UnityEngine;

namespace Story
{
    public class DisableWhileInDialog : MonoBehaviour
    {
        [SerializeField] private List<Behaviour> componentsToDisable = new();


        private void Awake()
        {
            DialogManager.OnDialogStart += OnDialogStart;
            DialogManager.OnDialogEnd += OnDialogEnd;
        }

        private void OnDestroy()
        {
            DialogManager.OnDialogStart -= OnDialogStart;
            DialogManager.OnDialogEnd -= OnDialogEnd;
        }

        private void OnDialogEnd()
        {
            foreach (Behaviour b in componentsToDisable)
            {
                b.enabled = true;
            }
        }

        private void OnDialogStart()
        {
            foreach (Behaviour b in componentsToDisable)
            {
                b.enabled = false;
            }
        }
    }
}