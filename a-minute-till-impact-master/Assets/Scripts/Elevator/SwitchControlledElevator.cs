using UnityEngine;

public class SwitchControlledElevator : ElevatorBase, ISwitchTarget
{
    public void Activate() => GoUp();
    public void Deactivate() => GoDown();
}
