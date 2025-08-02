using UnityEngine;

[System.Serializable]
public class PlayerActionFrame
{
    public float time;
    public Vector2 moveInput;
    public Vector2 lookInput;
    public bool jump;
    public bool interact;
    public bool crouch;
    public bool sprint;
}
