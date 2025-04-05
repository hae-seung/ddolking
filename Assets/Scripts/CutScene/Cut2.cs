
public class Cut2 : Cut
{
    public override void ShowText(string sceneName)
    {
        switch (sceneName)
        {
            case "chat1":
                cameraTween.endValueColor = cameraBackground[0];
                CutManager.Instance.ShowText(texts[0]);
                break;
        }
        cameraTween.CreateTween(true, true);
    }
}
