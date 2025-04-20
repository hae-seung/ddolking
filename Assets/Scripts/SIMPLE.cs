using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SIMPLE : MonoBehaviour
{
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("dd");
        if (other.name == "Player")
        {
            SceneManager.LoadScene("Example");
        }
    }
}
