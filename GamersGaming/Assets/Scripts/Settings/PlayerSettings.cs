using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Config/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public float MoveSpeed = 5f;
    public float SprintSpeed = 10f;
    public float Gravity = -20f;
    public float JumpHeight = 1.5f;
    public LayerMask GroundMask;
}
