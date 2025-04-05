using System.Collections;
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
        StartCoroutine(PlayLobbyBgm());
    }

    private IEnumerator PlayLobbyBgm()
    {
        yield return new WaitUntil(() => AudioManager.Instance.isFinishInit);
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
