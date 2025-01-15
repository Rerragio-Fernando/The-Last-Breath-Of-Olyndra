/*
 * ----------------------------------------------------------------------------------------------
 * Project: The Last Breath Of Olyndra                                                          *
 * Script: PlayerStatusEffects                                                        *
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
using System.Collections;
using UnityEngine;

public class PlayerStatusEffects : MonoBehaviour
{
    private Coroutine poisonCoroutine;
    private bool isPoisoned = false;
    PlayerHealth playerHealth;

    /// <summary>
    /// timer used by the poison coroutine
    /// </summary>
    WaitForSeconds poisonTickTimer;

    private void Start()
    {
        playerHealth = PlayerHealth.instance;
    }

    /// <summary>
    /// Applies poison to the player, dealing tick damage and automatically healing after a duration.
    /// </summary>
    /// <param name="damagePerTick">The amount of damage per tick.</param>
    /// <param name="poisonDuration">How long the poison effect lasts.</param>
    /// <param name="tickInterval">The time between each tick of damage.</param>
    public void ApplyPoison(float damagePerTick, float poisonDuration = 5.0f, float tickInterval = 0.5f)
    {
        if (isPoisoned)
        {
            // Player is already poisoned, return early
            return;
        }

        isPoisoned = true;
        playerHealth = playerHealth == null ? PlayerHealth.instance : playerHealth;
        playerHealth.setIsPoisoned(isPoisoned);
        poisonTickTimer = new WaitForSeconds(tickInterval);


        // Start the poison effect
        poisonCoroutine = StartCoroutine(PoisonCoroutine(damagePerTick, poisonDuration, tickInterval));
    }

    /// <summary>
    /// Coroutine to handle poison tick damage and healing after the effect ends.
    /// </summary>
    private IEnumerator PoisonCoroutine(float damagePerTick, float poisonDuration, float tickInterval)
    {
        float elapsedTime = 0f;

        // Deal tick damage over time
        while (elapsedTime < poisonDuration)
        {
            // Apply tick damage
            ApplyDamage(damagePerTick);

            // Wait for the interval before applying the next tick
            yield return poisonTickTimer;

            elapsedTime += tickInterval;
        }

        // After poison duration ends, remove the poison effect
        HealFromPoison();
    }

    /// <summary>
    /// Applies damage to the player.
    /// </summary>
    /// <param name="damage">The amount of damage to apply.</param>
    private void ApplyDamage(float damage)
    {
        playerHealth = playerHealth == null ? PlayerHealth.instance : playerHealth;
        playerHealth.applyDamage(damage);
    }

    /// <summary>
    /// Heals the player from poison.
    /// </summary>
    private void HealFromPoison()
    {
        isPoisoned = false;
        playerHealth = playerHealth == null ? PlayerHealth.instance : playerHealth;
        playerHealth.setIsPoisoned(isPoisoned);

    }
}
