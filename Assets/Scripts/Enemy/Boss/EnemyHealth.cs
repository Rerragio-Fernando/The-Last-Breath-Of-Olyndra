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
using UnityEngine.UI;


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

    private new void Start()
    {
        base.Start();

        healthBar = GameObject.Find("FenmorHealth").GetComponent<Slider>();
        if (healthBar)
        {
            healthBar.maxValue = maxHealth;
            findFillAreaColor();
            if (fillImage)
            {
                fillImage.color = Color.red;
            }
        }
    }


    public override void applyDamage(float damage)
    {
        base.applyDamage(damage);

        if (currentHealth == 0)
        {
            enemyDefeat();
        }
        else
        {
            Debug.Log("Character took damage!");
        }
    }

    void enemyDefeat()
    {
        StartCoroutine(waitAndLoadScene(2));
    }

}
