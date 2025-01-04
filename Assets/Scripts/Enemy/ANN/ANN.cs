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
    public class ANN
    {
        // The number of input neurons in the neural network
        [SerializeField] int numImputs;
        // The number of output neurons in the neural network
        [SerializeField] int numOutputs;
        // The number of hidden layers in the neural network
        [SerializeField] int numHidden;
        // The number of neurons in each hidden layer
        [SerializeField] int numNPerHidden;
        // The learning rate used for weight adjustments during training
        [SerializeField] double alpha;
        // The collection of layers that make up the neural network
        [SerializeField] List<Layer> layers = new List<Layer>();

        /// <summary>
        /// Initializes a new artificial neural network (ANN) with the specified structure and parameters.
        /// </summary>
        /// <param name="nI">The number of input neurons.</param>
        /// <param name="nO">The number of output neurons.</param>
        /// <param name="nH">The number of hidden layers.</param>
        /// <param name="nPh">The number of neurons in each hidden layer.</param>
        /// <param name="a">The learning rate (alpha).</param>
        public ANN(int nI, int nO, int nH, int nPh, double a)
        {
            numImputs = nI;
            numOutputs = nO;
            numHidden = nH;
            numNPerHidden = nPh;
            alpha = a;

            if (numHidden > 0)
            {
                layers.Add(new Layer(numNPerHidden, numImputs));
                for(int i=0; i < numHidden-1; i++)
                {
                    layers.Add(new Layer(numNPerHidden, numNPerHidden));
                }
                layers.Add(new Layer(numOutputs, numNPerHidden));
            }
            else
            {
                layers.Add(new Layer(numOutputs, numImputs));
            }
        }

        /// <summary>
        /// This function is used to train the ANN for a specific input/output situation
        /// </summary>
        /// <param name="inputValues"></param>
        /// <param name="desiredOutput"></param>
        /// <returns></returns>
        public List<double> Train(List<double> inputValues, List<double> desiredOutput)
        {
            List<double> outputValues = new List<double>();
            outputValues = calcOutput(inputValues, desiredOutput);
            updateWeights(outputValues, desiredOutput);
            return outputValues;
        }

        /// <summary>
        /// Executes the forward pass of the neural network with the given input values
        /// and updates the weights based on the desired output.
        /// </summary>
        /// <param name="inputValues">The list of input values for the neural network.</param>
        /// <param name="desiredOutput">The expected output values to guide weight adjustments.</param>
        /// <returns>A list of the computed outputs from the network.</returns>
        public List<double> calcOutput(List<double> inputValues, List<double> desiredOutput)
        {
            List<double> inputs = new List<double>();
            List<double> outputValues = new List<double>();
            int currentInput = 0;

            if (inputValues.Count != numImputs)
            {
                Debug.LogError("Number of Inputs must be " + numImputs);
                return outputValues;
            }

            inputs = new List<double>(inputValues);
            for (int i = 0; i < numHidden + 1; i++)
            {
                if (i > 0)
                {
                    inputs = new List<double>(outputValues);
                }
                outputValues.Clear();

                for (int j = 0; j < layers[i].getNumNeurons(); j++)
                {
                    double N = 0;
                    layers[i].GetNeurons()[j].getInputs().Clear();

                    for (int k = 0; k < layers[i].GetNeurons()[j].getNumInputs(); k++)
                    {
                        layers[i].GetNeurons()[j].getInputs().Add(inputs[currentInput]);
                        N += layers[i].GetNeurons()[j].getWeights()[k] * inputs[currentInput];
                        currentInput++;
                    }

                    N -= layers[i].GetNeurons()[j].getSetBias;

                    if (i == numHidden)
                    {
                        layers[i].GetNeurons()[j].getSetOutput = activationFunction(N);
                    } 
                    else
                    {
                        layers[i].GetNeurons()[j].getSetOutput = activationFunction(N, 2);
                    }

                    outputValues.Add(layers[i].GetNeurons()[j].getSetOutput);
                    currentInput = 0;
                }
            }
            return outputValues;
        }

        /// <summary>
        /// this function is used to calculate the output
        /// </summary>
        /// <param name="inputValues"></param>
        /// <returns></returns>
        public List<double> calcOutput(List<double> inputValues)
        {
            List<double> inputs = new List<double>();
            List<double> outputValues = new List<double>();
            int currentInput = 0;

            if (inputValues.Count != numImputs)
            {
                Debug.LogError("Number of Inputs must be " + numImputs);
                return outputValues;
            }

            inputs = new List<double>(inputValues);
            for (int i = 0; i < numHidden + 1; i++)
            {
                if (i > 0)
                {
                    inputs = new List<double>(outputValues);
                }
                outputValues.Clear();

                for (int j = 0; j < layers[i].getNumNeurons(); j++)
                {
                    double N = 0;
                    layers[i].GetNeurons()[j].getInputs().Clear();

                    for (int k = 0; k < layers[i].GetNeurons()[j].getNumInputs(); k++)
                    {
                        layers[i].GetNeurons()[j].getInputs().Add(inputs[currentInput]);
                        N += layers[i].GetNeurons()[j].getWeights()[k] * inputs[currentInput];
                        currentInput++;
                    }

                    N -= layers[i].GetNeurons()[j].getSetBias;

                    if (i == numHidden)
                    {
                        layers[i].GetNeurons()[j].getSetOutput = activationFunction(N);
                    } 
                    else
                    {
                        layers[i].GetNeurons()[j].getSetOutput = activationFunction(N, 2);
                    }

                    outputValues.Add(layers[i].GetNeurons()[j].getSetOutput);
                    currentInput = 0;
                }
            }
            return outputValues;
        }

        /// <summary>
        /// This method retrieves the current weights and biases from all the layers in the neural network.
        /// It concatenates them into a single string, which can be used to save the network state.
        /// </summary>
        /// <returns>A comma-separated string of the network's weights and biases.</returns>
        public string PrintWeights()
        {
            string weightStr = "";
            foreach (Layer l in layers)
            {
                foreach (Neuron n in l.GetNeurons())
                {
                    foreach (double w in n.getWeights())
                    {
                        weightStr += w + ",";
                    }
                    weightStr += n.getSetBias + ",";
                }
            }
            return weightStr;
        }

        /// <summary>
        /// This method loads the weights and biases from a comma-separated string and applies them to the network.
        /// It is typically used for restoring a previously saved network state.
        /// </summary>
        /// <param name="weightStr">A comma-separated string containing the weights and biases to be loaded into the network.</param>
        public void LoadWeights(string weightStr)
        {
            if (weightStr == "") return;
            string[] weightValues = weightStr.Split(',');
            int w = 0;
            foreach (Layer l in layers)
            {
                foreach (Neuron n in l.GetNeurons())
                {
                    for (int i = 0; i < n.getWeights().Count; i++)
                    {
                        n.getWeights()[i] = System.Convert.ToDouble(weightValues[w]);
                        w++;
                    }
                    n.getSetBias = System.Convert.ToDouble(weightValues[w]);
                    w++;
                }
            }
        }

        /// <summary>
        /// Updates the weights and biases of the network during the backpropagation phase.
        /// </summary>
        /// <param name="outputs">The computed outputs from the network.</param>
        /// <param name="desiredOutput">The expected outputs to guide weight adjustments.</param>
        void updateWeights(List<double> outputs, List<double> desiredOutput)
        {
            double error;
            for (int i = numHidden; i >= 0; i--)
            {
                for (int j = 0; j < layers[i].getNumNeurons(); j++)
                {
                    if (i == numHidden)
                    {
                        error = desiredOutput[j] - outputs[j];
                        // error gradient is calculated using the Delta Rule
                        layers[i].GetNeurons()[j].getSetErrorGradient = outputs[j] * (1 - outputs[j]) * error;
                    }
                    else
                    {
                        layers[i].GetNeurons()[j].getSetErrorGradient = layers[i].GetNeurons()[j].getSetOutput * (1 - layers[i].GetNeurons()[j].getSetOutput);
                        double errorGradSum = 0;
                        for (int p = 0; p < layers[i + 1].getNumNeurons(); p++)
                        {
                            errorGradSum += layers[i + 1].GetNeurons()[p].getSetErrorGradient * layers[i + 1].GetNeurons()[p].getWeights()[j];
                        }
                        layers[i].GetNeurons()[j].getSetErrorGradient *= errorGradSum;
                    }
                    for (int k = 0; k < layers[i].GetNeurons()[j].getNumInputs(); k++)
                    {
                        if (i == numHidden)
                        {
                            error = desiredOutput[j] - outputs[j];
                            layers[i].GetNeurons()[j].getWeights()[k] += alpha * layers[i].GetNeurons()[j].getInputs()[k] * error;
                        }
                        else
                        {
                            layers[i].GetNeurons()[j].getWeights()[k] += alpha * layers[i].GetNeurons()[j].getInputs()[k] * layers[i].GetNeurons()[j].getSetErrorGradient;
                        }
                    }
                    layers[i].GetNeurons()[j].getSetBias += alpha * -1 * layers[i].GetNeurons()[j].getSetErrorGradient;
                }

            }

        }

        /// <summary>
        /// Applies the activation function to a given value.
        /// Currently uses the sigmoid function.
        /// </summary>
        /// <param name="value">The input value for the activation function.</param>
        /// <returns>The result of the activation function.</returns>
        double activationFunction(double value, int activationType = 1)
        {
            switch(activationType)
            {
                case 0:
                    return step(value);
                case 1:
                    return sigmoid(value);
                case 2:
                    return tanH(value);
                case 3:
                    return reLu(value);
                case 4:
                    return leakyReLu(value);
                case 5:
                    return sinusoid(value);
                case 6:
                    return arcTan(value);
                case 7:
                    return softSign(value);
                default:
                    return sigmoid(value);
            }
        }

        /// <summary>
        /// Implements a step activation function, returning 0 for inputs below 0 and -1 otherwise.
        /// </summary>
        /// <param name="value">The input value for the step function.</param>
        /// <returns>0 if the value is less than 0, otherwise -1.</returns>
        double step(double value)
        {
            if (value < 0) return 0;
            else return -1;
        }

        double tanH(double value)
        {
            double k = (double)System.Math.Exp(-2 * value);
            return 2 / (1.0f + k) - 1;
        }

        /// <summary>
        /// Computes the sigmoid function, which maps inputs to a range between 0 and 1.
        /// </summary>
        /// <param name="value">The input value for the sigmoid function.</param>
        /// <returns>The result of the sigmoid function.</returns>
        double sigmoid(double value)
        {
            double k = (double)System.Math.Exp(value);
            return k / (1.0f + k);
        }

        double reLu(double value)
        {
            if (value > 0) return value;
            else return 0;
        }
        double leakyReLu(double value)
        {
            if (value < 0) return 0.01 * value;
            else return value;
        }

        double sinusoid(double value)
        {
            return Mathf.Sin((float)value);
        }

        double arcTan(double value)
        {
            return Mathf.Atan((float)value);
        }

        double softSign(double value)
        {
            return value / (1 + Mathf.Abs((float)value));
        }
    }
}
