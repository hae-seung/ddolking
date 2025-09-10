using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashParticle : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        Destroy(gameObject);
    }
}
