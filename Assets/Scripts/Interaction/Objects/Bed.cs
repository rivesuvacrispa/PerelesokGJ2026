using System;
using System.Collections;
using Player;
using Story;
using UI;
using UnityEngine;
using Util;

namespace Interaction.Objects
{
    public class Bed : MonoBehaviour, IInteractable
    {
        [Header("First day")]
        [SerializeField] private PlotMemory firstDaySleepMem;
        
        [Header("Second day")]
        [SerializeField] private DialogEntry secondDayDialog;

        [Header("Third day")]
        [SerializeField] private DialogEntry thirdDayDialog;
        [SerializeField] private DialogEntry helpMeCrackMirrorDialog;
        [SerializeField] private PlotMemory mirrorCrackedMem;

        [Header("Fourth day")]
        [SerializeField] private DialogEntry day4Dialog;
        
        [Header("3 shards dialog")]
        [SerializeField] private DialogEntry threeShardsDialog;
        
        [Header("Other")]
#if UNITY_EDITOR
        [SerializeField] private int debug_DayOnStart = 1;
#endif
        [SerializeField] private DialogEntry iDontWannaSleepDialog;

        public static int DayCounter { get; private set; } = 1;
        public static event System.Action OnSleep;

        private Coroutine currentRoutine;

        
        
#if UNITY_EDITOR
        private void Awake()
        {
            DayCounter = debug_DayOnStart;
        }
#endif
        private bool WantsToSleep()
        {
#if UNITY_EDITOR
            if (debug_DayOnStart != 1) return true;
#endif
            
            switch (DayCounter)
            {
                case 1:
                    return MemoryManager.HasMemory(firstDaySleepMem);
            }

            return true;
        }

        public string GetLabel()
        {
            return "Лечь спать";
        }

        public bool CanInteract()
        {
            return currentRoutine is null;
        }

        public void Interact()
        {
            if (WantsToSleep())
                currentRoutine = StartCoroutine(InteractRoutine());
            else 
                DialogManager.Instance.StartDialog(iDontWannaSleepDialog);
        }

        private IEnumerator InteractRoutine()
        {
            yield return BlackScreenOverlay.Instance.FadeIn();
            OnSleep?.Invoke();
            DayCounter++;
            print($"Day counter: {DayCounter}");
            yield return new WaitForSeconds(1f);
            yield return BlackScreenOverlay.Instance.FadeOut();
            currentRoutine = null;

            FireNewDayDialog();
        }

        private void FireNewDayDialog()
        {
            Vector3 closestMirror = PhysUtils.FindClosestMirrorPoint(PlayerCamera.Instance.transform.position);
            
            if (DayCounter == 2)
                DialogManager.Instance.StartDialog(secondDayDialog, closestMirror);
            
            else if (DayCounter == 3)
            {
                bool needsHelp = !MemoryManager.HasMemory(mirrorCrackedMem);
                DialogManager.Instance.StartDialog(needsHelp ? helpMeCrackMirrorDialog : thirdDayDialog, closestMirror);
            }
            
            else if (DayCounter == 4)
            {
                DialogManager.Instance.StartDialog(day4Dialog, closestMirror);
            }

            else if (DayCounter > 4 && MirrorShard.ShardsCount >= 3)
            {
                DialogManager.Instance.StartDialog(threeShardsDialog, closestMirror);
            }
        }
    }
}