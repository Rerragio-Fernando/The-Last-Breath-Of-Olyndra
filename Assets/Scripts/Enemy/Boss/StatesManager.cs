/*
 * ----------------------------------------------------------------------------------------------
 * Project: The Last Breath Of Olyndra                                                          *
 * Script: StatesManager                                                        *
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
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public enum StateType { idle, movement, attack };
    [System.Serializable]
    public class BossAIStateReference
    {
        public BossAIState bossAIState;
        public StateType stateType;
    }

    public class StatesManager : MonoBehaviour
    {
        public static StatesManager instance { get; private set; } = null;
        [SerializeField] private List<BossAIStateReference> AiStates = new List<BossAIStateReference>();

        List<double> cooldowns;


        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogWarning(gameObject.name + " attempted to create a second instance of StatesManager, destroyed");
                Destroy(gameObject);
                return;
            }
            instance = this;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            cooldowns= new List<double>();
            addAllCooldowns();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void addAllCooldowns()
        {
            foreach (var stateReference in AiStates)
            {
                if (stateReference.stateType == StateType.attack)
                {
                    BaseAttackState attackState = stateReference.bossAIState as BaseAttackState;
                    cooldowns.Add(attackState.getCooldown());
                }
            }
        }

        public BaseAttackState getAttackState(string name)
        {
            BaseAttackState nextAttack =null;
            foreach (var stateReference in AiStates)
            {
                if (stateReference.stateType == StateType.attack)
                {
                    BaseAttackState attackState = stateReference.bossAIState as BaseAttackState;
                    if (attackState != null && attackState.getName() == name)
                    {
                        return attackState;
                    }
                }
            }
            return nextAttack;
        }

        public List<double> getCooldowns()
        {
            return cooldowns;
        }

        private void OnApplicationQuit()
        {
            foreach (var stateReference in AiStates)
            {
                if (stateReference.stateType == StateType.attack)
                {
                    BaseAttackState attackState = stateReference.bossAIState as BaseAttackState;
                    if(attackState)
                    {
                        attackState.resetValues();
                    }
                }
            }
        }
    }
}
