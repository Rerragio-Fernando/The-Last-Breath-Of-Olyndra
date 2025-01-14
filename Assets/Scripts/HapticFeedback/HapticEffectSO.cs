/*
 * ----------------------------------------------------------------------------------------------*
 * This project was created by Marco Minganna as part of the Unit "Game Programming"             *
 * At Kingston University                                                                        *
 *                                                                                               *
 * Security Classification: Public                                                               *
 *                                                                                               *
 * December 2023                                                                                  *
 * ----------------------------------------------------------------------------------------------*
 */
using UnityEngine;

/// <summary>
/// Scriptable Object class used to create different Haptic Effects
/// </summary>
[CreateAssetMenu(menuName ="Haptic Effect", fileName ="HapticEffect")]
public class HapticEffectSO : ScriptableObject
{
    /// <summary>
    /// use to determine if the Haptic effect should run once or continuosly, if the latter the haptic effect should be manually stopped
    /// </summary>
    public enum EType
    {
        OneShot,
        Continuous
    }
    /// <summary>
    /// use to determine if the Haptic effect should run once or continuosly, initialized to oneshot
    /// </summary>
    [SerializeField] EType type = EType.OneShot;
    /// <summary>
    /// float that determines how long the effect should run for
    /// </summary>
    [SerializeField] float duration = 0.0f;
    /// <summary>
    /// the intensity of the haptic feeling for one of the motoe of the controller
    /// </summary>
    [SerializeField] float lowSpeedIntensity = 1.0f;
    /// <summary>
    /// curve used to determine how the rumble should feel
    /// </summary>
    [SerializeField] AnimationCurve lowSpeedMotor;
    /// <summary>
    /// the intensity of the haptic feeling for one of the motoe of the controller
    /// </summary>
    [SerializeField] float highSpeedIntensity = 1.0f;
    /// <summary>
    /// curve used to determine how the rumble should feel
    /// </summary>
    [SerializeField] AnimationCurve highSpeedMotor;
    /// <summary>
    /// true if the rumble should change based on distance, closer = higher
    /// </summary>
    [SerializeField] bool variesWithDistance = false;
    /// <summary>
    /// the max distance which the rumble should be felt
    /// </summary>
    [SerializeField] float maxDistance =  25f;
    /// <summary>
    /// curve used to change the current rumble based on distance
    /// </summary>
    [SerializeField] AnimationCurve fallOffCurve;
    /// <summary>
    /// the current position in the world of the effect
    /// </summary>
    [System.NonSerialized] Vector3 effectLocation;
    /// <summary>
    /// used to keep track on how much of the effect has been played
    /// </summary>
    [System.NonSerialized] float progress;
    /// <summary>
    /// called on creation of the effect
    /// </summary>
    /// <param name="newEffectLocation">the position of the effect</param>
    public void Initialize(Vector3 newEffectLocation)
    {
        effectLocation = newEffectLocation;
        progress = 0.0f;
    }
    /// <summary>
    /// the equivalent of Update for this ScriptableObject
    /// </summary>
    /// <param name="receiverPos">the current position of player</param>
    /// <param name="lowSpeed">float used to detemine the current vibration of one of the controller motors</param>
    /// <param name="hightSpeed">float used to detemine the current vibration of one of the controller motors</param>
    /// <returns></returns>
    public bool Tick(Vector3 receiverPos, out float lowSpeed, out float hightSpeed)
    {
        progress += Time.deltaTime / duration;

        //calculate the distance factor
        float distanceFactor = 1.0f;

        if(variesWithDistance)
        {
            float distance = (receiverPos - effectLocation).magnitude;
            distanceFactor = distance >= maxDistance ? 0.0f : fallOffCurve.Evaluate(distance / maxDistance);
        }
        lowSpeed = lowSpeedIntensity * distanceFactor * lowSpeedMotor.Evaluate(progress);
        hightSpeed = highSpeedIntensity * distanceFactor * highSpeedMotor.Evaluate(progress);

        // check if we are finished
        if(progress >=1.0f)
        {
            if(type == EType.OneShot)
            {
                return true;
            }
            progress = 0.0f;
        }
        return false;
    }
}
