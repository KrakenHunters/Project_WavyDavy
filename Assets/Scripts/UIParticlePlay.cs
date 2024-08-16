using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIParticlePlay : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particleSystem;
    private void Start()
    {
        _particleSystem.gameObject.SetActive(true);
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
