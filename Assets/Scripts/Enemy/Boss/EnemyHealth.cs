/*
 * ----------------------------------------------------------------------------------------------
 * Project: The Last Breath Of Olyndra                                                          *
 * Script: EnemyHealth                                                        *
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

public class EnemyHealth : CommonHealth
{
    public static EnemyHealth instance { get; private set; } = null;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning(gameObject.name + " attempted to create a second instance of EnemyHealth, destroyed");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }


    public void applyDamage(float damage)
    {
        if (currentHealth - damage <= 0)
        {
            currentHealth = 0;
            enemyDefeat();
        }
        else
        {
            currentHealth -= damage;
            Debug.LogWarning(" current health after taking damage: " + currentHealth);
        }
    }

    void enemyDefeat()
    {
        Debug.Log(" handle death");
    }


}
