using UnityEngine;

public class Switch : MonoBehaviour, IInteractable, IReplayable
{
    [SerializeField] private MonoBehaviour[] targetBehaviours;
    private ISwitchTarget[] targets;
    private bool isOn = false;

    [SerializeField] private string promptText = "Toggle Switch";

    public string Id => gameObject.name;

    private void Awake()
    {
        targets = new ISwitchTarget[targetBehaviours.Length];
        for (int i = 0; i < targetBehaviours.Length; i++)
            targets[i] = targetBehaviours[i] as ISwitchTarget;

        InteractableRegistry.Register(Id, this);
    }

    public void Interact(GameObject interactor)
    {
        Toggle();

        var recorder = interactor.GetComponent<PlayerRecorder>();
        if (recorder != null && recorder.isRecording)
        {
            recorder.RecordInteraction(Id, isOn ? "on" : "off");
        }
    }

    public string GetPrompt() => promptText;

    public void ReplayAction(string action)
    {
        if (action == "toggle")
        {
            Toggle();
        }
    }

    private void Toggle()
    {
        isOn = !isOn;

        foreach (var target in targets)
        {
            if (target == null) continue;

            if (isOn) target.Activate();
            else target.Deactivate();
        }
    }
}
