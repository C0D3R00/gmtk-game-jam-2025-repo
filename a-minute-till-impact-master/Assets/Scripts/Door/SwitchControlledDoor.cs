using UnityEngine;

public class SwitchControlledDoor : DoorBase, ISwitchTarget, IReplayable, IResettable
{
    [SerializeField] private string id = "Door_001";
    public string Id => id;

    private void Awake()
    {
        InteractableRegistry.Register(Id, this);
        ResettableRegistry.Register(this);
    }

    private void OnDestroy()
    {
        ResettableRegistry.Unregister(this);
    }

    public void Activate() => Open();
    public void Deactivate() => Close();

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
                Debug.LogWarning($"[SwitchControlledDoor] Unknown replay action: {action}");
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
