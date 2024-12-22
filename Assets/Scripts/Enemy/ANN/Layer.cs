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
    public class Layer
    {
        // Number of neurons contained in this layer
        [SerializeField] int numNeurons;
        // The list of neurons that make up this layer
        [SerializeField] List<Neuron> neurons = new List<Neuron>();

        /* <summary>
         * Initializes a new layer with the specified number of neurons, 
         * each configured with the given number of inputs.
         * </summary>
         * <param name="nNeurons">The number of neurons in the layer.</param>
         * <param name="numNeuronInputs">The number of inputs for each neuron in the layer, this will also match the number of neuron in the previous layer .</param>
         */
        public Layer(int nNeurons, int numNeuronInputs)
        {
            numNeurons = nNeurons;
            for (int i = 0; i < numNeurons; i++)
            {
                neurons.Add(new Neuron(numNeuronInputs));
            }
        }

    }
}
