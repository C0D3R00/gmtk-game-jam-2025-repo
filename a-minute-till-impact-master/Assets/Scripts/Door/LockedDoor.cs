using UnityEngine;

public class LockedDoor : InteractableDoor
{
    [Header("Lock Settings")]
    [SerializeField] private string requiredKeyId = "gold_key";
    [SerializeField] private string lockedPrompt = "Door is locked";

    private bool isUnlocked = false;

    public override void Interact()
    {
        if (isUnlocked || TryUnlock())
            base.Interact();
        else
            Debug.Log("🔒 Door is locked.");
    }

    public override string GetPrompt()
    {
        return isUnlocked || TryUnlock()
            ? base.GetPrompt()
            : lockedPrompt;
    }

    private bool TryUnlock()
    {
        //isUnlocked = InventoryManager.Instance.HasKey(requiredKeyId);
        return isUnlocked;
    }
}
