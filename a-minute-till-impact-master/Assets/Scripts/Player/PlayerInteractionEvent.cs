[System.Serializable]
public class PlayerInteractionEvent
{
    public float time;
    public string targetId; // unique ID for the interactable object
    public string action;   // e.g., "Interact", "Pickup", "UnlockDoor"
}
