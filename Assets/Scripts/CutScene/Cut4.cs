using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cut4 : Cut
{
    public override void ShowText(string sceneName)
    {
        switch (sceneName)
        {
            case "chat1":
                cameraTween.endValueColor = cameraBackground[0];
                CutManager.Instance.ShowText(texts[0]);
                break;
            case "chat2":
                cameraTween.endValueColor = cameraBackground[1];
                CutManager.Instance.ShowText(texts[1]);
                break;
            case "chat3":
                cameraTween.endValueColor = cameraBackground[2];
                CutManager.Instance.ShowText(texts[2]);
                break;
        }
        cameraTween.CreateTween(true, true);
    }
}
