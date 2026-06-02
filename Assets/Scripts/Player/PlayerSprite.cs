using System;
using UnityEngine;

namespace Player
{
    public class PlayerSprite : MonoBehaviour
    {
        [SerializeField] private Animator[] animators;
        
        private Transform playerTransform;

        private readonly int frontIdleAnimHash = Animator.StringToHash("Front Idle");
        private readonly int frontWalkAnimHash = Animator.StringToHash("Front Walk");
        private readonly int backIdleAnimHash = Animator.StringToHash("Back Idle");
        private readonly int backWalkAnimHash = Animator.StringToHash("Back Walk");
        private readonly int leftIdleAnimHash = Animator.StringToHash("Left Idle");
        private readonly int leftWalkAnimHash = Animator.StringToHash("Left Walk");
        private readonly int rightIdleAnimHash = Animator.StringToHash("Right Idle");
        private readonly int rightWalkAnimHash = Animator.StringToHash("Right Walk");

        private int[] lookDirectionToIdleAnim;
        private int[] lookDirectionToWalkAnim;
        
        private void Awake()
        {
            playerTransform = PlayerInstance.Instance.transform;
            lookDirectionToIdleAnim = new[]
            {
                frontIdleAnimHash,
                leftIdleAnimHash,
                backIdleAnimHash,
                rightIdleAnimHash
            };
            lookDirectionToWalkAnim = new[]
            {
                frontWalkAnimHash,
                leftWalkAnimHash,
                backWalkAnimHash,
                rightWalkAnimHash
            };
        }

        private int GetAnim(bool isMoving, int lookDirection)
        {
            return isMoving ? lookDirectionToWalkAnim[lookDirection] : lookDirectionToIdleAnim[lookDirection];
        }

        private void UpdatePosition()
        {
            transform.position = playerTransform.position;
        }

        private void UpdateSprites()
        {
            bool isMoving = PlayerInstance.Instance.Rigidbody.linearVelocity.sqrMagnitude > 0.09f;
            float y = playerTransform.eulerAngles.y;
            int lookDirection = Mathf.RoundToInt(y / 90f) % 4;
            
            // 0 - Z+
            // 1 - X+
            // 2 - Z-
            // 3 - X-
            for (var i = 0; i < animators.Length; i++)
            {
                Animator a = animators[i];
                a.Play(GetAnim(isMoving, (lookDirection + i) % 4));
            }
        }

        private void Update()
        {
            UpdatePosition();
        }

        private void FixedUpdate()
        {
            UpdateSprites();
        }
    }
}