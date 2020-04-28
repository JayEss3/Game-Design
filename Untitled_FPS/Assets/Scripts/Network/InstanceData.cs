using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InstanceData : NetworkBehaviour
{
    [SyncVar] public int instanceID;
    public Transform connectionSpawnLocation;
}
