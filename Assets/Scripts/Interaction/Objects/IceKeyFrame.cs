using System;
using System.Collections;
using Effects.SpellBalls;
using Spells;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace Interaction.Objects
{
    public class IceKeyFrame : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private GameObject key;
        [SerializeField] private GameObject frame;
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private SpellballReceiver receiver;

        private void Awake()
        {
            receiver.OnSpellBallReceived += OnSpellBallReceived;
        }

        private void OnSpellBallReceived(SpellBall spellball)
        {
            if (spellball is IceBall)
                key.gameObject.SetActive(true);
        }

        public IEnumerator SpawnAnimationRoutine()
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            key.gameObject.SetActive(false);
            frame.gameObject.SetActive(false);
            transform.position = PhysUtils.roomCenter;
            transform.rotation = Quaternion.identity;
            gameObject.SetActive(true);
            particles.Play();
            yield return null;

            yield return new WaitUntil(() => !particles.IsAlive());
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            frame.gameObject.SetActive(true);
            
            ResetRigidbody();
        }
        
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
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
        }
    }
}