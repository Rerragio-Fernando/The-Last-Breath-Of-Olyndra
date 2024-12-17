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

        }

        // Update is called once per frame
        void Update()
        {

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
