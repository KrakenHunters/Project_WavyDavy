using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    void Update() => transform.position = target.position;
}
