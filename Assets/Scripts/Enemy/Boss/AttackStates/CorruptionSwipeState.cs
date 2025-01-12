/*
 * ----------------------------------------------------------------------------------------------
 * Project: The Last Breath Of Olyndra                                                          *
 * Script: CorruptionSwipeState                                                        *
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
using UnityEngine.VFX;

namespace NPC
{
    [CreateAssetMenu(menuName = "AI/States/Attacks/Swipes")]
    public class CorruptionSwipeState : BaseAttackState
    {
        Transform bossTransform;
        [SerializeField]
        float attackRadius = 5f;
        [SerializeField]
        float attackAngle = 120f;
        [SerializeField]
        LayerMask targetLayer;
        /// <summary>
        /// Reference to the particle system prefab used for the slash
        /// </summary>
        [SerializeField]
        GameObject slashParticles;

        public override BossAIState stateTick(BossAIManager bossAI)
        {

            BossAIState stateToReturn = this;
            bossTransform = bossAI.getBossTransform();
            if (!isOnCooldown() && !isActive)
            {
                Activate();
                visualizeAbility();
            }
            if (checkCooldownStateChange())
            {
                Debug.Log("cooldown done Swipe");
                isActive = false;
                stateToReturn = nextState == null ? stateToReturn : nextState;
            }
            bossAI.getSetCurrentTarget = null;
            checkIfCooldownNeedReset(stateToReturn);
            bossAI.trainAnn();
            return stateToReturn;
        }

        public override void Activate()
        {
            Debug.Log($"{abilityName} activated!");
           
            Collider[] hits = Physics.OverlapSphere(bossTransform.position, attackRadius, targetLayer);
            foreach (var hit in hits)
            {
                Vector3 directionToTarget = (hit.transform.position - bossTransform.position).normalized;
                float angle = Vector3.Angle(bossTransform.forward, directionToTarget);

                if (angle <= attackAngle / 2)
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

        public override void applyDamage(PlayerHealth playerHealth)
        {
            if (playerHealth)
            {
                playerHealth.applyDamage(damage);
            }
        }

        public override void resetValues()
        {
            resetCooldown();
        }


        private void spawnSwipeEffect()
        {
            if (slashParticles)
            {
                GameObject tmpSlash = GameObject.Instantiate(slashParticles);
                tmpSlash.transform.position = bossTransform.position;

                float effectScale = attackRadius / 5f; 
                tmpSlash.transform.localScale = new Vector3(effectScale, effectScale, effectScale);

                VisualEffect visualEffect = tmpSlash.GetComponentInChildren<VisualEffect>();
                if (visualEffect != null)
                {
                    float effectDuration = 0.3f;
                    Destroy(tmpSlash, effectDuration + 1);
                }
                else
                {
                    Destroy(tmpSlash, 5f);
                }
            }
            else
            {
                Debug.LogError("SlashParticles prefab is null!");
            }
        }

        protected override void visualizeAbility()
        {
            //TODO set animation
            spawnSwipeEffect();
        }
    }
}
