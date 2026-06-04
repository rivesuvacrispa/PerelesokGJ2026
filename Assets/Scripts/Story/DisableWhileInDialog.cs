using System.Collections.Generic;
using UnityEngine;

namespace Story
{
    public class DisableWhileInDialog : MonoBehaviour
    {
        [SerializeField] private List<Behaviour> componentsToDisable = new();
        [SerializeField] private List<GameObject> gameObjectsToDisable = new();


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
            foreach (GameObject g in gameObjectsToDisable)
            {
                g.SetActive(true);
            }
        }

        private void OnDialogStart()
        {
            foreach (Behaviour b in componentsToDisable)
            {
                b.enabled = false;
            }
            foreach (GameObject g in gameObjectsToDisable)
            {
                g.SetActive(false);
            }
        }
    }
}