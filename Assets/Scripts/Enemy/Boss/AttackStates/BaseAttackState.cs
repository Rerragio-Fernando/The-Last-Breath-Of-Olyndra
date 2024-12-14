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
            lastUsedTime = Time.time;
        }

        protected bool isOnCooldown()
        {
            return Time.time < (lastUsedTime + cooldown);
        }

        public void resetCooldown()
        {
            lastUsedTime = -Mathf.Infinity;
        }


    }
}
