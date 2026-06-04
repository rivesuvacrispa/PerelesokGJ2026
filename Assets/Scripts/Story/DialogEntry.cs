using System.Collections.Generic;
using Player;
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
        [SerializeField] private DialogEvent storyEvent;
        [SerializeField] private PlayerSpriteMode spriteMode = PlayerSpriteMode.Normal;
        
        public string Text => text;
        public DialogEntry NextEntry => nextEntry;
        public List<DialogEntry> Options => options;
        public bool HasEvent => hasEvent;
        public DialogEvent StoryEvent => storyEvent;

        public PlayerSpriteMode SpriteMode => spriteMode;
    }
}