using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public SimpleBackendConnector backendConnector;
    public Dropdown inputDropdown;
    public Dropdown targetDropdown;

    void Start()
    {
        // Apply the initial dropdown values so the defaults aren't ignored
        OnLanguageSelectionChanged();
    }

    public void OnLanguageSelectionChanged()
    {
        string inputLanguage = inputDropdown.options[inputDropdown.value].text;
        string targetLanguage = targetDropdown.options[targetDropdown.value].text;
        // Assuming the dropdown text is the language code, e.g., "hi"
        //backendConnector.inputLanguage = inputLanguage;
        //backendConnector.targetLanguage = targetLanguage;

        switch (inputLanguage)
        {
            case "English":
                backendConnector.inputLanguage = "en";
                break;
            case "Tamil":
                backendConnector.inputLanguage = "ta";
                break;
            case "Kannada":
                backendConnector.inputLanguage = "kn";
                break;
            case "Hindi":
                backendConnector.inputLanguage = "hi";
                break;
            case "Punjabi":
                backendConnector.inputLanguage = "pa";
                break;
        }
        
        switch (targetLanguage)
        {
            case "English":
                backendConnector.targetLanguage = "en";
                break;
            case "Tamil":
                backendConnector.targetLanguage = "ta";
                break;
            case "Kannada":
                backendConnector.targetLanguage = "kn";
                break;
            case "Hindi":
                backendConnector.targetLanguage = "hi";
                break;
            case "Punjabi":
                backendConnector.targetLanguage = "pa";
                break;
        }
    }
}
