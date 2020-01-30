using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginUI : MonoBehaviour
{
    public void LAN_Game()
    {
        SceneManager.LoadScene(sceneName: "LAN_Main");
    }
}
