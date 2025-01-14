/*
 * ----------------------------------------------------------------------------------------------
 * Project: The Last Breath Of Olyndra                                                          *
 * Script: BlightedPounce                                                        *
 * Author: Marco Minganna                                                                       *
 * Unit: Digital Studio Project                                                                 *
 * Institution: Kingston University                                                             *
 *                                                                                              *
 * Date: September 2024 - January 2025                                                          *
 *                                                                                              *
 * Description:                                                                                 *
 * This script was developed as part of the coursework for the "DSP" unit at                    *
 * Kingston University.                                                                         *
 *                                                                                              *
 * License:                                                                                     *
 * This script is provided as-is for educational purposes. It is classified as Public and       *
 * may be shared, modified, or used with proper attribution to the original author, Marco       *
 * Minganna. Commercial use requires prior written consent.                                     *
 *                                                                                              *
 * Security Classification: Public                                                              *
 * ----------------------------------------------------------------------------------------------
 */
using UnityEngine;
namespace NPC
{
    [CreateAssetMenu(menuName = "AI/States/Attacks/Pounces")]
    public class BlightedPounce : BaseAttackState
    {
        Transform bossTransform;
        [SerializeField] private float leapSpeed = 10f;
        [SerializeField] private float impactRadius = 2f;
        [SerializeField] private LayerMask targetLayer;
        Vector3 targetPosition = Vector3.zero;
        Transform playerPos;

        bool impactReached = false;

        public override BossAIState stateTick(BossAIManager bossAI)
        {
            BossAIState stateToReturn = this;
            bossTransform = bossAI.getBossTransform();
            if (!isOnCooldown() && !isActive)
            {
                playerPos = bossAI.getSetCurrentTarget;

                if (playerPos != null)
                {
                    bossAI.getSetCurrentTarget = null;
                    Activate();
                    visualizeAbility(bossAI);
                }
            }

            if (isActive)
            {
                LeapTowardsTarget();
                checkLeapEnd();
                if(impactReached)
                {
                    bossAI.playHapticEffect();
                }
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
            if (playerPos)
            {
                targetPosition = playerPos.position;
            }
            else
            {
                Debug.LogWarning("Player position not set, defaulting to zero.");
                targetPosition = Vector3.zero;
            }

            isActive = true;
        }

        public override void applyDamage(PlayerHealth playerHealth)
        {
            if (playerHealth)
            {
                playerHealth.applyDamage(damage);
            }
            // TODO add knockback or other effects here
        }

        public override void resetValues()
        {
            isActive = false;
            targetPosition = Vector3.zero;
            impactReached = false;
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

            // Ensure the target is not behind the boss
            if (Vector3.Dot(direction, bossTransform.forward) < 0)
            {
                Debug.LogWarning("Target is behind the boss, adjusting target direction.");
                targetPosition = bossTransform.position + bossTransform.forward * 2f; // Redirect to a forward position
                direction = (targetPosition - bossTransform.position).normalized;
            }

            Vector3 leapMovement = direction * leapSpeed * Time.deltaTime;
            bossTransform.position = Vector3.MoveTowards(bossTransform.position, targetPosition, leapMovement.magnitude);
        }

        private void checkLeapEnd()
        {

            Vector3 bossPositionXZ = new Vector3(bossTransform.position.x, 0, bossTransform.position.z);
            Vector3 targetPositionXZ = new Vector3(targetPosition.x, 0, targetPosition.z);

            if (Vector3.Distance(bossPositionXZ, targetPositionXZ) <= impactRadius)
            {
                bossTransform.position = targetPositionXZ; // Snap to target position
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
                    PlayerHealth health = hit.gameObject.GetComponentInChildren<PlayerHealth>();
                    if (health)
                    {
                        applyDamage(health);
                    }
                }
            }
            impactReached = true;
            startCooldown();
        }

        protected override void visualizeAbility(BossAIManager bossAI)
        {
            if (bossAI)
            {
                bossAI.setAttackAnimationTrigger(abilityName);
            }
        }
    }
}
