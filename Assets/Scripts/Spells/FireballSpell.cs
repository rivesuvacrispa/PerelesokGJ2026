using Effects;
using Player;
using UnityEngine;

namespace Spells
{
    public class FireballSpell : Spell
    {
        [SerializeField] private SpellBall ball;
        
        public override void Cast()
        {
            Transform t = PlayerCamera.Instance.transform;
            Vector3 spawnPos = t.position - new Vector3(0, 0.5f, 0) + t.forward * 0.2f;
            SpellBall b = Instantiate(ball, spawnPos, t.rotation);
            b.Rigidbody.AddForce(t.forward * SpellCaster.SpellBallCastSpeed);
        }
    }
}