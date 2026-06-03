using System.Collections.Generic;
using Story.Events;
using UnityEngine;

namespace Story
{
    [CreateAssetMenu(menuName = "Dialog")]
    public class DialogEntry : ScriptableObject
    {
        [SerializeField] private string text;
        [SerializeField] private DialogEntry nextEntry;
        [SerializeField] private List<DialogEntry> options = new();
        [SerializeField] private bool hasEvent;
        [SerializeField] private StoryEvent storyEvent;

        public string Text => text;
        public DialogEntry NextEntry => nextEntry;
        public List<DialogEntry> Options => options;
        public bool HasEvent => hasEvent;
        public StoryEvent StoryEvent => storyEvent;
    }
}