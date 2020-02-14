using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private ParticleSystem _destructionParticleSystem;
    [SerializeField] private AudioSource _destructionAudio;
    public void ApplyDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
            StartCoroutine(Destruction());
    }
    private IEnumerator Destruction()
    {
        _destructionParticleSystem.Play();
        _destructionAudio.Play();
        var meshRender = GetComponent<MeshRenderer>();
        meshRender.enabled = false;
        yield return new WaitForSecondsRealtime(2f);
        Invoke("Respawn", 10f);
    }

    private void Respawn()
    {
        var meshRender = GetComponent<MeshRenderer>();
        meshRender.enabled = true;
        _health = 100;
    }
}
