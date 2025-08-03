using UnityEngine;

public class Switch : MonoBehaviour, IInteractable, IReplayable
{
    [SerializeField] private MonoBehaviour[] targetBehaviours;
    private ISwitchTarget[] targets;

    [SerializeField] private string promptText = "Toggle Switch";
    private bool isOn = true;

    public string Id => gameObject.name;

    private void Awake()
    {
        targets = new ISwitchTarget[targetBehaviours.Length];
        for (int i = 0; i < targetBehaviours.Length; i++)
        {
            if (targetBehaviours[i] is ISwitchTarget target)
                targets[i] = target;
            else
                Debug.LogWarning($"{targetBehaviours[i].name} does not implement ISwitchTarget.");
        }

        InteractableRegistry.Register(Id, this);
    }

    public void Interact(GameObject interactor)
    {
        Debug.Log("Interact switch");
        Toggle();

        // ✅ Use LoopRecorderSystem instead of PlayerRecorder
        if (LoopRecorderSystem.Instance != null)
        {
            string action = isOn ? "on" : "off";
            LoopRecorderSystem.Instance.RecordInteraction(Id, action);
        }
    }

    public string GetPrompt() => promptText;

    public void ReplayAction(string action)
    {
        if (action == "on")
        {
            SetState(true);
        }
        else if (action == "off")
        {
            SetState(false);
        }
        else if (action == "toggle") // optional fallback
        {
            Toggle();
        }
        else
        {
            Debug.LogWarning($"[Switch] Unknown action: {action}");
        }
    }

    private void Toggle()
    {
        SetState(!isOn);
    }

    private void SetState(bool on)
    {
        isOn = on;

        foreach (var target in targets)
        {
            if (target == null) continue;

            if (isOn) target.Activate();
            else target.Deactivate();
        }
    }
}
