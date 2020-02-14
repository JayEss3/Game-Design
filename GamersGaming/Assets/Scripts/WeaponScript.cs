using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField] private Transform _fireLocation;
    [SerializeField] private GameObject _projectile;
    public void Shoot()
    {
        var spawnedObject = Instantiate(_projectile, _fireLocation.position, transform.parent.rotation);
        spawnedObject.transform.parent = transform;
        spawnedObject.SendMessage("Fire");
    }
}
