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
    public class Brain : MonoBehaviour
    {
        ANN ann;
        double sumSquareError = 0;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            ann = new ANN(2, 1, 1, 2, 0.8);
            List<double> results;
            for (int i = 0; i < 5000; i++)
            {
                sumSquareError = 0;
                results = Train(1, 1, 0);
                sumSquareError += Mathf.Pow((float)results[0] - 0, 2);
                results = Train(1, 0, 1);
                sumSquareError += Mathf.Pow((float)results[0] - 1, 2);
                results = Train(0, 1, 1);
                sumSquareError += Mathf.Pow((float)results[0] - 1, 2);
                results = Train(0, 0, 0);
                sumSquareError += Mathf.Pow((float)results[0] - 0, 2);

            }

            Debug.Log("SSE: " + sumSquareError);

            results = Train(1, 1, 0,false);
            Debug.Log("1, 1: " + results[0]);
            results = Train(1, 0, 1, false);
            Debug.Log("1, 0: " + results[0]);
            results = Train(0, 1, 1, false);
            Debug.Log("0, 1: " + results[0]);
            results = Train(0, 0, 0, false);
            Debug.Log("0, 0: " + results[0]);

        }

        List<double> Train(double i1, double i2, double o, bool shouldTrain = true)
        {
            List<double> inputs = new  List<double>();
            List<double> outputs = new List<double>();
            inputs.Add(i1);
            inputs.Add(i2);
            outputs.Add(o);
            return (ann.Train(inputs, outputs));
        }
    
    }
}
