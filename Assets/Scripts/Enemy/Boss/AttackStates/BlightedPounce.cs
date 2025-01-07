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
        Vector3 targetPosition = Vector3.zero;
        Transform playerPos;

        public override BossAIState stateTick(BossAIManager bossAI)
        {
            BossAIState stateToReturn = this;
            bossTransform = bossAI.getBossTransform();
            if (!isOnCooldown()&& !isActive)
            {
                playerPos = bossAI.getSetCurrentTarget;
                bossAI.getSetCurrentTarget = null;
                Activate();

            }

            if(isActive)
            {
                LeapTowardsTarget();
                checkLeapEnd();
                visualizeAbility();
            }
            if (checkCooldownStateChange())
            {
                Debug.Log("cooldown done pounce");
                stateToReturn = nextState == null ? stateToReturn : nextState;
            }
            checkIfCooldownNeedReset(stateToReturn);
            bossAI.trainAnn();
            return stateToReturn;
        }

        public override void Activate()
        {
            Debug.Log($"{abilityName} activated!");
            if(playerPos)
            {
                targetPosition = playerPos.position;
                isActive = true;
            }
            else
            {
                targetPosition = Vector3.zero;
            }

        }

        public override void applyDamage(PlayerHealth playerHealth)
        {
            if(playerHealth)
            {
                playerHealth.applyDamage(damage);
            }
            // TODO add knockback or other effects here
        }

        public override void resetValues()
        {
            isActive = false;
            targetPosition = Vector3.zero;
            resetCooldown();
        }

        private void LeapTowardsTarget()
        {
            if (targetPosition == Vector3.zero)
            {
                Debug.LogWarning("Player position is null!");
                return;
            }
            Vector3 direction = (targetPosition - bossTransform.position).normalized;
            Vector3 leapMovement = direction * leapSpeed * Time.deltaTime;
            bossTransform.position = Vector3.MoveTowards(bossTransform.position, targetPosition, leapMovement.magnitude);
        }

        private void checkLeapEnd()
        {
            Vector3 bossPositionXZ = new Vector3(bossTransform.position.x, 0, bossTransform.position.z);
            Vector3 targetPositionXZ = new Vector3(targetPosition.x, 0, targetPosition.z);

            if (Vector3.Distance(bossPositionXZ, targetPositionXZ) <= impactRadius)
            {
                isActive = false;
                checkImpact();
                
            }
        }

        private void checkImpact()
        {
            Collider[] hitTargets = Physics.OverlapSphere(bossTransform.position, impactRadius * 2f, targetLayer);
            foreach (var hit in hitTargets)
            {
                if (hit.CompareTag("Player"))
                {
                    // TODO Apply high damage to Ilyra
                    PlayerHealth health = hit.gameObject.GetComponent<PlayerHealth>();
                    if (health)
                    {
                        applyDamage(health);
                    }
                    

                }
            }
            startCooldown();
        }

        protected override void visualizeAbility()
        {
            //TODO set animation

        }
    }
}
