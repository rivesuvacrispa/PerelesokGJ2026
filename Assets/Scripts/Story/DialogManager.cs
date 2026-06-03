using System;
using System.Collections.Generic;
using System.Linq;
using Controls;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Story
{
    public class DialogManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text mainText;
        [SerializeField] private Transform optionsTransform;

#if UNITY_EDITOR
        [SerializeField] private DialogEntry debug_DialogToStart;
#endif

        public static DialogManager Instance { get; private set; }
        private List<TMP_Text> optionsList = new();
        private DialogEntry currentEntry;
        
        public delegate void DialogManagerEvent();
        public static event DialogManagerEvent OnDialogStart;
        public static event DialogManagerEvent OnDialogEnd;


        
        private DialogManager() => Instance = this;


#if UNITY_EDITOR
        private void Start()
        {
            if (debug_DialogToStart is not null)
                StartDialog(debug_DialogToStart);
        }
#endif

        private void OnEnable()
        {
            ControlsManager.OnNextDialog += NextEntry;
        }

        private void OnDisable()
        {
            ControlsManager.OnNextDialog -= NextEntry;
        }

        public void StartDialog(DialogEntry entry)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            PlayEntry(entry);
            optionsTransform.gameObject.SetActive(true);
            OnDialogStart?.Invoke();
        }

        private void EndDialog()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            optionsTransform.gameObject.SetActive(false);
            ClearOptions();
            currentEntry = null;
            OnDialogEnd?.Invoke();
        }

        private void ClearOptions()
        {
            foreach (TMP_Text text in optionsList)
                text.gameObject.SetActive(false);
        }

        private void PlayEntry(DialogEntry entry)
        {
            ClearOptions();
            currentEntry = entry;
            mainText.SetText(entry.Text);
            for (var i = 0; i < entry.Options.Count; i++)
            {
                var opt = entry.Options[i];
                CreateOption($"{i + 1}. — ", opt);
            }
        }

        /**
         * Скипает текущий шаг диалога и включает следующий.
         * Шаг нельзя скипнуть, если есть варианты ответа, которые надо выбрать.
         */
        private void NextEntry()
        {
            if (currentEntry is null || currentEntry.Options.Count != 0) return;

            MakeStep(currentEntry.NextEntry);
        }

        /**
         * Переключает шаг диалога, запуская ивент кончившегося шага.
         * Если шаг null, то диалог заканчивается.
         */
        private void MakeStep(DialogEntry entry)
        {
            if (currentEntry is not null && currentEntry.HasEvent) 
                currentEntry.StoryEvent.Fire();
            
            if (entry is not null) PlayEntry(entry);
            else EndDialog();
        }

        private void CreateOption(string prefix, DialogEntry option)
        {
            TMP_Text opt = optionsList.FirstOrDefault(text => !text.gameObject.activeInHierarchy);
            if (opt is null)
            {
                opt = Instantiate(mainText, optionsTransform);
                opt.color = Color.white;
                optionsList.Add(opt);
            }

            opt.SetText(prefix + option.Text);
            opt.GetComponent<Button>().onClick.AddListener(() => MakeStep(option.NextEntry));
        }
    }
}