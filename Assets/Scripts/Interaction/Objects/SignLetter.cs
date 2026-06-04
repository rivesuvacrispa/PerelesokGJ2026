using System.Collections;
using Story;
using UnityEngine;

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
        
        public string GetLabel()
        {
            return "Прочитать";
        }

        public bool CanInteract()
        {
            return !PlotManager.HasMemory(letterWasBurnedMem);
        }

        public void Interact()
        {
            bool firstTime = !PlotManager.HasMemory(firstEncounterFinishedMem);
            
            DialogManager.Instance.StartDialog(firstTime ? firstEncounterDialogEntry : letterDialogEntry);
        }

        public void Burn()
        {
            PlotManager.AddMemory(letterWasBurnedMem);
            burnParticles.Play();
            StartCoroutine(BurnRoutine());
        }

        private IEnumerator BurnRoutine()
        {
            yield return new WaitForSeconds(burnParticles.main.duration / 2f);
            spriteRenderer.enabled = false;
            yield return new WaitUntil(() => !burnParticles.isPlaying);
            Destroy(gameObject);
        }
    }
}