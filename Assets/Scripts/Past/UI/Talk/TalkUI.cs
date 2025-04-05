using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using KoreanTyper;
using Random = UnityEngine.Random;


//한개의 대사만 출력하는 역할
public class TalkUI : MonoBehaviour
{
    [SerializeField] private Text speaker;
    [SerializeField] private Text speakerJob;
    [SerializeField] private TextMeshProUGUI context;
    [SerializeField] private Image portrait;
    [SerializeField] private Button arrow;
    [SerializeField] private AudioClip typingSFX;

    public bool isFinish { get; private set; }
    
    private string originText;

    private void Awake()
    {
        arrow.onClick.AddListener(() =>
        {
            isFinish = true;//화살표를 눌러야 한 대화를 모두 읽었음을 의미
            arrow.gameObject.SetActive(false);
        });
    }

    public void SetTalk(TalkData data, NPCData npc)
    {
        speaker.text = npc.npcName;
        speakerJob.text = npc.npcJob;
        for (int i = 0; i < npc.npcPortrait.Count; i++)
        {
            if (data.portaritId.Equals(npc.npcPortrait[i].portraitId))
            {
                portrait.sprite = npc.npcPortrait[i].portrait;
                break;
            }
        }
        
        originText = data.context;
        isFinish = false;
        arrow.gameObject.SetActive(false);
        StartCoroutine(TypingText());
    }

    private IEnumerator TypingText()
    {
        int typingLength = originText.GetTypingLength();

        for (int i = 0; i <= typingLength; i++)
        {
            context.text = originText.Typing(i);
            if(i%2 == 0)
            {
               AudioManager.Instance.PlaySfx(typingSFX);
            }
            yield return new WaitForSeconds(0.04f);
        }
        arrow.gameObject.SetActive(true);
    }
}
