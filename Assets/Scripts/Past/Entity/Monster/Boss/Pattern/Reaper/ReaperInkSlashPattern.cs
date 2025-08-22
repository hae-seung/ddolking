using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReaperInkSlashPattern : MonoBehaviour
{
    public UnityEvent onHit;
    public UnityEvent onStop;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
            onHit.Invoke();
    }

    private void OnParticleSystemStopped()
    {
        onStop?.Invoke();
    }
}
