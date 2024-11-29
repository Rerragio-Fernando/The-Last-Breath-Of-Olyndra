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
using database;

namespace NPC
{
    public class BossAIManager : MonoBehaviour
    {
        // the ID of the boss that needs to be spawned
        [SerializeField]
        private int bossID = 0;

        public static BossAIManager instance { get; private set; } = null;
        GameData data;
        bool hasBossBeenFound = false;
        bool isBossDefeated=false;
        bool hasPlayerEnteredArena = false;

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogWarning(gameObject.name + " attempted to create a second instance of BossAIManager, destroyed");
                Destroy(gameObject);
                return;
            }
            instance = this;
        }

        private void Start()
        {
            data = GameSavingManager.instance.geData;
            // if the boss is not found in  the database yet add it
            if (!data.getBossesFound.ContainsKey(bossID))
            {
                data.setBossFound(bossID, false);
                data.setBossDefeated(bossID, false);
            }

            isBossDefeated = data.getBossesDefeated[bossID];
            hasBossBeenFound = data.getBossesFound[bossID];

            if(!isBossDefeated)
            {
                //Spawn the area boss
            }
            if(hasBossBeenFound)
            {
                Debug.Log("Spawn fog wall");
            }

        }

        public void startBossFight()
        {
            if(!hasBossBeenFound)
            {
                hasBossBeenFound = true;
                data.setBossFound(bossID, hasBossBeenFound);

                //StartCutscene
                Debug.Log("Starting Cutscene");
            }
        }





        //Getter and setters 
        public int getBossID
        {
            get { return bossID; }
            set
            {
                if (bossID != value)
                {
                    bossID = value;
                }
            }
        }
        public bool playerEnteredArena
        {
            get { return hasPlayerEnteredArena; }
            set
            {
                if (hasPlayerEnteredArena != value)
                {
                    hasPlayerEnteredArena = value;
                }
            }
        }

        // Getters
        public bool getIsBossDefeated() { return isBossDefeated; }

    }
}


