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
using System.Collections.Generic;
using UnityEngine;


namespace NPC.Brain
{
    public class Neuron
    {
        // number of imputs coming in to the neuron
        [SerializeField] int numInputs;
        // A constant value added to the weighted sum of inputs to adjust the output
        [SerializeField] double bias;
        // The calculated output value of the neuron after applying the activation function
        [SerializeField] double output;
        // The error gradient used during the backpropagation process to update weights
        [SerializeField] double errorGradient;
        // The list of weights corresponding to each input connection
        [SerializeField] List<double> weights = new List<double>();
        // The list of inputs received by this neuron
        [SerializeField] List<double> inputs = new List<double>();

        public Neuron(int nInputs)
        {
            bias = Random.Range(-1.0f, 1.0f);
            numInputs = nInputs;
            for(int i=0; i< numInputs; i++)
            {
                weights.Add(Random.Range(-1.0f, 1.0f));
            }
        }

        //Getter and setters 
        public double getSetOutput
        {
            get { return output; }
            set
            {
                if (output != value)
                {
                    output = value;
                }
            }
        }

        public double getSetErrorGradient
        {
            get { return errorGradient; }
            set
            {
                if (errorGradient != value)
                {
                    errorGradient = value;
                }
            }
        }

        public double getSetBias
        {
            get { return bias; }
            set
            {
                if (bias != value)
                {
                    bias = value;
                }
            }
        }



        //Getters
        public List<double> getInputs()
        {
            return inputs;
        }

        public int getNumInputs()
        {
            return numInputs;
        }

        public List<double> getWeights()
        {
            return weights;
        }




    }
}
