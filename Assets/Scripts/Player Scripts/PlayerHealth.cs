/*
 * ----------------------------------------------------------------------------------------------
 * Project: The Last Breath Of Olyndra                                                          *
 * Script: PlayerHealth                                                        *
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
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : CommonHealth
{

    public static PlayerHealth instance { get; private set; } = null;

    bool poisoned = false;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning(gameObject.name + " attempted to create a second instance of PlayerHealth, destroyed");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private new void Start()
    {
        base.Start();
        healthBar = GameObject.Find("Health Slider").GetComponent<Slider>();
        if (healthBar)
        {
            healthBar.maxValue = maxHealth;
            findFillAreaColor();
            if (fillImage)
            {
                fillImage.color = Color.green;
            }
        }
    }




    public override void applyDamage(float damage)
    {
        base.applyDamage(damage);

        if (currentHealth == 0)
        {
            playerDefeat();
        }
        else
        {
            Debug.Log("Character took damage!");
        }
    }

    void playerDefeat()
    {
        PlayerEventSystem.TriggerDeathEvent();
        StartCoroutine(WaitAndReloadScene());
    }

    public void setIsPoisoned(bool isPoisoned)
    {
        Debug.Log("I am poisoned");
        poisoned = isPoisoned;
        if (!fillImage) return;

        if (poisoned)
        {
            fillImage.color = new Color(0.5f, 0f, 0.5f);
        }
        else
        {
            fillImage.color = Color.green;
        }
    }

    private IEnumerator WaitAndReloadScene()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(waitAndLoadScene(1));
    }



}
