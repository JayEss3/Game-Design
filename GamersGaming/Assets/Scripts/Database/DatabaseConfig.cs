using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DatabaseConfig", menuName = "Config/DatabaseConfig")]
public class DatabaseConfig : ScriptableObject
{
    public string host;
    public string user;
    public string pass;
    public string database;
}
