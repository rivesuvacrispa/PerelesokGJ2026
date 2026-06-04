using Effects.SpellBalls;
using Player;
using UnityEngine;

namespace Spells
{
    [CreateAssetMenu(menuName = "Spell")]
    public class ScriptableSpell : ScriptableObject
    {
        [SerializeField] private string title;
        [SerializeField] private SpellBall ball;


        public string Title => title;
        
        public void Cast()
        {
            Transform t = PlayerCamera.Instance.transform;
            Vector3 spawnPos = t.position - new Vector3(0, 0.5f, 0) + t.forward * 0.2f;
            SpellBall b = Instantiate(ball, spawnPos, t.rotation);
            b.Rigidbody.AddForce(t.forward * SpellCaster.SpellBallCastSpeed);
        }
    }
}