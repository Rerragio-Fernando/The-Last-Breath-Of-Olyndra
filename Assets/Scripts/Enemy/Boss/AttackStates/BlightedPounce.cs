using UnityEngine;
namespace NPC
{
    [CreateAssetMenu(menuName = "AI/States/Attacks/Pounces")]
    public class BlightedPounce : BaseAttackState
    {
        Transform bossTransform;
        [SerializeField] private float leapSpeed = 10f;
        [SerializeField] private float leapDistance = 5f; 
        [SerializeField] private float impactRadius = 2f; 
        [SerializeField] private LayerMask targetLayer;
        private bool isLeaping = false;
        Vector3 targetPosition = Vector3.zero;
        Transform playerPos;

        public override BossAIState stateTick(BossAIManager bossAI)
        {
            BossAIState stateToReturn = this;
            bossTransform = bossAI.getBossTransform();
            Debug.Log(isLeaping);
            if (!isOnCooldown() && !isLeaping)
            {
                Debug.Log("In here");
                playerPos = bossAI.getSetCurrentTarget;
                Activate();
            }

            if(isLeaping)
            {
                Debug.Log("How the shit am I here if I never logged the one before");
                LeapTowardsTarget();
                CheckImpact();
                visualizeAbility();
            }
            stateToReturn = nextState == null ? stateToReturn : nextState;
            checkIfCooldownNeedReset(stateToReturn);
            return stateToReturn;
        }

        public override void Activate()
        {
            Debug.Log($"{abilityName} activated!");
            if(playerPos)
            {
                targetPosition = playerPos.position;
                isLeaping = true;
            }
            else
            {
                targetPosition = Vector3.zero;
            }
            startCooldown();

        }

        public override void resetValues()
        {
            isLeaping = false;
            targetPosition = Vector3.zero;
        }

        private void LeapTowardsTarget()
        {
            if (targetPosition == Vector3.zero)
            {
                //Debug.LogWarning("Player position is null!");
                return;
            }
            Vector3 direction = (targetPosition - bossTransform.position).normalized;
            Vector3 leapMovement = direction * leapSpeed * Time.deltaTime;
            bossTransform.position = Vector3.MoveTowards(bossTransform.position, targetPosition, leapMovement.magnitude);
            Debug.Log($"Leap direction: {direction}, Leap movement: {leapMovement}");

            if (Vector3.Distance(bossTransform.position, targetPosition) <= 1.0f)
            {
                isLeaping = false;
            }
        }

        private void CheckImpact()
        {
            Collider[] hitTargets = Physics.OverlapSphere(bossTransform.position, impactRadius, targetLayer);
            foreach (var hit in hitTargets)
            {
                if (hit.CompareTag("Player"))
                {
                    // TODO Apply high damage to Ilyra
                    Debug.Log("Ilyra hit! Taking high damage!");
                    // TODO add knockback or other effects here
                }
            }
        }

        protected override void visualizeAbility()
        {
            //TODO set animation

        }
    }
}
