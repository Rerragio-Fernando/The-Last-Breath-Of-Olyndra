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
using UnityEngine.AI;

namespace NPC
{
    public class AiMovements : MonoBehaviour
    {
        [SerializeField]
        float detectionRadius = 10f;
        Transform currentTarget;
        private NavMeshAgent agent;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        public void updateMovementPosition()
        {
            if (currentTarget == null) return;
            agent.SetDestination(currentTarget.position);

        }

        public void stopAgent()
        {
            agent.ResetPath();
        }

        //Getter and setters 
        public Transform getSetCurrentTarget
        {
            get { return currentTarget; }
            set
            {
                if (currentTarget != value)
                {
                    currentTarget = value;
                }
            }
        }


        public float getSetAgentStoppingDistance
        {
            get { return agent.stoppingDistance; }
            set
            {
                if (agent.stoppingDistance != value)
                {
                    agent.stoppingDistance = value;
                }
            }
        }

    }
}
