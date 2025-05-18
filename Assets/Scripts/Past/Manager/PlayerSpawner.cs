using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Interactor player;
    [SerializeField] private AudioClip pastBgm;
    [SerializeField] private Light2D light;
    
    private void Start()
    {
        SpawnPlayer();
        PlayBgm();
    }

    private void PlayBgm()
    { 
       AudioManager.Instance.PlayBgm(pastBgm);
    }


    private void SpawnPlayer()
    {
        player.transform.position = spawnPoint.position;
    }
}
