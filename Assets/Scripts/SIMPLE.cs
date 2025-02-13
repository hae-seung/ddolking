using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SIMPLE : MonoBehaviour
{
    public int experience;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            GameEventsManager.Instance.playerEvents.GainExperience(experience);
            Destroy(gameObject);
        }
    }
}
