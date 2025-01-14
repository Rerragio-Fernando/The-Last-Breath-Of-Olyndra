/*
 * ----------------------------------------------------------------------------------------------
 * Project: The Last Breath Of Olyndra                                                          *
 * Script: HapticManager                                                        *
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
using UnityEngine.InputSystem;

/// <summary>
/// Class created manage each haptic effect that should play during the gameplay
/// Logic was created with help from the youtube video: https://www.youtube.com/watch?v=6YMcoVkAOBA
/// </summary>
public class HapticManager : MonoBehaviour
{
    /// <summary>
    /// static instance of the class used to easily reference the class from other scripts
    /// </summary>
    public static HapticManager instance { get; private set; } = null;
    /// <summary>
    /// A list of all the haptic effects currently active on screen 
    /// </summary>
    List<HapticEffectSO> activeEffects = new List<HapticEffectSO>();

    /// <summary>
    /// Awake is called before Start
    /// </summary>
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning(gameObject.name + " attempted to create a second instance of HapticManager, destroyed");
            Destroy(gameObject);
        }
        instance = this;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        HapticLogic();
    }
    /// <summary>
    /// Function used to manage the vibration depending on the curve of the current effect
    /// </summary>
    private void HapticLogic()
    {
        float lowSpeedMotor = 0.0f;
        float highSpeedMotor = 0.0f;
        if (activeEffects.Count > 0)
        {
            for (int index = 0; index < activeEffects.Count; ++index)
            {
                var effect = activeEffects[index];
                //tick the effect and cleanUp when finished
                float lowSpeedComponent = 0.0f;
                float hightSpeedComponent = 0.0f;
                if (effect.Tick(Camera.main.transform.position, out lowSpeedComponent, out hightSpeedComponent))
                {
                    activeEffects.RemoveAt(index);
                    --index;
                }

                lowSpeedMotor = Mathf.Clamp01(lowSpeedComponent + lowSpeedMotor);
                highSpeedMotor = Mathf.Clamp01(hightSpeedComponent + highSpeedMotor);
            }
        }
        if(Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(lowSpeedMotor, highSpeedMotor);
        }
        

    }

    /// <summary>
    /// ensure the motors speed of the controller is set back to 0 when the class is destroyed to avoid constant rumble
    /// </summary>
    private void OnDestroy()
    {
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0.0f, 0.0f);
        }
            
    }
    /// <summary>
    /// Function callable for external classes, it is used to start an Haptic effect 
    /// </summary>
    /// <param name="effect">the effect that should start playing</param>
    /// <param name="location">the location where the effect should play, only relevant if variesWithDistance boolean in the HapticEffect is true</param>
    /// <returns></returns>
    public static HapticEffectSO PlayEffect(HapticEffectSO effect, Vector3 location)
    {
        return instance.PlayEffect_Internal(effect, location);
    }
    /// <summary>
    /// Function callable for external classes,  used to stop a continous haptic effect
    /// </summary>
    /// <param name="effect">the effect that shopuld be stopped</param>
    public static void StopEffect(HapticEffectSO effect)
    {
        instance.StopEffect_Internal(effect);
    }
    /// <summary>
    /// function used to stop a continous haptic effect
    /// </summary>
    /// <param name="effect">the effect that shopuld be stopped</param>
    void StopEffect_Internal(HapticEffectSO effect)
    {
        activeEffects.Remove(effect);
    }
    /// <summary>
    /// function used to start an Haptic effect 
    /// </summary>
    /// <param name="effect">the effect that should start playing</param>
    /// <param name="location">the location where the effect should play, only relevant if variesWithDistance boolean in the HapticEffect is true</param>
    /// <returns></returns>
    HapticEffectSO PlayEffect_Internal(HapticEffectSO effect, Vector3 location)
    {
        if(!effect)
        { 
            return null; 
        }
        var activeEffect = ScriptableObject.Instantiate(effect);
        activeEffect.Initialize(location);

        activeEffects.Add(activeEffect);

        return activeEffect;
    }
}
