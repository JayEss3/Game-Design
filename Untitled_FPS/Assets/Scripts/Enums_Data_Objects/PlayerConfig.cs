using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Config/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    public float mouseXSensitivity = 10f;
    public float mouseYSensitivity = 10f;
    public float volumeLevel = 1f;
}
