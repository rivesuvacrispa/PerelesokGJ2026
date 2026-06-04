using System;
using System.Collections.Generic;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace Interaction.Objects
{
    public class MirrorShard : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particles;

        public Rigidbody Rigidbody { get; private set; }

        public static readonly List<MirrorShard> SHARDS = new();
        public static int ShardsCount => SHARDS.Count;
        public static event Action OnShardSpawn;
        

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            SHARDS.Add(this);
            OnShardSpawn?.Invoke();
        }

        public void PlayParticles()
        {
            particles.Play();
        }

        public bool IsPlaying => particles.IsAlive();

        private void Update()
        {
            if (transform.position.y < -1)
            {
                ResetRigidbody();
            }
        }

        public void ResetRigidbody()
        {
            transform.position = PhysUtils.roomCenter + Random.insideUnitSphere.normalized * 0.25f;
            Rigidbody.angularVelocity = Vector3.zero;
            Rigidbody.linearVelocity = Vector3.zero;
        }

        public static void ClearAll()
        {
            foreach (MirrorShard shard in SHARDS)
            {
                Destroy(shard.gameObject);
            }
            SHARDS.Clear();
        }
    }
}