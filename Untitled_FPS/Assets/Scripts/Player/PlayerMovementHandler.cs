using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMovementHandler : NetworkBehaviour
{
    [SerializeField] private CharacterController _currentController = null;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _boostSpeed = 100f;
    [SerializeField] private float _boostTime = 0.25f;
    [SerializeField] private float _boostRemaining = 0f;
    [SerializeField] private bool boosted = false; // if the tagger has boosted in the air already. Reset this on player contact.
    [SerializeField] private float _sprintSpeedMutliplyer = 1.75f;
    [SerializeField] private float _gravity = 9.79f;
    [SerializeField] private float _jumpForce = 4f;

    private float m_downVelocity = 0f;
    private Player _player = null;
    private Vector2 _rotation = new Vector2();
    private Quaternion _playerTargetRot;
    private Quaternion _cameraTargetRot;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        _player = GetComponent<Player>();
        _playerTargetRot = transform.rotation;
        _cameraTargetRot = _player.GetPlayerCamera().transform.rotation;
    }

    private void Update()
    {
        if (!base.hasAuthority)
            return;

        if (!Cursor.visible && _boostRemaining <= 0)
        {
            if (Input.GetMouseButtonDown(0) && !boosted)
            {
                boosted = true;
                _boostRemaining = _boostTime;
                _currentController.Move(Camera.main.transform.forward * Time.deltaTime * _boostSpeed);
                return;
            }
            // Player Ground Movement
            var inputMove = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
                inputMove += transform.forward;
            if (Input.GetKey(KeyCode.S))
                inputMove -= transform.forward;
            if (Input.GetKey(KeyCode.A))
                inputMove -= transform.right;
            if (Input.GetKey(KeyCode.D))
                inputMove += transform.right;

            if (Input.GetKey(KeyCode.LeftShift))
                _currentController.Move(inputMove * Time.deltaTime * _moveSpeed * _sprintSpeedMutliplyer);
            else
                _currentController.Move(inputMove * Time.deltaTime * _moveSpeed);

            // Camera Controller
            _rotation.x = Input.GetAxis("Mouse X");
            _rotation.y = Input.GetAxis("Mouse Y");
            _playerTargetRot *= Quaternion.Euler(0f, _rotation.x, 0f);
            _cameraTargetRot *= Quaternion.Euler(-_rotation.y, 0f, 0f);
            _cameraTargetRot = LockCameraMovement(_cameraTargetRot);
            _player.GetPlayerCamera().localRotation = _cameraTargetRot;
            transform.localRotation = _playerTargetRot;

            // Jumping
            if (isGrounded())
            {
                boosted = false;
                m_downVelocity = 0f;
            }
            if (isGrounded() && Input.GetKey(KeyCode.Space))
                m_downVelocity = _jumpForce;
        }
        else
        {
            _boostRemaining -= Time.deltaTime;
            if (_boostRemaining > 0f)
                _currentController.Move(Camera.main.transform.forward * Time.deltaTime * _boostSpeed);
        }

        m_downVelocity -= _gravity * Time.deltaTime;
        _currentController.Move(new Vector3(0f, m_downVelocity * Time.deltaTime, 0f));
    }

    // Util functions
    private bool isGrounded()
    {
        return Physics.CheckSphere(transform.position - new Vector3(0f, 0.1f, 0f), 0.05f);
    }

    private Quaternion LockCameraMovement(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        var angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, -85, 90);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}
