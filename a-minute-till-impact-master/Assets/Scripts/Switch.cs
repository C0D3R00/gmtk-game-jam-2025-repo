using UnityEngine;

public class Switch : MonoBehaviour, ISwitchTarget
{
    [SerializeField] private bool isToggle = true;
    [SerializeField] private bool initialState = false;
    [SerializeField] private MonoBehaviour[] targetBehaviours; // Must implement ISwitchTarget

    private ISwitchTarget[] targets;
    private bool isActive;

    private void Awake()
    {
        isActive = initialState;
        targets = new ISwitchTarget[targetBehaviours.Length];
        for (int i = 0; i < targetBehaviours.Length; i++)
            targets[i] = targetBehaviours[i] as ISwitchTarget;

        UpdateTargets();
    }

    public void Trigger()
    {
        if (isToggle)
            isActive = !isActive;
        else
            isActive = true;

        UpdateTargets();
    }

    public void Release() // Optional for pressure plates
    {
        if (!isToggle)
        {
            isActive = false;
            UpdateTargets();
        }
    }

    private void UpdateTargets()
    {
        foreach (var target in targets)
        {
            if (target == null) continue;
            if (isActive) target.Activate();
            else target.Deactivate();
        }
    }
    public void Activate() => Trigger();
    public void Deactivate() => Release();
}
