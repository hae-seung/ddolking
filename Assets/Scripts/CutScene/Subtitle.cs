using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Subtitle : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    private Text subtitleText;
    private WaitForSeconds typingWait;
    private string msg;

    private void Awake()
    {
        subtitleText = GetComponent<Text>();
        typingWait = new WaitForSeconds(0.07f);
    }

    public void ShowText(string text)
    {
        gameObject.SetActive(true);
        subtitleText.text = "";
        msg = text;
        if (msg.Length != 0)
        {
            _audioSource.Play();
            StartCoroutine(TypingMsg());
        }
    }

    private IEnumerator TypingMsg()
    {
        StringBuilder stringBuilder = new StringBuilder();
        
        int len = msg.Length;
        for (int i = 0; i < len; i++)
        {
            stringBuilder.Append(msg[i]);
            subtitleText.text = stringBuilder.ToString();
            _audioSource.pitch = Random.Range(0.8f, 1.2f);
            yield return typingWait;
        }
        _audioSource.Stop();
    }

    public void SetTextNULL()
    {
        gameObject.SetActive(false);
        subtitleText.text = "";
    }
}
