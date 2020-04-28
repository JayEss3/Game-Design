using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class ShootingHandler : NetworkBehaviour
{
    [SerializeField] private GameObject _weaponParent = null;
    [SerializeField] private TextMeshProUGUI _currentAmmo = null;
    [SerializeField] private TextMeshProUGUI _maxAmmo = null;
    [SerializeField] private GameObject _weaponUI = null;
    public AudioSource audioSource = null;
    public AudioSource playerAudio = null;
    public AudioClip pickUpAudio = null;
    public AudioClip reloadSound = null;

    [SyncVar(hook = nameof(OnChangedWeapon))]
    public WeaponType m_currentWeapon = WeaponType.None;
    public AudioClip weaponSound = null;
    void OnChangedWeapon(WeaponType oldWeapon, WeaponType newWeapon)
    {
        StartCoroutine(ChangeWeapon(newWeapon));
    }
    private IEnumerator ChangeWeapon(WeaponType newWeapon)
    {
        if(_weaponParent.transform.childCount != 0)
            Destroy(_weaponParent.transform.GetChild(0).gameObject);
        yield return null;
        switch (newWeapon)
        {
            case WeaponType.TestPistol:
                var testPistol = Resources.Load<GameObject>("Test Pistol");
                Instantiate(testPistol, _weaponParent.transform);
                SetupWeapon();
                break;
            case WeaponType.TestRifle:
                var testRifle = Resources.Load<GameObject>("Test Rifle");
                Instantiate(testRifle, _weaponParent.transform);
                SetupWeapon();
                break;
            case WeaponType.TestSniperRifle:
                var testSniperRifle = Resources.Load<GameObject>("Test Sniper Rifle");
                Instantiate(testSniperRifle, _weaponParent.transform);
                SetupWeapon();
                break;
            case WeaponType.None:
                if (!base.hasAuthority)
                    break;
                _weaponUI.SetActive(false);
                break;
        }
    }

    private void SetupWeapon()
    {
        _weaponParent.transform.GetChild(0).GetComponent<Rigidbody>().useGravity = false;
        _weaponParent.transform.GetChild(0).GetComponent<MeshCollider>().enabled = false;
        _weaponParent.transform.GetChild(0).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        _weaponParent.transform.GetChild(0).GetComponent<Weapon>().owningPlayer = gameObject.GetComponent<Player>();
        weaponSound = _weaponParent.transform.GetChild(0).GetComponent<Weapon>().fireSound;
        if (!base.hasAuthority)
            return;
        SetCurrentAmmo(_weaponParent.transform.GetChild(0).GetComponent<Weapon>().GetRoundsRemaining());
        SetMaxAmmo(_weaponParent.transform.GetChild(0).GetComponent<Weapon>().GetMaxAmmo());
        _weaponUI.SetActive(true);
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        _weaponParent.transform.parent = Camera.main.transform;
    }

    private void Update()
    {
        if (!base.hasAuthority)
            return;
        if (Cursor.visible)
            return;

        if (m_currentWeapon != WeaponType.None)
        {
            if (Input.GetMouseButtonDown(0))
                _weaponParent.transform.GetChild(0).SendMessage("PressShoot");
            else if (Input.GetMouseButton(0))
                _weaponParent.transform.GetChild(0).SendMessage("HoldShoot");
            if (Input.GetKeyDown(KeyCode.R))
            {
                playerAudio.clip = reloadSound;
                playerAudio.Play();
                _weaponParent.transform.GetChild(0).SendMessage("Reload");
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (m_currentWeapon != WeaponType.None)
                CmdDropWeapon();
            else
            {
                RaycastHit[] hits;
                hits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, 5f);
                for (int i = 0; i < hits.Length; i++)
                {
                    RaycastHit hit = hits[i];
                    if (hit.transform.tag == "Weapon" && m_currentWeapon == WeaponType.None)
                    {
                        playerAudio.clip = pickUpAudio;
                        playerAudio.Play();
                        CmdPickUpWeapon(hit.transform.gameObject);
                    }
                }
            }
        }
    }

    [Command]
    private void CmdDropWeapon()
    {
        var pos = _weaponParent.transform.position;
        var rot = _weaponParent.transform.rotation;
        GameObject weaponToDrop = null;
        switch (m_currentWeapon)
        {
            case WeaponType.TestPistol:
                var testPistol = Resources.Load<GameObject>("Test Pistol");
                weaponToDrop = Instantiate(testPistol, pos, rot);
                break;
            case WeaponType.TestRifle:
                var testRifle = Resources.Load<GameObject>("Test Rifle");
                weaponToDrop = Instantiate(testRifle, pos, rot);
                break;
            case WeaponType.TestSniperRifle:
                var testSniperRifle = Resources.Load<GameObject>("Test Sniper Rifle");
                weaponToDrop = Instantiate(testSniperRifle, pos, rot);
                break;
        }

        m_currentWeapon = WeaponType.None;
        NetworkServer.Spawn(weaponToDrop);
    }

    [Command]
    private void CmdPickUpWeapon(GameObject weapon)
    {
        m_currentWeapon = weapon.GetComponent<Weapon>().weapon;       
        NetworkServer.Destroy(weapon);
    }

    public void PlayWeaponSound(GameObject weapom)
    {
        CmdPlayWeaponSound(weapom);
    }
    [Command]
    public void CmdPlayWeaponSound(GameObject weapom)
    {
        RpcPlayWeaponSound(weapom);
    }
    [ClientRpc]
    public void RpcPlayWeaponSound(GameObject weapom)
    {
        var audioSource = _weaponParent.transform.GetChild(0).GetComponent<AudioSource>();
        audioSource.Play();
    }
    public void SetMaxAmmo(int maxAmmo)
    {
        if (!base.hasAuthority)
            return;
        _maxAmmo.text = maxAmmo.ToString();
    }

    public void SetCurrentAmmo(int currentAmmo)
    {
        if (!base.hasAuthority)
            return;
        _currentAmmo.text = currentAmmo.ToString();
    }
}
