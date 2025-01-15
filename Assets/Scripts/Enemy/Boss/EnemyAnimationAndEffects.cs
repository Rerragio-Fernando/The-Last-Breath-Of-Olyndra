/*
 * ----------------------------------------------------------------------------------------------
 * Project: The Last Breath Of Olyndra                                                          *
 * Script: EnemyAnimationAndEffects                                                        *
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
using UnityEngine.VFX;

public class EnemyAnimationAndEffects : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void spawnSwipeEffect(GameObject slashParticles, float attackRadius = 5f)
    {
        if (slashParticles)
        {
            GameObject tmpSlash = GameObject.Instantiate(slashParticles);
            tmpSlash.transform.position = transform.position;

            float effectScale = attackRadius / 5f;
            tmpSlash.transform.localScale = new Vector3(effectScale, effectScale, effectScale);

            VisualEffect visualEffect = tmpSlash.GetComponentInChildren<VisualEffect>();
            if (visualEffect != null)
            {
                float effectDuration = 0.3f;
                Destroy(tmpSlash, effectDuration + 1);
            }
            else
            {
                Destroy(tmpSlash, 5f);
            }
        }
        else
        {
            Debug.LogError("SlashParticles prefab is null!");
        }
    }

    public void spawnSwipingAttackVfx(GameObject slashParticles)
    {
        spawnSwipeEffect(slashParticles);
    }
}
