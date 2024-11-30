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

namespace animations
{
    public class CutsceneManager : MonoBehaviour
    {
        /// <summary>
        /// Singleton 
        /// </summary>
        public static CutsceneManager instance { get; private set; } = null;

        /// <summary>
        /// Delegate used to inform other classes that the cutscene has ended
        /// </summary>
        /// <param name="isCompleted"> true if the cutscene ended </param>
        public delegate void OnCutsceneCompletedDelegate(bool isCompleted);
        /// <summary>
        /// event of the OnCutsceneCompletedDelegate delegate 
        /// </summary>
        public event OnCutsceneCompletedDelegate onCutsceneCompleted;
        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogWarning(gameObject.name + " attempted to create a second instance of CutsceneManager, destroyed");
                Destroy(gameObject);
                return;
            }
            instance = this;
        }


        public void startCutscene(int cutsceneIndex)
        {
            // play the cuscene and add a callback on video ended (might change logic depending on implementation

            onCutsceneCompleted(true);
        }

    }


}

