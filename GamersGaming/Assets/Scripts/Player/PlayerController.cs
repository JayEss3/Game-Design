using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;
public class PlayerController : NetworkBehaviour
{
    [Header("Objects")]
    [SerializeField] private KeyBindings keyBindings;
    [SerializeField] private PlayerSettings playerSettings;
    [SerializeField] private PlayerConfig playerConfig;
    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private TextMeshProUGUI playerNameplate;
    [SerializeField] private Canvas canvas;
    [SerializeField] private UserInterfaceManager userInterfaceManager;
    // Privates
    Vector3 velocity;
    bool isGrounded;
    // Private References
    float groundDistance = 0.05f;
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        playerNameplate.text = playerConfig.playerName;
        if (!keyBindings)
            keyBindings = Resources.Load<KeyBindings>("Objects/KeyBindings");
        if (!playerSettings)
            playerSettings = Resources.Load<PlayerSettings>("Objects/PlayerSettings");
        if (!playerConfig)
            playerConfig = Resources.Load<PlayerConfig>("Objects/PlayerConfig");
        if (!characterController)
            characterController = transform.GetComponent<CharacterController>();
        if (!userInterfaceManager)
            userInterfaceManager = GameObject.Find("Canvas").GetComponent<UserInterfaceManager>();
    }
    private void Awake()
    {
        canvas.worldCamera = Camera.main;
    }
    private void OnValidate()
    {
        if (!keyBindings)
            keyBindings = Resources.Load<KeyBindings>("Objects/KeyBindings");
        if (!playerSettings)
            playerSettings = Resources.Load<PlayerSettings>("Objects/PlayerSettings");
        if (!playerConfig)
            playerConfig = Resources.Load<PlayerConfig>("Objects/PlayerConfig");
        if (!characterController)
            characterController = transform.GetComponent<CharacterController>();
        if (!groundCheck)
            Debug.LogWarning($"{gameObject.name} is missing GroundCheck");
    }
    private void Update()
    {
        if (!isLocalPlayer) return;
        MovePlayer();
        HandleInteraction();
        if (Input.GetKeyDown(keyBindings.Menu))
            userInterfaceManager.MenuToggle();
    }
    private void HandleInteraction()
    {
        if (Input.GetKeyDown(keyBindings.Interact))
        {
            var colliders = Physics.OverlapSphere(transform.position, 1);
            foreach (Collider collider in colliders)
            {
                try { collider.gameObject.SendMessage("Interact", gameObject); }
                catch { }
            }
        }
    }
    private void MovePlayer()
    {
        CheckGround();
        var inputs = HandleMoveInputs();
        var move = transform.right * inputs.x + transform.forward * inputs.y;
        if (Input.GetKey(keyBindings.Sprint))
            characterController.Move(move * playerSettings.SprintSpeed * Time.deltaTime);
        else
            characterController.Move(move * playerSettings.MoveSpeed * Time.deltaTime);
        HandleJump();
        HandleGravity();
    }
    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position - new Vector3(0, groundDistance, 0), groundDistance);
        if (isGrounded)
            velocity.y = playerSettings.Gravity;
    }
    private Vector2 HandleMoveInputs()
    {
        float x = 0f, z = 0f;
        if (Input.GetKey(keyBindings.Forward))
            z += 1;
        if (Input.GetKey(keyBindings.Backward))
            z -= 1;
        if (Input.GetKey(keyBindings.Right))
            x += 1;
        if (Input.GetKey(keyBindings.Left))
            x -= 1;
        return new Vector2(x, z);
    }
    private void HandleJump()
    {
        if (Input.GetButton("Jump") && (isGrounded || transform.parent))
            velocity.y = Mathf.Sqrt(3 * -2 * playerSettings.Gravity);
        else if (transform.parent)
            velocity.y = 0f;
    }
    private void HandleGravity() {
        if (!transform.parent)
            velocity.y += playerSettings.Gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}