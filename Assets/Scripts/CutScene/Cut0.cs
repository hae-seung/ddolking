using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Cut0 : MonoBehaviour
{
    [SerializeField] private DOTweenAnimation cameraTween;
    [SerializeField] private List<Color> cameraBackground;
    
    [SerializeField] private List<string> texts;
    
    public void SceneTrigger(string sceneName)
    {
        switch (sceneName)
        {
            case "scene0":
                cameraTween.endValueColor = cameraBackground[0];
                CutManager.Instance.ShowText(texts[0]);
                break;
            case "scene1":
                cameraTween.endValueColor = cameraBackground[1];
                CutManager.Instance.ShowText(texts[1]);
                break;
            
        }
        cameraTween.CreateTween(true, true);
    }
    
}
