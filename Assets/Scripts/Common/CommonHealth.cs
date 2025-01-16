/*
 * ----------------------------------------------------------------------------------------------
 * Project: The Last Breath Of Olyndra                                                          *
 * Script: CommonHealth                                                       *
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

public class CommonHealth : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 1000;
    protected float currentHealth;
    protected float previousHealth;
    protected float damageTaken = 0;
    protected Slider healthBar;
    protected Image fillImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        currentHealth = maxHealth;
        previousHealth = currentHealth;
    }
    private void Update()
    {
        if (healthBar)
        {
            healthBar.value = currentHealth;
        }

    }


    public bool didHealthChange()
    {
        bool wasUnitDamaged = previousHealth != currentHealth;
        setDamageTaken();
        previousHealth = currentHealth;
        return wasUnitDamaged;
    }

    protected void findFillAreaColor()
    {
        Transform fillArea = healthBar.transform.Find("Fill Area");
        if (fillArea)
        {
            fillImage = fillArea.Find("Fill").GetComponent<Image>();
        }
    }

    void setDamageTaken()
    {
        damageTaken = previousHealth - currentHealth;

        if (damageTaken < 0)
        {
            damageTaken = 0;
        }
    }

    public virtual void applyDamage(float damage)
    {
        if (currentHealth - damage <= 0)
        {
            currentHealth = 0;
        }
        else
        {
            currentHealth -= damage;
        }
    }

    public float getDamageTaken()
    {
        return damageTaken;
    }

    public double getHealth()
    {
        return currentHealth;
    }

    public Slider getSetHealthBar
    {
        get { return healthBar; }
        set
        {
            if (healthBar != value)
            {
                healthBar = value;
            }
        }
    }

    protected IEnumerator waitAndLoadScene(int sceneToLoad, float waitTime=3f)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }
}
