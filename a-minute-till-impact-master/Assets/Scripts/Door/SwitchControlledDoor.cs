using UnityEngine;

public class SwitchControlledDoor : DoorBase, ISwitchTarget
{
    public void Activate() => Open();
    public void Deactivate() => Close();
}
