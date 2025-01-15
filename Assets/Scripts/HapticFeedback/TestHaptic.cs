/*
 * ----------------------------------------------------------------------------------------------
 * Project: The Last Breath Of Olyndra                                                          *
 * Script: TestHaptic                                                        *
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

/// <summary>
/// Class created to test the OneShot haptic effect
/// Logic was created with help from the youtube video: https://www.youtube.com/watch?v=6YMcoVkAOBA
/// </summary>
public class TestHaptic : MonoBehaviour
{
    /// <summary>
    /// reference to a OneShot haptic effect
    /// </summary>
    [Tooltip("Reference to a OneShot haptic effect")]
    [SerializeField] HapticEffectSO impactEffect;
    /// <summary>
    /// check used to confirm if the effect should be played
    /// </summary>
    bool shouldPlayEffect = true;


    /// <summary>
    /// function called when the player collide with this object
    /// </summary>
    /// <param name="collision"> the other object colliding</param>
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Here");
        if(shouldPlayEffect && collision.gameObject.tag=="Player")
        {
            HapticManager.PlayEffect(impactEffect, this.transform.position);
        }
    }

    /// <summary>
    /// function called when the player using Character Controller collide with this object
    /// </summary>
    public void playEffectUsingCharacterController()
    {
        if (shouldPlayEffect)
        {
            HapticManager.PlayEffect(impactEffect, this.transform.position);
        }
    }

    /// <summary>
    /// setter for the shouldPlayEffect variable
    /// </summary>
    /// <param name="newVal">false if it should no longer play the effect</param>
    public void setShouldPlayEffect(bool newVal)
    {
        shouldPlayEffect = newVal;
    }
}
