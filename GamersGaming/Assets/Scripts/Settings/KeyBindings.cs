using UnityEngine;

[CreateAssetMenu(fileName = "KeyBindings", menuName = "Config/KeyBindings")]
public class KeyBindings : ScriptableObject
{
    public KeyCode Forward = KeyCode.W;
    public KeyCode Backward = KeyCode.S;
    public KeyCode Left = KeyCode.A;
    public KeyCode Right = KeyCode.D;
    public KeyCode Jump = KeyCode.Space;
    public KeyCode Sprint = KeyCode.LeftShift;
    public KeyCode Crouch = KeyCode.LeftControl;
    public KeyCode Interact = KeyCode.F;
}
