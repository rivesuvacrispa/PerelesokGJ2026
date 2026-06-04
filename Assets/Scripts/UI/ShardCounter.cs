using System;
using Interaction.Objects;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ShardCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        private void OnEnable()
        {
            MirrorShard.OnShardSpawn += OnShardSpawn;
        }
        
        private void OnDisable()
        {
            MirrorShard.OnShardSpawn -= OnShardSpawn;
        }

        private void OnShardSpawn()
        {
            text.SetText($"{MirrorShard.ShardsCount} / 3");
        }
    }
}