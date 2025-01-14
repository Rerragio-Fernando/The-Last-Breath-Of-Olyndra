/*
 * ----------------------------------------------------------------------------------------------
 * Project: The Last Breath Of Olyndra                                                          *
 * Script: Brain                                                        *
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
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NPC.Brain
{

    public class AILearningReplay
    {
        public List<double> states;
        public double reward;

        public AILearningReplay(double distanceFromPlayer, double playerHealth, double AIHealth, List<double> cooldowns, double r)
        {
            states = new List<double>();
            states.Add(distanceFromPlayer);
            states.Add(playerHealth);
            states.Add(AIHealth);

            foreach(double cooldown in cooldowns)
            {
                states.Add(cooldown);
            }

        }
    }

    public enum rewardStates { positive, negative, neutral }

    public class Brain : MonoBehaviour
    {
        public static Brain instance { get; private set; } = null;
        ANN ann;

        // reward associated with actions
        float reward = 0.0f;
        // memory - list of past actions and rewards
        List<AILearningReplay> replayMemory = new List<AILearningReplay>();
        // memory capacity
        int memoryCapacity = 1000;

        // how much future states affects rewards
        float discount = 0.99f;
        // chance of picking random action
        float exploreRate = 100.0f;
        // max chance value
        float maxExplorerRate = 100.0f;
        // min chance value
        float minExploreRate = 0.01f;
        //chance decay amount for each update
        float exploreDecay = 0.0001f;

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogWarning(gameObject.name + " attempted to create a second instance of Brain, destroyed");
                Destroy(gameObject);
                return;
            }
            instance = this;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

            ann = new ANN(6, 3, 1, 6, 0.2);
        }

        public string nextActionSelection(double distanceFromPlayer, double playerHealth, double AIHealth, List<double> cooldowns)
        {
            List<double> states = new List<double>();
            string currentAttack = "Blighted Pounce";
            states = new List<double>();
            states.Add(distanceFromPlayer);
            states.Add(playerHealth);
            states.Add(AIHealth);

            foreach (int cooldown in cooldowns)
            {
                states.Add(cooldown);
            }

            List<double> qs = new List<double>();
            qs = softMax(ann.calcOutput(states));
            double maxQ = qs.Max();
            int maxQIndex = qs.ToList().IndexOf(maxQ);

            exploreRate = Mathf.Clamp(exploreRate - exploreDecay, minExploreRate, maxExplorerRate);

            if (Random.Range(0, 100) < exploreRate)
                maxQIndex = Random.Range(0, cooldowns.Count);

            // Perform the attack corresponding to maxQIndex
            switch (maxQIndex)
            {
                case 0:
                    currentAttack = "Swipe";
                    break;
                case 1:
                    currentAttack = "Blighted Pounce";
                    break;
                case 2:
                    currentAttack = "Blight Breath";
                    break;
            }

            return currentAttack;
        }

        public void trainBrain(rewardStates currentReward, double distanceFromPlayer, double playerHealth, double AIHealth, List<double> cooldowns)
        {
            switch(currentReward)
            {
                case rewardStates.positive:
                    reward = 1.0f;
                    break;
                case rewardStates.negative:
                    reward = -1.0f;
                    break;
                case rewardStates.neutral:
                    reward = 0.5f;
                    break;
            }
            // After selecting the action
            AILearningReplay currentReplay = new AILearningReplay(distanceFromPlayer, playerHealth, AIHealth, cooldowns, reward);

            // Store in replay memory
            if (replayMemory.Count > memoryCapacity)
            {
                replayMemory.RemoveAt(0);
            }
            replayMemory.Add(currentReplay);

            if (currentReward == rewardStates.negative)
            {
                // Backpropagate and train
                for (int i = replayMemory.Count - 1; i >= 0; i--)
                {
                    List<double> oldOutputs = softMax(ann.calcOutput(replayMemory[i].states));
                    double maxQOld = oldOutputs.Max();
                    int action = oldOutputs.ToList().IndexOf(maxQOld);

                    double feedback;
                    if (i == replayMemory.Count - 1)
                    {
                        feedback = replayMemory[i].reward;
                    }
                    else
                    {
                        List<double> newOutputs = softMax(ann.calcOutput(replayMemory[i + 1].states));
                        double maxQ = newOutputs.Max();
                        feedback = replayMemory[i].reward + discount * maxQ;
                    }

                    oldOutputs[action] = feedback;
                    ann.Train(replayMemory[i].states, oldOutputs);
                }
            }
        }


        List<double> softMax(List<double> values)
        {
            double max = values.Max();

            float scale = 0.0f;
            for (int i = 0; i < values.Count; ++i)
            {
                scale += Mathf.Exp((float)(values[i] - max));
            }


            List<double> result = new List<double>();
            for (int i = 0; i < values.Count; ++i)
            {
                result.Add(Mathf.Exp((float)(values[i] - max)) / scale);
            }

            return result;
        }
    }
}
