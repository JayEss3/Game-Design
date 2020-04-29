using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Weapon : NetworkBehaviour
{
    [SerializeField] private float _fireRate = 1;
    [SerializeField] private float _reloadTime = 1;
    [SerializeField] private int _magSize = 1 ;
    [SerializeField] private Transform _bulletFireLocation = null;
    [SerializeField] private float _damage = 1;
    [SerializeField] private float _effectiveRange = 1;
    [SerializeField] private FireType _fireType = FireType.Automatic;

    [SyncVar(hook = nameof(OnWeaponChange))]
    public WeaponType weapon = WeaponType.None;

    private int m_roundsRemaining = 0;
    private bool isShooting = false;
    public Player owningPlayer;
    public AudioClip fireSound = null;
    private bool isReloading;

    public int GetMaxAmmo() { return _magSize; }
    public int GetRoundsRemaining() { return m_roundsRemaining; }

    private void OnWeaponChange(WeaponType oldWeaponType, WeaponType newWeaponType)
    {
        StartCoroutine(ChangeWeapon(newWeaponType));
    }
    private IEnumerator ChangeWeapon(WeaponType newWeaponType)
    {
        while(transform.childCount > 0) // remove old weapon
        {
            Destroy(transform.GetChild(0).gameObject);
            yield return null;
        }
        SetNewWeapon(newWeaponType);
    }
    public void SetNewWeapon(WeaponType newWeaponType)
    {
        switch (newWeaponType)
        {
            case WeaponType.TestPistol:
                var testPistol = Resources.Load<GameObject>("Test Pistol");
                Instantiate(testPistol, transform);
                break;
            case WeaponType.TestRifle:
                var testRifle = Resources.Load<GameObject>("Test Rifle");
                Instantiate(testRifle, transform);
                break;
            case WeaponType.TestSniperRifle:
                var testSniperRifle = Resources.Load<GameObject>("Test Sniper Rifle");
                Instantiate(testSniperRifle, transform);
                break;
        }
    }

    public void Awake()
    {
        m_roundsRemaining = _magSize;
    }
    public void PressShoot()
    {
        if (!isShooting && _fireType == FireType.SingleAction)
        {
            Fire();
            isShooting = true;
            StartCoroutine(ShootBullet());
        }
        if (_fireType == FireType.SemiAutomatic)
        {
            Fire();
        }
    }
    public void HoldShoot()
    {
        if (!isShooting && _fireType == FireType.Automatic)
        {
            Fire();
            isShooting = true;
            StartCoroutine(ShootBullet());
        }
    }

    public void Reload()
    {
        StartCoroutine(StartReload());
    }
    private IEnumerator StartReload()
    {
        isReloading = true;
        yield return new WaitForSeconds(_reloadTime);
        m_roundsRemaining = _magSize;
        gameObject.SendMessageUpwards("SetCurrentAmmo", m_roundsRemaining);
        isReloading = false;
    }

    private void Fire()
    {
        if (m_roundsRemaining > 0 && !isReloading)
        {
            GetComponent<AudioSource>().Play();
            gameObject.SendMessageUpwards("PlayWeaponSound", gameObject);
            var ray = new Ray(_bulletFireLocation.position, _bulletFireLocation.forward);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, _effectiveRange)) {
                Debug.Log($"I hit something : {hitInfo.transform.name}");
                if (hitInfo.transform.tag == "Player")
                {
                    owningPlayer.ApplyDamage(hitInfo.transform.gameObject, _damage);
                    Debug.Log("Sending to Owning player");
                }
            }
            m_roundsRemaining--;
            gameObject.SendMessageUpwards("SetCurrentAmmo", m_roundsRemaining);
        }
    }
    private IEnumerator ShootBullet()
    {
        yield return new WaitForSeconds(1 / _fireRate);
        isShooting = false;
    }
}

public enum FireType
{
    SingleAction,
    SemiAutomatic,
    Automatic
}
