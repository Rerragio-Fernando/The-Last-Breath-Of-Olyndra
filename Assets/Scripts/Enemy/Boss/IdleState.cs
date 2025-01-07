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
    [CreateAssetMenu(menuName ="AI/States/Idles")]
    public class IdleState : BossAIState
    {
        public float fieldOfViewAngle = 90f;
        public float detectionRange = 20f;
        public LayerMask obstructionMask;
        public override BossAIState stateTick(BossAIManager bossAI)
        {
            BossAIState stateToReturn = this;
            if (!bossAI.getSetCurrentTarget)
            {
                // determine whenever the boss should move towards player or something else
                // determine whenever the determined target is in the field of view
                decideNextAttack(bossAI, ref stateToReturn);

            }
            return stateToReturn;
        }

        private void decideNextAttack(BossAIManager bossAI, ref BossAIState  stateToReturn)
        {
            Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;

            if (playerTransform == null)
            {
                Debug.LogWarning("Player not found!");
                return; 
            }
            if (isTargetInFOV(bossAI.getBossTransform(), playerTransform))
            {
                bossAI.getSetCurrentTarget = playerTransform;
                Vector3 bossPositionXZ = new Vector3(bossAI.getBossTransform().position.x, 0, bossAI.getBossTransform().position.z);
                Vector3 targetPositionXZ = new Vector3(bossAI.getSetCurrentTarget.position.x, 0, bossAI.getSetCurrentTarget.position.z);
                float distanceToPlayer = Vector3.Distance(bossPositionXZ, targetPositionXZ);
                bossAI.findNextAttackUsingANN(distanceToPlayer, 100); //TODO get player health ( either here or in the Manager)
                /*
                if (distanceToPlayer > 10f)
                {
                    bossAI.getSetAgentStoppingDistance = 10;
                    if (Random.Range(0, 2) == 0)
                    {
                        bossAI.getSetAttackString = "Blighted Pounce";
                    }
                    else
                    {
                        bossAI.getSetAttackString = "Blight Breath";
                    }
                }
                else
                {
                    bossAI.getSetAgentStoppingDistance = 5;
                    bossAI.getSetAttackString = "Swipe";
                }
                */
                
                stateToReturn = nextState == null ? this : nextState;
            }
            else
            {
                Debug.Log("not found player");
            }
        }

        //TODO improve this function
        private bool isTargetInFOV(Transform aiTransform, Transform target)
        {
            Vector3 directionToTarget = (target.position - aiTransform.position).normalized;
            float distanceToTarget = Vector3.Distance(aiTransform.position, target.position);

            float angle = Vector3.Angle(aiTransform.forward, directionToTarget);
            //if (angle > fieldOfViewAngle / 2) return false;

            if (distanceToTarget > detectionRange) return false;

            if (Physics.Raycast(aiTransform.position, directionToTarget, out RaycastHit hit, detectionRange, obstructionMask))
            {
                if (hit.transform != target)
                {
                    return false;
                }
            }

            return true;
        }
    }


}
