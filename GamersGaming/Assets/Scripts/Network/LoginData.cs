using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LoginData", menuName = "Config/LoginData")]
public class LoginData : ScriptableObject
{
    public string username;
    public string displayName;
    public string email;
    public string password;
}
