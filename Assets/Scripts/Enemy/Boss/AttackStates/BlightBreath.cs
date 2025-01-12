/*
 * ----------------------------------------------------------------------------------------------
 * Project: The Last Breath Of Olyndra                                                          *
 * Script: BlightBreath                                                        *
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
    [CreateAssetMenu(menuName = "AI/States/Attacks/BlightBreath")]
    public class BlightBreath : BaseAttackState
    {
        Transform bossTransform;

        [SerializeField]
        float exhaleRange = 10f;
        [SerializeField]
        float exhaleAngle = 90f;
        [SerializeField]
        float hazardDuration = 5f; 
        [SerializeField]
        LayerMask targetLayer;

        /// <summary>
        /// Reference to the toxic gas particle system prefab
        /// </summary>
        [SerializeField]
        GameObject gasParticlesPrefab;

        public override BossAIState stateTick(BossAIManager bossAI)
        {
            BossAIState stateToReturn = this;
            bossTransform = bossAI.getBossTransform();

            if (!isOnCooldown() && !isActive)
            {
                Activate();
                bossAI.getSetCurrentTarget = null;
                visualizeAbility(bossAI);
            }

            if (checkCooldownStateChange())
            {
                Debug.Log("Cooldown done: Toxic Exhale");
                isActive = false;
                stateToReturn = nextState == null ? stateToReturn : nextState;
            }

            checkIfCooldownNeedReset(stateToReturn);
            bossAI.trainAnn();
            return stateToReturn;
        }

        public override void Activate()
        {
            Debug.Log($"{abilityName} activated!");

            Collider[] hits = Physics.OverlapSphere(bossTransform.position, exhaleRange, targetLayer);
            foreach (var hit in hits)
            {
                Vector3 directionToTarget = (hit.transform.position - bossTransform.position).normalized;
                float angle = Vector3.Angle(bossTransform.forward, directionToTarget);

                if (angle <= exhaleAngle / 2)
                {
                    PlayerHealth health = hit.gameObject.GetComponentInChildren<PlayerHealth>();
                    if (health)
                    {
                        applyDamage(health);
                    }
                    
                }
            }

            isActive = true;
            startCooldown();
        }

        private void applyHazardEffect(GameObject target)
        {
            // TODO: Add logic to apply damage-over-time or debuff to the player
            Debug.Log($"Applying toxic effect to {target.name}");
        }

        public override void applyDamage(PlayerHealth playerHealth)
        {
            if (playerHealth)
            {
                //apply base damage
                playerHealth.applyDamage(damage);
                // add tick damage
                applyHazardEffect(playerHealth.gameObject);
            }
            
        }

        protected override void visualizeAbility(BossAIManager bossAI)
        {
            if (bossAI)
            {
                bossAI.setAttackAnimationTrigger(abilityName);
                Debug.Log(abilityName);
            }
            spawnGasCloudEffect();
        }

        private void spawnGasCloudEffect()
        {
            if (gasParticlesPrefab)
            {
                GameObject gasEffect = Instantiate(gasParticlesPrefab);
                gasEffect.transform.position = bossTransform.position + bossTransform.forward * (exhaleRange / 2);
                gasEffect.transform.rotation = bossTransform.rotation;
                gasEffect.transform.localScale = new Vector3(exhaleRange, exhaleRange / 2, exhaleRange);

                Destroy(gasEffect, hazardDuration);
            }
            else
            {
                Debug.LogError("GasParticles prefab is null!");
            }
        }

        public override void resetValues()
        {
            resetCooldown();
        }
    }
}
