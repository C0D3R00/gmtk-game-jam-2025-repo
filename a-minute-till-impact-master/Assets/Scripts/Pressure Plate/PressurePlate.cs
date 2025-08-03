using UnityEngine;

public class PressurePlate : MonoBehaviour, IReplayable, IResettable
{
    [Header("Target Objects")]
    [SerializeField] private MonoBehaviour[] targetBehaviours; // Must implement ISwitchTarget
    [SerializeField] private string plateId = "Plate_001";

    private ISwitchTarget[] targets;
    private int triggerCount = 0;

    public string Id => plateId;

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

        InteractableRegistry.Register(plateId, this);
        ResettableRegistry.Register(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsValidTrigger(other)) return;

        triggerCount++;
        if (triggerCount == 1)
        {
            foreach (var target in targets)
                target?.Activate();

            // Record press event using LoopRecorderSystem
            LoopRecorderSystem.Instance.RecordInteraction(plateId, "press");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsValidTrigger(other)) return;

        triggerCount = Mathf.Max(0, triggerCount - 1);
        if (triggerCount == 0)
        {
            foreach (var target in targets)
                target?.Deactivate();

            // Record release event
            LoopRecorderSystem.Instance.RecordInteraction(plateId, "release");
        }
    }

    private bool IsValidTrigger(Collider col)
    {
        return col.CompareTag("Player") || col.CompareTag("Movable");
    }

    public void ReplayAction(string action)
    {
        switch (action)
        {
            case "press":
                triggerCount = 1;
                foreach (var target in targets)
                    target?.Activate();
                break;

            case "release":
                triggerCount = 0;
                foreach (var target in targets)
                    target?.Deactivate();
                break;

            default:
                Debug.LogWarning($"[PressurePlate] Unknown replay action: {action}");
                break;
        }
    }

    public void ResetToInitialState()
    {
        triggerCount = 0;
        foreach (var target in targets)
            target?.Deactivate();
    }
}
