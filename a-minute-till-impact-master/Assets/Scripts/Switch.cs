using UnityEngine;

public class Switch : MonoBehaviour, IInteractable, IReplayable
{
    [SerializeField] private MonoBehaviour[] targetBehaviours;
    private ISwitchTarget[] targets;

    [SerializeField] private string promptText = "Toggle Switch";

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
            LoopRecorderSystem.Instance.RecordInteraction(Id, "toggle");
        }
    }

    public string GetPrompt() => promptText;

    public void ReplayAction(string action)
    {
        if (action == "toggle")
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
        foreach (var target in targets)
        {
            if (target == null) continue;

            // You could also check state here if needed
            target.Activate(); // or target.Toggle() if supported
        }
    }
}
