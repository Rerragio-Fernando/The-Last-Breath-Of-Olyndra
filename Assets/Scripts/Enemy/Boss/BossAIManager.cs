/*
 * ----------------------------------------------------------------------------------------------
 * Project: The Last Breath Of Olyndra                                                          *
 * Script: BossAIManager                                                        *
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
using System.Collections.Generic;
using database;
using animations;

namespace NPC
{
    public class BossAIManager : MonoBehaviour
    {
        // the ID of the boss that needs to be spawned
        [SerializeField]
        private int bossID = 0;
        [SerializeField]
        GameObject bossPrefab;
        GameObject bossObject;
        [SerializeField]
        Transform spawningPoint;

        CutsceneManager cutsceneManager;

        [Header("Current State")]
        [SerializeField] BossAIState currentState;

        Animator aiAnimator;

        public static BossAIManager instance { get; private set; } = null;
        GameData data;
        bool hasBossBeenFound = false;
        bool isBossDefeated=false;
        bool hasPlayerEnteredArena = false;
        bool hasFightStarted = false;

        StatesManager stageManager;
        AiMovements bossMovements;
        Brain.Brain AIBrain;
        PlayerHealth playerHealth;
        EnemyHealth enemyHealth;



        double distanceFromPlayerOnAttack;
        List<double> cooldowns;

        string nextPlannedAttack = "Swipe";

        /// <summary>
        /// reference to a OneShot haptic effect
        /// </summary>
        [Tooltip("Reference to a OneShot haptic effect")]
        [SerializeField] HapticEffectSO impactEffect;


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
            cutsceneManager = CutsceneManager.instance;
            AIBrain = Brain.Brain.instance;
            stageManager = StatesManager.instance;
            playerHealth = PlayerHealth.instance;
            cutsceneManager.onCutsceneCompleted += startBossLogic;
            cooldowns = stageManager.getCooldowns();
            // if the boss is not found in  the database yet add it
            if (!data.getBossesFound.ContainsKey(bossID))
            {
                data.setBossFound(bossID, false);
                data.setBossDefeated(bossID, false);
            }

            spawningPoint = spawningPoint == null ? GameObject.Find("SpawnPoint").transform : spawningPoint; 


            isBossDefeated = data.getBossesDefeated[bossID];
            hasBossBeenFound = data.getBossesFound[bossID];

            if(!isBossDefeated)
            {
                //Spawn the area boss
                spawnBoss();
                enemyHealth = EnemyHealth.instance;
                aiAnimator = bossObject == null ? null : bossObject.gameObject.GetComponentInChildren<Animator>();
                bossMovements = bossObject == null? null: bossObject.GetComponent<AiMovements>();
            }
            if(hasBossBeenFound)
            {
                Debug.Log("Spawn fog wall");
            }

        }

        protected virtual void FixedUpdate()
        {
            if (!hasFightStarted) return;
            processStateMachine();
            updateMovementAnimation();

        }    

        void updateMovementAnimation()
        {
            if(aiAnimator && bossMovements)
            {
                aiAnimator.SetFloat("speed", bossMovements.getCurrentAgentSpeed());
            }
        }

        public void setAttackAnimationBool(string attack, bool isActive)
        {
            aiAnimator.SetBool(attack, isActive);
        }

        public void setAttackAnimationTrigger(string attack)
        {
            aiAnimator.SetTrigger(attack);
        }


        private void spawnBoss()
        {
            if (bossPrefab == null || spawningPoint == null)
            {
                Debug.LogError("Boss prefab or Spawning point is not assigned!");
                return;
            }
            bossObject = Instantiate(bossPrefab, spawningPoint.position, spawningPoint.rotation);

            adjustBossPosition();

            bossObject.transform.rotation = spawningPoint.rotation;
        }

        public void playHapticEffect()
        {
            Debug.Log("in here");
            if (impactEffect)
            {
                Debug.Log("in here again");
                HapticManager.PlayEffect(impactEffect, this.transform.position);
            }
        }

        public BossAIState findState(string name)
        {
            if(stageManager)
            {
                return stageManager.getAttackState(name);
            }
            else
            {
                return null;
            }    
            
        }

        public void findNextAttackUsingANN(double distanceFromPlayer)
        {
            distanceFromPlayerOnAttack = distanceFromPlayer;
            cooldowns = cooldowns == null? stageManager.getCooldowns() : cooldowns;
            if (!playerHealth || !enemyHealth)
            {
                return;
            }
            getSetAttackString = AIBrain.nextActionSelection(distanceFromPlayerOnAttack, playerHealth.getHealth(),enemyHealth.getHealth(), cooldowns);
            switch(getSetAttackString)
            {
                case "Swipe":
                    getSetAgentStoppingDistance = 2;
                    break;
                case "Blighted Pounce":
                    getSetAgentStoppingDistance = 10;
                    break;
                case "Blight Breath":
                    getSetAgentStoppingDistance = 5;
                    break;
            }
        }

        public void trainAnn()
        {
            if (!playerHealth || !enemyHealth)
            {
                return;
            }
            cooldowns = cooldowns == null ? stageManager.getCooldowns() : cooldowns;
            Brain.rewardStates currentReward = Brain.rewardStates.neutral;
            bool playerDamaged = playerHealth.didHealthChange();
            bool enemyDamaged = enemyHealth.didHealthChange();

            if (playerDamaged || enemyDamaged)
            {
                if (playerDamaged && enemyDamaged)
                {
                    currentReward = playerHealth.getDamageTaken() > enemyHealth.getDamageTaken()
                        ? Brain.rewardStates.positive
                        : Brain.rewardStates.negative;
                }
                else
                {
                    currentReward = playerDamaged
                        ? Brain.rewardStates.positive
                        : Brain.rewardStates.negative;
                }
            }
            else
            {
                currentReward = Brain.rewardStates.neutral;
            }
            AIBrain.trainBrain(currentReward, distanceFromPlayerOnAttack, playerHealth.getHealth(), 100.0f, cooldowns);
            
        }

        private void adjustBossPosition()
        {
            // Get the mesh renderer so that we can get the bounds of the mesh
            Renderer bossRenderer = bossObject.GetComponentInChildren<Renderer>();
            if (bossRenderer != null)
            {
                // Collect the mesh bound and store it in a variable
                Bounds bounds = bossRenderer.bounds;

                // Calculate the offset needed to ensure any mesh prefab will spawn with the lowest bound of the mesh on the spawning point
                float yOffset = bounds.min.y - bossObject.transform.position.y;
                bossObject.transform.position = spawningPoint.position - new Vector3(0, yOffset, 0);
            }
            else
            {
                Debug.LogWarning("No Renderer found on the boss prefab. Spawn position may not be accurate.");
                bossObject.transform.position = spawningPoint.position;
            }
        }

        public void startBossFight()
        {
            if(!hasBossBeenFound)
            {
                hasBossBeenFound = true;
                data.setBossFound(bossID, hasBossBeenFound);

                //StartCutscene
                cutsceneManager.startCutscene(bossID);
            }
        }

        public void startBossLogic(bool isCutsceneCompleted)
        {
            hasFightStarted = true;
        }


        private void processStateMachine()
        {
            BossAIState nextState = currentState?.stateTick(this);

            if(nextState)
            {
                currentState = nextState;
            }
        }

        public void updateBossMovements(bool shouldMove)
        {
            if(shouldMove)
            {
                bossMovements.updateMovementPosition();
            }
            else
            {
                bossMovements.stopAgent();
            }
            
        }

        private void OnApplicationQuit()
        {
            if (currentState != this)
            {
                BaseAttackState attackState = currentState as BaseAttackState;

                if (attackState != null)
                {
                    attackState.resetCooldown();
                    attackState.resetValues();
                }
            }
        }

        //Getter and setters 
        public int getSetBossID
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

        public string getSetAttackString
        {
            get { return nextPlannedAttack; }
            set
            {
                if (nextPlannedAttack != value)
                {
                    nextPlannedAttack = value;
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

        public Transform getSetCurrentTarget
        {
            get { return bossMovements.getSetCurrentTarget; }
            set
            {
                if (bossMovements.getSetCurrentTarget != value)
                {
                    bossMovements.getSetCurrentTarget = value;
                }
            }
        }

        public float getSetAgentStoppingDistance
        {
            get { return bossMovements.getSetAgentStoppingDistance; }
            set
            {
                if (bossMovements.getSetAgentStoppingDistance != value)
                {
                    bossMovements.getSetAgentStoppingDistance = value;
                }
            }
        }

        // Getters
        public bool getIsBossDefeated() { return isBossDefeated; }

        public Transform getBossTransform() { return bossObject.transform; }

    }
}


