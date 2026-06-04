using System;
using System.Collections;
using Effects.SpellBalls;
using Player;
using Spells;
using Story;
using UnityEngine;
using Util;

namespace Interaction.Objects
{
    public class SignLetter : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogEntry letterDialogEntry;
        [SerializeField] private DialogEntry firstEncounterDialogEntry;
        [SerializeField] private PlotMemory firstEncounterFinishedMem;
        [SerializeField] private PlotMemory letterWasBurnedMem;
        [SerializeField] private ParticleSystem burnParticles;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private SpellballReceiver receiver;

        private void Awake()
        {
            receiver.OnSpellBallReceived += OnSpellballReceived;
        }

        private void OnSpellballReceived(SpellBall spellball)
        {
            if (spellball is Fireball && !MemoryManager.HasMemory(letterWasBurnedMem))
                Burn();
        }

        public string GetLabel()
        {
            return "Прочитать";
        }

        public bool CanInteract()
        {
            return !MemoryManager.HasMemory(letterWasBurnedMem);
        }

        public void Interact()
        {
            bool firstTime = !MemoryManager.HasMemory(firstEncounterFinishedMem);

            Vector3 cameraLookAt = PhysUtils.FindClosestMirrorPoint(PlayerCamera.Instance.transform.position);
            DialogManager.Instance.StartDialog(
                firstTime ? firstEncounterDialogEntry : letterDialogEntry,
                firstTime ? cameraLookAt : default);
        }

        public void Burn()
        {
            MemoryManager.AddMemory(letterWasBurnedMem);
            burnParticles.Play();
            StartCoroutine(BurnRoutine());
        }

        private IEnumerator BurnRoutine()
        {
            yield return new WaitForSeconds(burnParticles.main.duration / 2f);
            spriteRenderer.enabled = false;
            yield return new WaitUntil(() => !burnParticles.IsAlive());
            Destroy(gameObject);
        }
    }
}