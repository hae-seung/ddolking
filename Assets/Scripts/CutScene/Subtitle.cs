using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Subtitle : MonoBehaviour
{
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
        subtitleText.text = "";
        msg = text;
        StartCoroutine(TypingMsg());
    }

    private IEnumerator TypingMsg()
    {
        StringBuilder stringBuilder = new StringBuilder();
        
        int len = msg.Length;
        for (int i = 0; i < len; i++)
        {
            stringBuilder.Append(msg[i]);
            subtitleText.text = stringBuilder.ToString();
            yield return typingWait;
        }
    }
}
