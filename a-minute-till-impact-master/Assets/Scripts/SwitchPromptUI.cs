using TMPro;
using UnityEngine;

public class SwitchPromptUI : MonoBehaviour
{
    public static SwitchPromptUI Instance { get; private set; }

    [Header("UI Elements")]
    [SerializeField] private GameObject promptRoot;  // e.g., the full panel or canvas group
    [SerializeField] private TMP_Text promptText;        // UI text element to show the message

    [Header("Prompt Settings")]
    [SerializeField] private string defaultPrompt = "Press [E] to Interact";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (promptRoot != null)
            promptRoot.SetActive(false);
    }

    public void ShowPrompt(bool show, string customText = null)
    {
        if (promptRoot == null || promptText == null) return;

        promptRoot.SetActive(show);
        if (show)
        {
            promptText.text = string.IsNullOrEmpty(customText) ? defaultPrompt : customText;
        }
    }
}
