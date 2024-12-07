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

namespace database
{
    [System.Serializable]
    public class GameData
    {
        [Header("Game Scene")]
        [SerializeField] int currentMap = 0;

        [Header("Player Details")]
        [SerializeField] string playerName = "";

        [Header("Boss Data")]
        [SerializeField] SerializableDictionary<int,bool> bossesFound = new SerializableDictionary<int, bool>();
        [SerializeField] SerializableDictionary<int, bool> bossesDefeated = new SerializableDictionary<int, bool>();

        //Getters
        public SerializableDictionary<int, bool> getBossesFound
        {
            get { return bossesFound; }
        }
        public SerializableDictionary<int, bool> getBossesDefeated
        {
            get { return bossesDefeated; }
        }

        //Setters
        public void setBossFound(int id,bool value)
        {
            if (bossesFound.ContainsKey(id))
            {
                bossesFound[id] = value;
            }
            else
            {
                bossesFound.Add(id, value);
            }
        }
        public void setBossDefeated(int id, bool value)
        {
            if (bossesDefeated.ContainsKey(id))
            {
                bossesDefeated[id] = value;
            }
            else
            {
                bossesDefeated.Add(id, value);
            }
        }
    }
}

