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
        private float lastUsedTime = -Mathf.Infinity;
        [SerializeField]
        protected float damage;

        public override BossAIState stateTick(BossAIManager bossAI)
        {
            BossAIState stateToReturn = this;
            return stateToReturn;
        }

        public abstract void Activate();
        protected abstract void visualizeAbility();

        protected void startCooldown()
        {
            lastUsedTime = Time.time;
        }

        protected bool isOnCooldown()
        {
            return Time.time < (lastUsedTime + cooldown);
        }


    }
}
