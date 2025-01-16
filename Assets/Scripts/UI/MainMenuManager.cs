/*
 * ----------------------------------------------------------------------------------------------
 * Project: The Last Breath Of Olyndra                                                          *
 * Script: MainMenuManager                                                        *
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
#if UNITY_STANDALONE_WIN
using UnityEditor;
#endif
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    TMP_Text flashingText;
    [SerializeField]
    GameObject loadingPart;
    [SerializeField]
    GameObject mainMenu;

    [SerializeField]
    Image startButtonBackground;
    [SerializeField]
    Image quitButtonBackground;


    [SerializeField]
    float flashSpeed = 1f;
    [SerializeField]
    int textureSize = 256;
    [SerializeField]
    float goldIntensity = 5f;
    [SerializeField]
    float centerOpacity = 1.0f;
    [SerializeField]
    float edgeOpacity = 0.2f;

    private bool isVisible = true;

    private void Start()
    {
        Color innerColor = new Color32(0, 100, 0, 255);
        Color outerColor = new Color32(184, 134, 11,255);

        Texture2D radialTexture = GenerateRadialGradientTexture(innerColor, outerColor, textureSize, goldIntensity, centerOpacity, edgeOpacity);

        if(startButtonBackground)
        {
            startButtonBackground.sprite = Sprite.Create(radialTexture, new Rect(0, 0, radialTexture.width, radialTexture.height), new Vector2(0.5f, 0.5f));
        }

        if(quitButtonBackground)
        {
            quitButtonBackground.sprite = Sprite.Create(radialTexture, new Rect(0, 0, radialTexture.width, radialTexture.height), new Vector2(0.5f, 0.5f));
        }
        

    }

    Texture2D GenerateRadialGradientTexture(Color innerColor, Color outerColor, int size, float intensity, float centerAlpha, float edgeAlpha)
    {
        Texture2D texture = new Texture2D(size, size);
        Vector2 center = new Vector2(size / 2f, size / 2f);
        float maxDistance = size / 2f;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), center);
                float t = Mathf.Clamp01(distance / maxDistance);

                t = Mathf.Pow(t, intensity); 
                float alpha = Mathf.Lerp(centerAlpha, edgeAlpha, t);

                Color pixelColor = Color.Lerp(innerColor, outerColor, t);
                pixelColor.a = alpha; 
                texture.SetPixel(x, y, pixelColor);
            }
        }

        texture.Apply();
        return texture;
    }

    // Update is called once per frame
    void Update()
    {
        if (flashingText != null)
        {
            float time = Mathf.PingPong(Time.time * flashSpeed, 1f);
            isVisible = time > 0.5f;
            flashingText.enabled = isVisible;
        }
        if (Input.anyKeyDown || IsGamepadButtonPressed())
        {
            OnInputReceived();
        }

    }

    private bool IsGamepadButtonPressed()
    {
        // Check for common gamepad button inputs
        return Input.GetKeyDown(KeyCode.JoystickButton0) || 
               Input.GetKeyDown(KeyCode.JoystickButton1) || 
               Input.GetKeyDown(KeyCode.JoystickButton2) || 
               Input.GetKeyDown(KeyCode.JoystickButton3);   
    }

    private void OnInputReceived()
    {

        if(loadingPart)
        {
            loadingPart.SetActive(false);
        }
        if(mainMenu)
        {
            mainMenu.SetActive(true);
        }

    }

    public void startGame()
    {
        SceneManager.LoadScene(1);
    }

    public void exitGame()
    {
#if UNITY_EDITOR && !UNITY_WEBGL
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
