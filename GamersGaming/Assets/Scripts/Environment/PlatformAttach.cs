using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlatformAttach : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            other.transform.parent = transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            other.transform.parent = null;
    }
}
