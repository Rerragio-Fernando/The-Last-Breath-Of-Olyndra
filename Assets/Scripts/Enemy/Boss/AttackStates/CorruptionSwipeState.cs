using UnityEngine;

namespace NPC
{
    [CreateAssetMenu(menuName = "AI/States/Attacks")]
    public class CorruptionSwipeState : BaseAttackState
    {
        Transform bossTransform;
        [SerializeField]
        float attackRadius = 5f;
        [SerializeField]
        float attackAngle = 120f;
        [SerializeField]
        LayerMask targetLayer;
        public override BossAIState stateTick(BossAIManager bossAI)
        {
            BossAIState stateToReturn = this;
            bossTransform = bossAI.getBossTransform();
            if(!isOnCooldown())
            {
                Activate();
                visualizeAbility();
            }

            return stateToReturn;
        }

        public override void Activate()
        {
            Debug.Log($"{abilityName} activated!");
            //TODO set animation

            Collider[] hits = Physics.OverlapSphere(bossTransform.position, attackRadius, targetLayer);
            foreach (var hit in hits)
            {
                Vector3 directionToTarget = (hit.transform.position - bossTransform.position).normalized;
                float angle = Vector3.Angle(bossTransform.forward, directionToTarget);

                if (angle <= attackAngle / 2)
                {
                    // TODO damage the player
                    Debug.Log("Damaging player");
                }
            }

            startCooldown();

            
        }

        protected override void visualizeAbility()
        {

            Debug.Log("Visualizing ability effects if any");
        }
    }
}
