using UnityEngine;
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
    float flashSpeed = 1f;

    private bool isVisible = true;



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
}
