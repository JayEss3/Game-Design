using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class Player : NetworkBehaviour
{
    //54.196.241.190
    [Header("Synced Vars")]
    [SyncVar(hook =nameof(UpdateHealth))] public float health = 100f;
    [SyncVar] public string playerType = "None";
    [SyncVar] public string playerName = "Player";
    [Header("Player User Interface")]
    public TextMeshProUGUI healthText = null;
    public RectTransform healthBar = null;
    [Header("Audio")]
    public List<AudioClip> audioClips = new List<AudioClip>();
    public AudioSource audioSource = null;

    private Transform m_camera = null;
    public Transform GetPlayerCamera() { return m_camera; }
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        m_camera = Camera.main.transform;
        Camera.main.orthographic = false;
        m_camera.GetComponent<AudioListener>().enabled = true;
        m_camera.SetPositionAndRotation(transform.position + new Vector3(0f, 1.65f, 0f), transform.rotation);
        m_camera.SetParent(transform.GetChild(0));
    }
    public void UpdateHealth(float oldHealth, float newHealth)
    {
        if(newHealth <= 100)
        {
            healthText.text = newHealth.ToString();
            healthBar.sizeDelta = new Vector2((newHealth / 100) * 200, healthBar.sizeDelta.y);
        }
        else
        {
            healthText.text = newHealth.ToString();
            healthBar.sizeDelta = new Vector2((newHealth / 2500) * 200, healthBar.sizeDelta.y);
        }
    }
    public void ApplyDamage(GameObject target, float damage)
    {
        audioSource.clip = audioClips[0];
        audioSource.Play();
        CmdApplyDamage(target, damage);
        Debug.Log("Sending message to server");
    }
    [Command]
    public void CmdApplyDamage(GameObject target, float damage)
    {
        target.GetComponent<Player>().health -= damage;
        if (target.GetComponent<Player>().health <= 0) {
            var pim = target.GetComponent<PlayerInstanceManager>();
            if (pim.inInstance)
            {
                var im = pim.currentPlayerInstance.GetComponent<InstanceManager>();
                var spawnIndex = Random.Range(0, im.respawnPoints.Count);
                var spawnLocation = im.respawnPoints[spawnIndex];
                var tarply = target.GetComponent<Player>();
                if(tarply.playerType == "Tagger")
                    tarply.health = 2500;
                else
                    tarply.health = 100;
                RpcRespawnPlayer(spawnLocation.position, target);
            }
        }
    }
    [ClientRpc]
    public void RpcRespawnPlayer(Vector3 pos, GameObject player)
    {
        player.transform.position = pos;
        var paudioSource = player.GetComponent<AudioSource>();
        paudioSource.clip = audioClips[1];
        paudioSource.Play();
    }
}
