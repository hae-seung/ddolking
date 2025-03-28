using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private AudioClip lobbyBgm;
    [SerializeField] private GameObject titleLogo;

    private void Awake()
    {
        titleLogo.SetActive(true);
    }
    
    private void Start()
    {
        PlayLobbyBgm();
    }

    private void PlayLobbyBgm()
    {
        AudioManager.Instance.PlayBgm(lobbyBgm);
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadCutScene()
    {
        SceneManager.LoadScene("CutScene_1");
    }
}
