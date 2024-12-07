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
    [CreateAssetMenu(menuName = "AI/States/Movements")]
    public class MovementState : BossAIState
    {

        public override BossAIState stateTick(BossAIManager bossAI)
        {
            BossAIState stateToReturn = this;
            float distanceToTarget = Vector3.Distance(bossAI.getBossTransform().position, bossAI.getSetCurrentTarget.position);

            if (distanceToTarget > bossAI.getSetAgentStoppingDistance)
            {
                bossAI.updateBossMovements(true);
            }
            else
            {

                // TODO add logic to add an attack using ANN
                stateToReturn = nextState == null ? this : nextState;
                bossAI.updateBossMovements(false);
            }
            return stateToReturn;
        }
    }
}
