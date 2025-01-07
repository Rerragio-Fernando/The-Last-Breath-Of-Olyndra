/*
 * ----------------------------------------------------------------------------------------------
 * Project: The Last Breath Of Olyndra                                                          *
 * Script: [Script Name or Description]                                                         *
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

        protected void checkIfCooldownNeedReset(BossAIState stateToReturn)
        {
            if (stateToReturn != this)
            {
                BaseAttackState attackState = stateToReturn as BaseAttackState;

                if (attackState != null && attackState.abilityName != this.abilityName)
                {
                    attackState.resetCooldown();
                }
            }
        }

        public abstract void Activate();
        protected abstract void visualizeAbility();

        public abstract void resetValues();

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
