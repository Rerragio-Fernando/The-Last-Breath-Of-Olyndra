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
            if(!bossAI.getSetCurrentTarget)
            {
                // determine whenever the boss should move towards player or something else
                // determine whenever the determined target is in the field of view
                if (isTargetInFOV(bossAI.getBossTransform(), GameObject.FindGameObjectWithTag("Player").transform))
                {
                    bossAI.getSetCurrentTarget = GameObject.FindGameObjectWithTag("Player").transform;
                    Debug.Log("Found player");
                }
                else
                {
                    Debug.Log("not found player");
                }
                
            }
            return this;
        }

        private bool isTargetInFOV(Transform aiTransform, Transform target)
        {
            Vector3 directionToTarget = (target.position - aiTransform.position).normalized;
            float distanceToTarget = Vector3.Distance(aiTransform.position, target.position);

            float angle = Vector3.Angle(aiTransform.forward, directionToTarget);
            if (angle > fieldOfViewAngle / 2) return false;

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
