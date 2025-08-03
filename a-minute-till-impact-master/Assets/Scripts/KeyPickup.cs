using UnityEngine;

public class KeyPickup : MonoBehaviour, IReplayable
{
    [SerializeField] private string keyId = "Key_001";
    public string Id => keyId;

    private void Awake()
    {
        InteractableRegistry.Register(keyId, this);
    }

    public void ReplayAction(string action)
    {
        if (action == "pickup")
        {
            // Simulate pickup (e.g., hide from ghost player)
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning($"[KeyPickup] Unknown replay action: {action}");
        }
    }

    public void OnInteract(GameObject player)
    {
        // Record the interaction via LoopRecorderSystem
        if (LoopRecorderSystem.Instance != null)
        {
            LoopRecorderSystem.Instance.RecordInteraction(Id, "pickup");
        }

        // Simulate pickup (hide the key)
        gameObject.SetActive(false);
    }
}
