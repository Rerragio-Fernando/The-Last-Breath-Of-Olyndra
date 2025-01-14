/*
 * ----------------------------------------------------------------------------------------------
 * Project: The Last Breath Of Olyndra                                                          *
 * Script: BaseAttackState                                                         *
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
    public abstract class BaseAttackState : BossAIState
    {
        [SerializeField]
        protected string abilityName;
        [SerializeField]
        protected float cooldown;
        [SerializeField]
        protected bool comboAttack;
        [SerializeField]
        protected int numbOfCombo;
        private float lastUsedTime = -Mathf.Infinity;
        [SerializeField]
        protected float damage;

        protected bool isActive =false;

        protected int abilityUsage = 0;

        private bool cooldownFinished = false;


        public override BossAIState stateTick(BossAIManager bossAI)
        {
            BossAIState stateToReturn = this;
            checkIfCooldownNeedReset(stateToReturn);
            return stateToReturn;
        }

        protected bool checkIfCooldownNeedReset(BossAIState stateToReturn)
        {
            bool isDifferentState = false;
            if (stateToReturn != this)
            {
                BaseAttackState attackState = stateToReturn as BaseAttackState;

                if (attackState != null && attackState.abilityName != this.abilityName)
                {
                    isDifferentState = true;
                    attackState.resetCooldown();
                }
            }
            return isDifferentState;
        }

        public abstract void Activate();
        protected abstract void visualizeAbility(BossAIManager bossAI);

        public abstract void resetValues();

        public abstract void applyDamage(PlayerHealth playerHealth);

        protected void startCooldown()
        {
            Debug.Log($"Cooldown started for {abilityName}, duration: {cooldown}");
            lastUsedTime = Time.time;
            abilityUsage++;
            cooldownFinished = false;
        }

        protected bool isOnCooldown()
        {
            return Time.time < (lastUsedTime + cooldown);
        }

        protected bool checkCooldownStateChange()
        {
            if (cooldown == 0)
            {
                cooldownFinished = true;
                return true;
            }

            if (!isOnCooldown() && !cooldownFinished)
            {
                cooldownFinished = true;
                return true;
            }
            return false;
        }

        public void resetCooldown()
        {
            isActive = false;
            abilityUsage = 0;
            lastUsedTime = -Mathf.Infinity;
        }

        //getters
        public string getName()
        {
            return abilityName;
        }

        public double getCooldown()
        {
            return cooldown;
        }


    }
}
