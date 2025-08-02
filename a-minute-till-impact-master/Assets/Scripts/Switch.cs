using UnityEngine;

public class Switch : MonoBehaviour, IInteractable
{
    [SerializeField] private MonoBehaviour[] targetBehaviours;
    private ISwitchTarget[] targets;
    private bool isOn = false;

    [SerializeField] private string promptText = "Toggle Switch";

    private void Awake()
    {
        targets = new ISwitchTarget[targetBehaviours.Length];
        for (int i = 0; i < targetBehaviours.Length; i++)
            targets[i] = targetBehaviours[i] as ISwitchTarget;
    }

    public void Interact()
    {
        isOn = !isOn;

        foreach (var target in targets)
        {
            if (target == null) continue;

            if (isOn) target.Activate();
            else target.Deactivate();
        }
    }

    public string GetPrompt() => promptText;
}
