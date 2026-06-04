using System.Collections;
using Global;
using Story;
using UI;
using UnityEngine;

namespace Interaction.Objects
{
    public class IceKey : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogEntry endGameDialog;
        private bool canInteract = true;
        
        public string GetLabel()
        {
            return "Использовать";
        }

        public bool CanInteract()
        {
            return canInteract;
        }

        public void Interact()
        {
            canInteract = false;
            StartCoroutine(InteractRoutine());
        }

        private IEnumerator InteractRoutine()
        {
            gameObject.transform.parent.SetParent(null, true);
            yield return new WaitForEndOfFrame();
            
            Vector3 targetPos = GlobalDefinitions.Safe.transform.position + Vector3.up * 0.1f;
            float t = 0;
            while (t < 2f)
            {
                Vector3 currentPos = transform.parent.position;
                float delta = Vector3.Distance(currentPos, targetPos);
                transform.parent.position = Vector3.MoveTowards(currentPos, targetPos, delta * t / 2f);
                t += Time.deltaTime;
                yield return null;
            }

            GlobalDefinitions.Safe.Play();
            yield return BlackScreenOverlay.Instance.FadeIn(3f);
            transform.parent.localScale = Vector3.zero;
            DialogManager.Instance.StartDialog(endGameDialog);
        }
    }
}