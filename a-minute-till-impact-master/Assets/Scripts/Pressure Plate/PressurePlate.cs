using System.Linq;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [Header("Target Objects")]
    [SerializeField] private MonoBehaviour[] targetBehaviours; // must implement ISwitchTarget

    private ISwitchTarget[] targets;
    private int triggerCount = 0;

    private void Awake()
    {
        targets = new ISwitchTarget[targetBehaviours.Length];
        for (int i = 0; i < targetBehaviours.Length; i++)
            targets[i] = targetBehaviours[i] as ISwitchTarget;

        Debug.Log(targets.Count());
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (!IsValidTrigger(other)) return;

        triggerCount++;
        if (triggerCount == 1) // First object entered
        {
            foreach (var target in targets)
                target?.Activate();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsValidTrigger(other)) return;

        triggerCount = Mathf.Max(0, triggerCount - 1);
        if (triggerCount == 0) // All objects exited
        {
            foreach (var target in targets)
                target?.Deactivate();
        }
    }

    private bool IsValidTrigger(Collider col)
    {
        // Optional: Only react to specific tags/layers
        return col.CompareTag("Player") || col.CompareTag("Movable");
    }
}
