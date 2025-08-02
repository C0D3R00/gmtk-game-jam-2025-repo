using UnityEngine;

public class KeyPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private string keyId = "gold_key";
    [SerializeField] private string promptText = "Pick up key";

    public void Interact()
    {
        InventoryManager.Instance.AddKey(keyId);
        Debug.Log($"🔑 Picked up key: {keyId}");
        Destroy(gameObject); // remove key from world
    }

    public string GetPrompt()
    {
        return promptText;
    }
}
