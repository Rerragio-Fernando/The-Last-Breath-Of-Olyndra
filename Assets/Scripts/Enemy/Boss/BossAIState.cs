using UnityEngine;

namespace NPC
{
    public class BossAIState : ScriptableObject
    {
        [Header("Next State")]
        [SerializeField] protected BossAIState nextState;

        [Header("Boss Animator")]
        [SerializeField] protected Animator animator;
        public virtual BossAIState stateTick(BossAIManager bossAI)
        {
            return this;
        }
    }
}

