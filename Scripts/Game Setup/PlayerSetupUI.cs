using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetupUI : MonoBehaviour
{
    // UI
    public InputField playerNameField;
    public Toggle standardDeckToggle;
    public Toggle customDeckToggle;
    public Toggle customDeckCheck;
    public Button customDeckButton;

    // Show custom deck button
    public void ShowCustomDeckButton() {
        customDeckButton.gameObject.SetActive(true);
        customDeckCheck.gameObject.SetActive(true);
    }

    // Hide custom deck button
    public void HideCustomDeckButton() {
        customDeckButton.gameObject.SetActive(false);
        customDeckCheck.gameObject.SetActive(false);
    }

    // Set custom deck button function
    public void LoadDeckBuilder() {
        SceneLoader.LoadDeckBuilder();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
