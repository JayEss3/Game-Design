using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerCameraController : NetworkBehaviour
{
    [Header("Objects")]
    [SerializeField] private PlayerConfig playerConfig;
    // Privates
    float xRotation = 0f;
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        Camera.main.orthographic = false;
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0f, 1.6f, 0f);
        Camera.main.transform.localEulerAngles = new Vector3(10f, 0f, 0f);
    }
    void OnDisable()
    {
        if (isLocalPlayer)
        {
            Camera.main.orthographic = true;
            Camera.main.transform.SetParent(null);
            Camera.main.transform.localPosition = new Vector3(0f, 70f, 0f);
            Camera.main.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
        }
    }
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void OnValidate()
    {
        if (!playerConfig)
            playerConfig = Resources.Load<PlayerConfig>("Objects/PlayerConfig");
    }
    private void Update()
    {
        if (!isLocalPlayer) return;
        var mouseX = Input.GetAxis("Mouse X") * playerConfig.mouseSensitivity * Time.deltaTime;
        var mouseY = Input.GetAxis("Mouse Y") * playerConfig.mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
