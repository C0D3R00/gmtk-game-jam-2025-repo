using UnityEngine;

public class InteractableDoor : DoorBase, IInteractable
{
    [Header("Prompt Text")]
    [SerializeField] private string openPrompt = "Open Door";
    [SerializeField] private string closePrompt = "Close Door";

    public virtual void Interact(GameObject interactor)
    {
        Toggle();

        var recorder = interactor.GetComponent<PlayerRecorder>();
        if (recorder != null && recorder.IsRecording)
        {
            recorder.RecordInteraction(this.name, isOpen ? "open" : "close");
        }
    }

    public virtual string GetPrompt()
    {
        return isOpen ? closePrompt : openPrompt;
    }
}
