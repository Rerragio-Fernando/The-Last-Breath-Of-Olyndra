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
    public class BossAreaTrigger : MonoBehaviour
    {
        BossAIManager aiManager;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            aiManager = BossAIManager.instance;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (aiManager == null) return;
                
            if(other.gameObject.tag=="Player" && !aiManager.getIsBossDefeated())
            {
                aiManager.playerEnteredArena = true;
                aiManager.startBossFight();
            }
        }
    }
}
