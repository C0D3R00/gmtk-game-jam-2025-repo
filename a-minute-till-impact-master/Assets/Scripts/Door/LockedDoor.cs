using UnityEngine;

public class LockedDoor : InteractableDoor, IReplayable
{
    [Header("Lock Settings")]
    [SerializeField] private string requiredKeyId = "gold_key";
    [SerializeField] private string lockedPrompt = "Door is locked";

    private bool isUnlocked = false;

    public string Id => throw new System.NotImplementedException();

    public override void Interact(GameObject interactor)
    {
        if (isUnlocked || TryUnlock())
            base.Interact(interactor);
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
        isUnlocked = InventoryManager.Instance.HasKey(requiredKeyId);
        return isUnlocked;
    }

    public void ReplayAction(string action)
    {
        if (isUnlocked || TryUnlock())
            base.Interact(null);
        else
            Debug.Log("🔒 Door is locked.");
    }
}
