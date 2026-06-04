using System;
using System.Collections;
using Story;
using Story.Events;
using UI;
using UnityEngine;

namespace Interaction.Objects
{
    public class Sword : MonoBehaviour, IInteractable
    {
        [SerializeField] private Animator animator;
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private GameObject swordGo;
        [SerializeField] private DialogEntry endGameDialog;

        private bool canInteract;

        private void OnEnable()
        {
            StartCoroutine(AppearRoutine());
            canInteract = true;
        }

        private IEnumerator AppearRoutine()
        {
            particles.Play();
            yield return new WaitForSeconds(3f);
            swordGo.SetActive(true);
        }


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
            animator.Play("BreakthroughAnimation");
            StartCoroutine(EndGameRoutine());
        }

        private IEnumerator EndGameRoutine()
        {
            yield return BlackScreenOverlay.Instance.FadeIn(5f);
            DialogManager.Instance.StartDialog(endGameDialog);
        }
    }
}