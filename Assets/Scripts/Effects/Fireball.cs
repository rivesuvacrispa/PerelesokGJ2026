using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Effects
{
    [RequireComponent(typeof(Rigidbody))]
    public class Fireball : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            rb.AddForce(Random.insideUnitSphere.normalized * 100);
        }

        private void OnCollisionEnter(Collision collision)
        {
            rb.linearVelocity *= 1.25f;
        }
    }
}