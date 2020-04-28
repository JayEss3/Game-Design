using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class InstanceMaster : NetworkBehaviour
{
    public string instanceName = null;
    public Sprite instanceImage = null;
    public float spawnOffset = 2000f; 
    public GameObject instanceMap = null;
    public SyncListInt spawnedInstances = new SyncListInt();

    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.GetComponent<NetworkIdentity>().isLocalPlayer)
            return;

        if (collision.transform.tag == "Player")
        {
            collision.GetComponent<PlayerUIHandler>().FillInstaceUI(gameObject, instanceName, instanceImage);
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (!collision.GetComponent<NetworkIdentity>().isLocalPlayer)
            return;

        if (collision.transform.tag == "Player")
            collision.GetComponent<PlayerUIHandler>().ClearInstanceUI();
    }
}