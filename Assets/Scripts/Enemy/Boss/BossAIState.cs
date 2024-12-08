using UnityEngine;

namespace NPC
{
    public class BossAIState : ScriptableObject
    {
        public virtual BossAIState stateTick(BossAIManager bossAI)
        {
            return this;
        }
    }
}

