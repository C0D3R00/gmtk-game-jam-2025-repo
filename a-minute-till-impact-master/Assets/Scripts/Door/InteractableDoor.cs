using UnityEngine;

public class InteractableDoor : DoorBase, IInteractable, IReplayable, IResettable
{
    [Header("Prompt Text")]
    [SerializeField] private string openPrompt = "Open Door";
    [SerializeField] private string closePrompt = "Close Door";

    public string Id => gameObject.name;

    private void Awake()
    {
        InteractableRegistry.Register(Id, this);
        ResettableRegistry.Register(this);
    }

    private void OnDestroy()
    {
        ResettableRegistry.Unregister(this);
    }

    public virtual void Interact(GameObject interactor)
    {
        Toggle();

        if (LoopRecorderSystem.Instance != null)
        {
            LoopRecorderSystem.Instance.RecordInteraction(Id, isOpen ? "open" : "close");
        }
    }

    public virtual string GetPrompt()
    {
        return isOpen ? closePrompt : openPrompt;
    }

    public void ReplayAction(string action)
    {
        switch (action)
        {
            case "open":
                Open();
                break;
            case "close":
                Close();
                break;
            case "toggle":
                Toggle();
                break;
            default:
                Debug.LogWarning($"[InteractableDoor] Unknown replay action: {action}");
                break;
        }
    }

    public void ResetToInitialState()
    {
        Close();
        if (movementType == DoorMovementType.Slide && closedPoint != null)
        {
            transform.position = closedPoint.position;
        }
        else if (movementType == DoorMovementType.Rotate && closedPoint != null)
        {
            transform.rotation = closedPoint.rotation;
        }
    }
}
