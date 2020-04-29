using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InstanceManager : NetworkBehaviour
{
    public List<Transform> respawnPoints;
    public List<Transform> weaponSpawnLocations;
    public List<GameObject> spawnableWeapons;
    public int maxPlayerCount = 8;
    [SyncVar(hook=nameof(CheckIfReady))] public int currentPlayerCount = 0;
    //public delegate void StartMatchDelegate();
    //[SyncEvent] public event StartMatchDelegate EventStartMatch;

    public void SpawnWeapons()
    {
        foreach(var location in weaponSpawnLocations)
        {
            var weaponIndex = Random.Range(0,spawnableWeapons.Count);
            var weapon = Instantiate(spawnableWeapons[weaponIndex]);
            weapon.transform.position = location.position;
            NetworkServer.Spawn(weapon);
        }
    }

    public void CheckIfReady(int oldCount, int newCount)
    {
        if (maxPlayerCount == newCount)
            CmdStartMatch();
    }
    [Command]
    public void CmdStartMatch()
    {

    }
}
