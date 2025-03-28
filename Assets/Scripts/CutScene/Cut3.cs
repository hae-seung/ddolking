

public class Cut3 : Cut
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
                CutManager.Instance.ShowText(texts[1]);
                break;
            case "chat3":
                CutManager.Instance.ShowText(texts[2]);
            break;
            case "chat4":
                cameraTween.endValueColor = cameraBackground[3];
                CutManager.Instance.ShowText(texts[3]);
                break;
        }
        cameraTween.CreateTween(true, true);
    }
}
