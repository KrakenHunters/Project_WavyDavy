using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIParticlePlay : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private void Start()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();
    }
    void Update()
    {
        if (_particleSystem != null && GetComponent<CanvasGroup>().alpha >= 1f)
        {
            _particleSystem.Play();
        }
        else
        {
            _particleSystem.Stop();
        }
    }
}
